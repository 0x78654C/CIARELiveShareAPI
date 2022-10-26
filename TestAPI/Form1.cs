using Microsoft.AspNetCore.SignalR.Client;

namespace TestAPI;
public partial class Form1 : Form
{

    /*
        Test CIARE API form.
     */

    HubConnection hubConnection;

    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        // Create connection and events.
        ApiConnection(receiveTxt, liveUrlTxt.Text);
    }

    /// <summary>
    /// Conection builder and event checker.
    /// </summary>
    /// <param name="receiveTxt"></param>
    /// <param name="url"></param>
    private void ApiConnection(TextBox receiveTxt, string url)
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl(url)
            .WithAutomaticReconnect()
            .Build();

        hubConnection.Reconnecting += (sender) =>
        {
            SetText(receiveTxt, "Attempt to reconnect..");
            return Task.CompletedTask;
        };


        hubConnection.Reconnected += (sender) =>
        {
            SetText(receiveTxt, "Reconnected...");
            return Task.CompletedTask;
        };

        hubConnection.Closed += (sender) =>
        {
            var message = "Connection Closed;";
            MethodInvoker setText = new MethodInvoker(() =>
            {
                receiveTxt.Text += $"{message}\n";
                liveCodeTxt.Enabled = false;
                connectBtn.Enabled = true;
            });
            receiveTxt.BeginInvoke(setText);
            return Task.CompletedTask;
        };
    }

    /// <summary>
    /// Open connection with the API.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void connectBtn_Click(object sender, EventArgs e)
    {
        hubConnection.On<string>("GetSend", (code) =>
        {
            if (code != liveCodeTxt.Text)
                SetTextLive(liveCodeTxt, $"{code}\n");
        });

        try
        {
            await hubConnection.StartAsync();
            MethodInvoker setText = new MethodInvoker(() =>
            {
                receiveTxt.Text += $"Connection Started!\n";
                connectBtn.Enabled = false;
                liveCodeTxt.Enabled = true;
            });
            receiveTxt.BeginInvoke(setText);
        }
        catch (Exception ex)
        {
            SetText(receiveTxt, $"Error: {ex.ToString()}");
        }
    }

    /// <summary>
    /// Set text on separated thread.
    /// </summary>
    /// <param name="textBox"></param>
    /// <param name="text"></param>
    private void SetText(TextBox textBox, string text)
    {
        MethodInvoker setText = new MethodInvoker(() =>
        {
            textBox.Text = text;
        });
        textBox.BeginInvoke(setText);
    }

    /// <summary>
    /// Set Live text in realtime.
    /// </summary>
    /// <param name="textBox"></param>
    /// <param name="text"></param>
    private void SetTextLive(TextBox textBox, string text)
    {
        MethodInvoker setText = new MethodInvoker(() =>
        {
            textBox.ScrollToCaret();
            textBox.Text = text;
            textBox.ScrollToCaret();
        });
        textBox.BeginInvoke(setText);
    }

    /// <summary>
    /// Update textbox with data in real time.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void liveCodeTxt_TextChanged(object sender, EventArgs e)
    {
        Task.Run(() => SendReceiveData());
    }


    /// <summary>
    /// Send/Receive data event starter.
    /// </summary>
    private async void SendReceiveData()
    {
        try
        {
            await hubConnection.InvokeAsync("GetSendCode", liveCodeTxt.Text);
        }
        catch (Exception ex)
        {
            SetText(receiveTxt, $"Error: {ex.ToString()}");
        }
    }
}
