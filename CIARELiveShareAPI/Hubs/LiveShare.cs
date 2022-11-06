using CIARELiveShareAPI.Utils;
using Microsoft.AspNetCore.SignalR;

namespace CIARELiveShareAPI.Hubs;

public class LiveShare : Hub
{
    /// <summary>
    /// Send/Receive code to connection id using speficic session ID attashed
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="code"></param>
    public void GetSendCode(string sessionId, string code)
    {
        try
        {
            var connectionId = Context.ConnectionId;
            var listKeys = GlobalVariables.listKeys;
            var countSessionIds = listKeys.Where(x => x.Contains(sessionId)).Count();
            var countId = listKeys.Where(x => x.Contains(connectionId)).Count();
            var patern = $"{sessionId}|{connectionId}";
            if (countSessionIds < 2 && countId < 1)
                listKeys.Add(patern);
            foreach (var sessionCon in listKeys)
            {
                string sessionKey = sessionCon.Split('|')[0];
                string con = sessionCon.Split('|')[1];
                if (sessionKey == sessionId && con != connectionId)
                {
                    Clients.Client(con).SendAsync("GetSend", code);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
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
        try
        {
            if (GlobalVariables.listKeys.Count > 0)
                GlobalVariables.listKeys.RemoveAll(x => x.Contains(Context.ConnectionId));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        await base.OnDisconnectedAsync(exception);
    }


    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"{Context.ConnectionId} - Connected");
        await base.OnConnectedAsync();
    }
}
