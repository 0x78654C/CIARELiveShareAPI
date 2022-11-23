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
    public void GetSendCode(string sessionId, string code, string position)
    {
        try
        {
            var connectionId = Context.ConnectionId;
            var listKeys = GlobalVariables.listKeys;
            var countSessionIds = listKeys.Where(x => x.Contains(sessionId)).Count();
            var countId = listKeys.Where(x => x.Contains(connectionId)).Count();
            var patern = $"{sessionId}|{connectionId}";
            SendHostData(sessionId, code, connectionId);
            if (countSessionIds < 2 && countId < 1)
                listKeys.Add(patern);
            foreach (var sessionCon in listKeys)
            {
                string sessionKey = sessionCon.Split('|')[0];
                string con = sessionCon.Split('|')[1];
                if (sessionKey == sessionId && con != connectionId)
                    Clients.Client(con).SendAsync("GetSend", code, position, connectionId);
            }
        }
        catch { }
    }

    /// <summary>
    /// Store code from live share host on firt connection
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
        }
        else
        {
            foreach (var sId in hostData)
            {
                if (sId.Key == sessionId)
                {
                    Clients.Client(connectionId).SendAsync("GetSend", sId.Value, "0|0", connectionId);
                    hostData.Remove(sId.Key);
                }
            }
        }
    }

    /// <summary>
    /// Remove data from dictionary on client disconnect.
    /// </summary>
    /// <param name="connectionID"></param>
    private void RemoveHostData(string connectionID)
    {
        string sId = string.Empty;
        var listKeys = GlobalVariables.listKeys;
        foreach (var key in listKeys)
        {
            if (key.Contains(connectionID))
                sId = key;
        }
        sId = sId.Split('|')[0];
        foreach (var s in GlobalVariables.hostData)
        {
            if (s.Key.Contains(sId))
            {
                GlobalVariables.hostData.Remove(s.Key);
            }
        }
    }

    /// <summary>
    /// Remove patern from list on disconnect and display disconnected client in console.
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        try
        {
            var connectionId = Context.ConnectionId;
            RemoveHostData(connectionId);
            if (GlobalVariables.listKeys.Count > 0)
                GlobalVariables.listKeys.RemoveAll(x => x.Contains(connectionId));

        }
        catch { }

        await base.OnDisconnectedAsync(exception);
    }
}
