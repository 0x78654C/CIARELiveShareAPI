using CIARELiveShareAPI.Utils;
using Microsoft.AspNetCore.SignalR;


namespace CIARELiveShareAPI.Hubs;

public class LiveShare : Hub
{
    /// <summary>
    /// Send/Receive code to connection id using speficic session ID attashed
    /// </summary>
    public void GetSendCode(string sessionId, string code, string position)
    {
        try
        {
            var connectionId = Context.ConnectionId;
            var listKeys = GlobalVariables.listKeys;
            var countSessionIds = listKeys.Count(x => x.Contains(sessionId));
            var countId = listKeys.Count(x => x.Contains(connectionId));
            var pattern = $"{sessionId}|{connectionId}";
            SendHostData(sessionId, code, connectionId);
            if (countSessionIds < 2 && countId < 1)
                listKeys.Add(pattern);
            foreach (var item in listKeys.Select(x=> x.Split('|')))
            {
                var sessionKey = item.First();
                var con = item[1];
                if (sessionKey == sessionId && con != connectionId)
                    Clients.Client(con).SendAsync("GetSend", code, position, connectionId);
            }
        }
        catch
        {
            // ignored
        }
    }

    /// <summary>
    /// Store code from live share host on first connection
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="data"></param>
    /// <param name="connectionId"></param>
    private void SendHostData(string sessionId, string data, string connectionId)
    {
        var hostData = GlobalVariables.hostData;
        if (data != "remote")
        {
            if (!hostData.ContainsKey(sessionId))
            {
                hostData.Add(sessionId, data);
            }
            return;
        }

        if (GlobalVariables.hostData.TryGetValue(sessionId, out var value))
        {
            Clients.Client(connectionId).SendAsync("GetSend", value, "0|0", connectionId);
            hostData.Remove(sessionId);
        }
    }

    /// <summary>
    /// Remove data from dictionary on client disconnect.
    /// </summary>
    /// <param name="connectionId"></param>
    private static void RemoveHostData(string connectionId)
    {
        string sId = GlobalVariables.listKeys.FirstOrDefault(x => x.Contains(connectionId))?.Split('|').First();
        if (sId is null) return;
        foreach (var s in GlobalVariables.hostData.Where(x => x.Key.Contains(sId)))
        {
            GlobalVariables.hostData.Remove(s.Key);
        }
    }

    /// <summary>
    /// Remove pattern from list on disconnect and display disconnected client in console.
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        try
        {
            var connectionId = Context.ConnectionId;
            RemoveHostData(connectionId);
            if (GlobalVariables.listKeys.Count > 0)
                GlobalVariables.listKeys.RemoveAll(x => x.Contains(connectionId));

        }
        catch
        {
            // ignored
        }

        await base.OnDisconnectedAsync(exception);
    }
}
