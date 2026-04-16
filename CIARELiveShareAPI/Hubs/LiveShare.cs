using CIARELiveShareAPI.Utils;
using Microsoft.AspNetCore.SignalR;

namespace CIARELiveShareAPI.Hubs;

public class LiveShare : Hub
{
    private readonly ILogger<LiveShare> _logger;

    private const int MaxSessionIdLength = 128;
    private const int MaxCodeLength = 5_000_000;
    private const int MaxPositionLength = 64;

    public LiveShare(ILogger<LiveShare> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Send/Receive code to connection id using specific session ID attached.
    /// </summary>
    public async Task GetSendCode(string sessionId, string code, string position)
    {
        if (string.IsNullOrEmpty(sessionId) || sessionId.Length > MaxSessionIdLength)
            return;
        if (code?.Length > MaxCodeLength)
            return;
        if (position?.Length > MaxPositionLength)
            return;

        try
        {
            var connectionId = Context.ConnectionId;
            var connections = GlobalVariables.connections;

            await SendHostData(sessionId, code, connectionId);

            // Register connection if not already registered
            if (!connections.ContainsKey(connectionId))
            {
                var sessionConnectionCount = connections.Count(x => x.Value == sessionId);
                if (sessionConnectionCount < GlobalVariables.MaxConnectionsPerSession)
                {
                    var activeSessionCount = connections.Values.Distinct().Count();
                    if (activeSessionCount < GlobalVariables.MaxSessions || connections.Values.Contains(sessionId))
                        connections.TryAdd(connectionId, sessionId);
                }
            }

            foreach (var kvp in connections)
            {
                if (kvp.Value == sessionId && kvp.Key != connectionId)
                    await Clients.Client(kvp.Key).SendAsync("GetSend", code, position, connectionId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetSendCode for session {SessionId}", sessionId);
        }
    }

    /// <summary>
    /// Store code from live share host on first connection.
    /// </summary>
    private async Task SendHostData(string sessionId, string data, string connectionId)
    {
        var hostData = GlobalVariables.hostData;
        if (data != "remote")
        {
            hostData.TryAdd(sessionId, data);
            return;
        }

        if (hostData.TryRemove(sessionId, out var value))
        {
            await Clients.Client(connectionId).SendAsync("GetSend", value, "0|0", connectionId);
        }
    }

    /// <summary>
    /// Remove data from dictionary on client disconnect.
    /// </summary>
    private static void RemoveHostData(string connectionId)
    {
        if (!GlobalVariables.connections.TryGetValue(connectionId, out var sessionId))
            return;

        var otherConnectionsExist = GlobalVariables.connections
            .Any(x => x.Value == sessionId && x.Key != connectionId);

        if (!otherConnectionsExist)
            GlobalVariables.hostData.TryRemove(sessionId, out _);
    }

    /// <summary>
    /// Remove connection on disconnect and clean up session data.
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        try
        {
            var connectionId = Context.ConnectionId;
            RemoveHostData(connectionId);
            GlobalVariables.connections.TryRemove(connectionId, out _);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in OnDisconnectedAsync");
        }

        await base.OnDisconnectedAsync(exception);
    }
}
