using Microsoft.AspNetCore.SignalR.Client;

namespace TestAPI;
public partial class Form1 : Form
{

    /*
        Test CIARE API form.
     */

    HubConnection hubConnection;
    private string _codeReceive;
    private bool _connected = false;
    private bool _writeing = false;

    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        // Create connection and events.
        ApiConnection(receiveTxt, liveUrlTxt.Text);
        SetText(liveKeyTxt, Utils.KeyGenerator.GeneratePassword(10));
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
            updateLiveCode.Stop();
            updateLiveCode.Enabled = false;
            editorWrite.Stop();
            editorWrite.Enabled = false;
            _connected = false;
            return Task.CompletedTask;
        };


        hubConnection.Reconnected += (sender) =>
        {
            SetText(receiveTxt, "Reconnected...");
            _connected = true;
            updateLiveCode.Enabled = true;
            updateLiveCode.Start();
            editorWrite.Enabled = true;
            editorWrite.Start();
            return Task.CompletedTask;
        };

        hubConnection.Closed += (sender) =>
        {
            var message = "Connection Closed;";
            MethodInvoker setText = new MethodInvoker(() =>
            {
                receiveTxt.Text += $"{message}\n";
                connectBtn.Enabled = true;
            });
            receiveTxt.BeginInvoke(setText);
            _connected = false;
            updateLiveCode.Stop();
            updateLiveCode.Enabled = false;
            editorWrite.Stop();
            editorWrite.Enabled = false;
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
        if (string.IsNullOrEmpty(userNameTxt.Text))
        {
            MessageBox.Show("No live share key provided!");
            return;
        }

        hubConnection.On<string>("GetSend", (code) =>
        {
            _codeReceive = code;
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
            await hubConnection.InvokeAsync("GetSendCode", userNameTxt.Text, string.Empty);
            _connected = true;
            updateLiveCode.Enabled = true;
            updateLiveCode.Start();
            editorWrite.Enabled = true;
            editorWrite.Start();
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
            textBox.Text = text + "\n";
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
        _writeing = false;
        if (_connected)
            Task.Run(() => SendReceiveData());
    }


    /// <summary>
    /// Send/Receive data event starter.
    /// </summary>
    private async void SendReceiveData()
    {
        try
        {
            SetText(receiveTxt, hubConnection.ConnectionId);
            await hubConnection.InvokeAsync("GetSendCode", userNameTxt.Text, liveCodeTxt.Text);
        }
        catch (Exception ex)
        {
            SetText(receiveTxt, $"Error: {ex.ToString()}");
        }
    }

    /// <summary>
    /// Close API connection button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void button1_Click(object sender, EventArgs e)
    {
        CloseConnection();
    }

    /// <summary>
    /// Update texteditor from live share on real time.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void updateLiveCode_Tick(object sender, EventArgs e)
    {
        try
        {
            if (liveCodeTxt.Text != _codeReceive &&
                _codeReceive.Length > 0 && _writeing)
            {
                SetTextLive(liveCodeTxt, _codeReceive);
            }
        }
        catch { 
        // Skip error if is null.
        }
    }

    /// <summary>
    /// Set flag for update editor if user does not write in it.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void editorWrite_Tick(object sender, EventArgs e)
    {
        _writeing = true;
    }


    /// <summary>
    /// Close API connection on form close event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Form1_FormClosed(object sender, FormClosedEventArgs e)
    {
        CloseConnection();
    }

    /// <summary>
    /// Close API connection and stop timers.
    /// </summary>
    private void CloseConnection()
    {
        receiveTxt.Clear();
        if (hubConnection != null)
            hubConnection.StopAsync();

        // Stop live code share timer.
        updateLiveCode.Stop();
        updateLiveCode.Enabled = false;

        editorWrite.Stop();
        editorWrite.Enabled = false;
    }
}
