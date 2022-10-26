using Microsoft.AspNetCore.SignalR;

namespace CIARELiveShareAPI.Hubs;

public class LiveShare : Hub
{
    public Task GetSendCode (string code)
    {
        return Clients.All.SendAsync("GetSend", code);
    }
}
