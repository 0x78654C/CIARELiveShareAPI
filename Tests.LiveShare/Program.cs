using System.Collections.Concurrent;
using CIARELiveShareAPI.Hubs;
using CIARELiveShareAPI.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(Array.Empty<string>());
builder.Logging.ClearProviders();
builder.WebHost.UseUrls("http://127.0.0.1:0");
builder.Services.AddSignalR();
await using var server = builder.Build();
server.MapHub<DisconnectTestHub>("/live");
await server.StartAsync();
string url = server.Urls.Single() + "/live";
await using var host = new HubConnectionBuilder().WithUrl(url).Build();
await using var departing = new HubConnectionBuilder().WithUrl(url).Build();
await using var remaining = new HubConnectionBuilder().WithUrl(url).Build();
await using var unrelated = new HubConnectionBuilder().WithUrl(url).Build();
var hostNotifications = new ConcurrentQueue<string>();
var remainingNotifications = new ConcurrentQueue<string>();
var unrelatedNotifications = new ConcurrentQueue<string>();
host.On<string>("UserDisconnected", id => hostNotifications.Enqueue(id));
remaining.On<string>("UserDisconnected", id => remainingNotifications.Enqueue(id));
unrelated.On<string>("UserDisconnected", id => unrelatedNotifications.Enqueue(id));

await host.StartAsync();
await departing.StartAsync();
await remaining.StartAsync();
await unrelated.StartAsync();
await host.InvokeAsync("GetSendCode", "shared-session", "cached-document", "0|0|Host");
await departing.InvokeAsync("GetSendCode", "shared-session", string.Empty, "0|0|Guest");
await remaining.InvokeAsync("GetSendCode", "shared-session", string.Empty, "0|0|Guest");
await unrelated.InvokeAsync("GetSendCode", "other-session", string.Empty, "0|0|Other");
string departedId = departing.ConnectionId;
await departing.StopAsync();
await WaitUntil(() => hostNotifications.Count == 1 && remainingNotifications.Count == 1);
Assert(hostNotifications.Single() == departedId && remainingNotifications.Single() == departedId,
    "Every remaining session participant receives the departed connection ID");
Assert(!GlobalVariables.connections.ContainsKey(departedId), "Departed connection is removed");
Assert(GlobalVariables.connections.ContainsKey(remaining.ConnectionId), "Other participants stay registered");
Assert(GlobalVariables.hostData["shared-session"] == "cached-document", "Active session data is preserved");

await departing.StartAsync();
string rejoinedId = departing.ConnectionId;
Assert(rejoinedId != departedId, "Rejoining gets a new connection ID");
await departing.InvokeAsync("GetSendCode", "shared-session", string.Empty, "0|0|Guest");
await departing.SendAsync("AbortConnection");
await WaitUntil(() => hostNotifications.Count == 2 && remainingNotifications.Count == 2);
Assert(hostNotifications.Last() == rejoinedId && remainingNotifications.Last() == rejoinedId,
    "Unexpected connection loss also notifies remaining participants");
Assert(!GlobalVariables.connections.ContainsKey(rejoinedId), "Aborted connection is removed");
await Task.Delay(200);
Assert(unrelatedNotifications.IsEmpty, "Disconnect notifications stay within the departed user's session");

await using (var unregistered = new HubConnectionBuilder().WithUrl(url).Build())
{
    await unregistered.StartAsync();
    await unregistered.StopAsync();
}
await Task.Delay(200);
Assert(hostNotifications.Count == 2 && remainingNotifications.Count == 2 && unrelatedNotifications.IsEmpty,
    "Connections that never join a session do not produce participant notifications");

await remaining.StopAsync();
await WaitUntil(() => hostNotifications.Count == 3);
await host.StopAsync();
await WaitUntil(() => !GlobalVariables.hostData.ContainsKey("shared-session"));
Assert(GlobalVariables.connections.Values.All(session => session != "shared-session"),
    "The final departure removes the session's connections and cached document");
Assert(GlobalVariables.connections.ContainsKey(unrelated.ConnectionId), "Other sessions remain connected");
await unrelated.StopAsync();
await WaitUntil(() => GlobalVariables.connections.IsEmpty && GlobalVariables.hostData.IsEmpty);
Console.WriteLine("PASS: Live Share disconnect regression checks");

static async Task WaitUntil(Func<bool> condition)
{
    using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
    while (!condition())
        await Task.Delay(10, timeout.Token);
}

static void Assert(bool condition, string message)
{
    if (!condition)
        throw new InvalidOperationException(message);
    Console.WriteLine("PASS: " + message);
}

public sealed class DisconnectTestHub(ILogger<LiveShare> logger) : LiveShare(logger)
{
    public void AbortConnection() => Context.Abort();
}
