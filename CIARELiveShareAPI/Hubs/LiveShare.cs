using CIARELiveShareAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace CIARELiveShareAPI.Hubs;

public class LiveShare : Hub
{
    public void GetSendCode(string key, string code)
    {
        try
        {
            GlobalVariables.userKey = key;
            var connectionId = Context.ConnectionId;
            var listKeys = GlobalVariables.listKeys;
            var countKeys = listKeys.Where(x => x.Contains(key)).Count();
            var countId = listKeys.Where(x => x.Contains(key)).Count();
            var patern = $"{key}|{connectionId}";
            if (countKeys < 2)
                listKeys.Add(patern);
            foreach (var keyCon in listKeys)
            {
                string keyP = keyCon.Split('|')[0];
                string con = keyCon.Split('|')[1];
                if (keyP == key)
                    Clients.Client(con).SendAsync("GetSend", code);
            }
        }
        catch (Exception ex)
        {
            //Console.WriteLine(ex.ToString());
        }
    }

    /// <summary>
    /// Remove patern from list on disconnect.
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine($"{Context.ConnectionId} - Disconnected");
        if (GlobalVariables.listKeys.Count > 0)
            GlobalVariables.listKeys.RemoveAll(x => x.Contains(Context.ConnectionId));
        await base.OnDisconnectedAsync(exception);
    }


    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"{Context.ConnectionId} - Connected");
        await base.OnConnectedAsync();
    }
}
