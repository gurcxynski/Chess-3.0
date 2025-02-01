using System;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Chess.Core.Network;

abstract public class NetworkConnector : IMoveReceiver
{
    protected const string prefix = "ServerData";
    protected const char separator = ';';
    protected const int UDP_DESTINATION_PORT = 12000;

    private bool disposed = false;

    public event EventHandler<string> OnMoveDataReceived;
    public event EventHandler OnConnectionEstablished;
    public event EventHandler OnMessageSent;

    protected CancellationTokenSource cancellationTokenSource = new();

    protected TcpClient tcpClient;
    protected TcpListener tcpListener;

    protected NetworkStream networkStream;
    public bool Connected => tcpClient?.Connected ?? false;

    abstract public void Start();


    public async void Send(byte[] data)
    {
        if (networkStream == null || !tcpClient.Connected)
        {
            System.Diagnostics.Debug.WriteLine("TCP client is not connected.");
            return;
        }

        try
        {
            await networkStream.WriteAsync(data);
            OnMessageSent?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to send data: {ex.Message}");
        }
    }

    public async void ListenIncoming()
    {
        if (networkStream == null || !tcpClient.Connected)
        {
            System.Diagnostics.Debug.WriteLine("TCP client is not connected.");
            return;
        }

        byte[] buffer = new byte[1024];
        try
        {
            int bytesRead = await networkStream.ReadAsync(buffer);
            if (bytesRead > 0)
            {
                var moveData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                OnMoveDataReceived?.Invoke(this, moveData);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to receive data: {ex.Message}");
        }
    }

    public void Stop()
    {
        tcpListener?.Stop();
        tcpClient?.Close();
        cancellationTokenSource.Cancel();
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected void Dispose(bool disposing)
    {
        if (disposed) return;
        if (disposing)
        {
            cancellationTokenSource?.Dispose();
            tcpClient?.Dispose();
            networkStream?.Dispose();
            tcpListener?.Dispose();
        }
        disposed = true;
    }
    protected void RaiseConnectionEstablished() => OnConnectionEstablished?.Invoke(this, EventArgs.Empty); 
    protected static IPAddress GetLocalIPAddress()
    {
        foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (networkInterface.OperationalStatus == OperationalStatus.Up)
            {
                var properties = networkInterface.GetIPProperties();
                foreach (var address in properties.UnicastAddresses)
                {
                    if (address.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return address.Address;
                    }
                }
            }
        }
        throw new InvalidOperationException("No network adapters with an IPv4 address in the system!");
    }
}