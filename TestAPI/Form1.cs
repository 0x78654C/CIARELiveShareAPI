using Microsoft.AspNetCore.SignalR.Client;
using TestAPI.Utils;

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
    private string? _sessionId = "";
    private string? _encryptionPassword;
    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        // Create connection and events.
        ApiConnection(receiveTxt, liveUrlTxt.Text);
        SetText(sessionIdTxt, KeyGenerator.GeneratePassword(16, false, false, false, true));
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
                receiveTxt.Text = $"{message}\n";
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
        if (string.IsNullOrEmpty(passwordTxt.Text))
        {
            MessageBox.Show("No password provided!");
            return;
        }

        if (string.IsNullOrEmpty(remoteIdTxt.Text))
        {
            MessageBox.Show("You must enter the remote session id to connect!");
            return;
        }

        hubConnection.On<string>("GetSend", (code) =>
        {
            SetLiveCode(_encryptionPassword, code, liveCodeTxt, _writeing);
        });

        try
        {
            await hubConnection.StartAsync();
            MethodInvoker setText = new MethodInvoker(() =>
            {
                receiveTxt.Text = $"Connection Started!\n";
                connectBtn.Enabled = false;
                startShareBtn.Enabled = false;
                liveCodeTxt.Enabled = true;
            });
            receiveTxt.BeginInvoke(setText);
            _sessionId = remoteIdTxt.Text;
            _encryptionPassword = passwordTxt.Text;
            await hubConnection.InvokeAsync("GetSendCode", _sessionId, string.Empty);
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
        textBox.Text = text;
        textBox.ScrollToCaret();
    }

    /// <summary>
    /// Update textbox with data in real time.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void liveCodeTxt_TextChanged(object sender, EventArgs e)
    {
        Task.Delay(10);
        _writeing = false;
        if (_connected)
            SendData(_encryptionPassword, _sessionId, liveCodeTxt.Text);
    }


    /// <summary>
    /// Send/Receive data event starter.
    /// </summary>
    private async void SendData(string password, string sessionId, string codeEditor)
    {
        try
        {

            string encrypted = AESEncryption.Encrypt(codeEditor, password);
            await hubConnection.InvokeAsync("GetSendCode", sessionId, encrypted);
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
        //SetLiveCode(_encryptionPassword, _codeReceive, liveCodeTxt, _writeing);
    }

    /// <summary>
    /// Sets the code 
    /// </summary>
    /// <param name="password"></param>
    /// <param name="codeReceived"></param>
    /// <param name="codeEditor"></param>
    /// <param name="writer"></param>
    private void SetLiveCode(string password, string codeReceived, TextBox codeEditor, bool writer)
    {
        try
        {
            if (writer)
            {
                string decrypt = AESEncryption.Decrypt(codeReceived, password);
                SetTextLive(codeEditor, decrypt);
            }
        }
        catch
        {
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
        Task.Run(() => CloseConnection());
    }

    /// <summary>
    /// Close API connection and stop timers.
    /// </summary>
    private async void CloseConnection()
    {
        receiveTxt.Clear();
        if (hubConnection != null)
            await hubConnection.StopAsync();

        // Stop live code share timer.
        updateLiveCode.Stop();
        updateLiveCode.Enabled = false;
        _connected = false;
        editorWrite.Stop();
        editorWrite.Enabled = false;
        startShareBtn.Enabled = true;
    }

    private async void startShareBtn_Click(object sender, EventArgs e)
    {
        if (_connected)
        {
            startShareBtn.Text = "Start Share";
            receiveTxt.Text = $"Live share Stoped!\n";
            connectBtn.Enabled = true;
            button1.Enabled = true;
            if (hubConnection != null)
                await hubConnection.StopAsync();

            // Stop live code share timer.
            updateLiveCode.Stop();
            updateLiveCode.Enabled = false;

            editorWrite.Stop();
            editorWrite.Enabled = false;
        }
        else
        {
            if (string.IsNullOrEmpty(myPasswordTxt.Text))
            {
                MessageBox.Show("No password provided!");
                return;
            }

            hubConnection.On<string>("GetSend", (code) =>
            {
                _writeing = true;
                SetLiveCode(_encryptionPassword, code, liveCodeTxt, _writeing);
            });

            try
            {
                await hubConnection.StartAsync();
                MethodInvoker setText = new MethodInvoker(() =>
                {
                    receiveTxt.Text = $"Live share started!\n";
                    connectBtn.Enabled = false;
                });

                _encryptionPassword = myPasswordTxt.Text;
                _sessionId = sessionIdTxt.Text;
                receiveTxt.BeginInvoke(setText);
                await hubConnection.InvokeAsync("GetSendCode", sessionIdTxt.Text, string.Empty);
                _connected = true;
                updateLiveCode.Enabled = true;
                updateLiveCode.Start();
                editorWrite.Enabled = true;
                startShareBtn.Text = "Stop Share";
                editorWrite.Start();
                button1.Enabled = false;
            }
            catch (Exception ex)
            {
                SetText(receiveTxt, $"Error: {ex.ToString()}");
            }
        }
    }
}
