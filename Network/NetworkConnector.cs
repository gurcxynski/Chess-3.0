using Chess.Core;
using Chess.Engine;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Chess.Network;

abstract public class NetworkConnector : IMoveReceiver
{
    protected const string prefix = "ServerData";
    protected const char separator = ';';
    protected const int UDP_DESTINATION_PORT = 12000;

    private bool disposed = false;

    public event EventHandler<(byte[], int)> OnMoveDataReceived;
    public event EventHandler<string> OnConnectionEstablished;
    public event EventHandler OnMessageSent;

    protected CancellationTokenSource cancellationTokenSource = new();

    protected TcpClient tcpClient;
    protected TcpListener tcpListener;

    protected NetworkStream networkStream;
    public bool Connected => tcpClient?.Connected ?? false;

    abstract public void Start();

    public async void Send(string data)
    {
        if (networkStream == null || !tcpClient.Connected)
        {
            System.Diagnostics.Debug.WriteLine("TCP client is not connected.");
            return;
        }

        try
        {
            await networkStream.WriteAsync(Encoding.UTF8.GetBytes(data));
            OnMessageSent?.Invoke(this, EventArgs.Empty);
            System.Diagnostics.Debug.WriteLine($"Data sent: {data}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to send data: {ex.Message}");
        }
    }

    public async void Listen()
    {
        if (!Connected)
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
                var moveData = (buffer, bytesRead);
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
        Dispose();
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
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
            tcpClient?.Dispose();
            networkStream?.Dispose();
            tcpListener?.Dispose();
        }
        disposed = true;
    }
    protected void RaiseConnectionEstablished(string extraInfo) => OnConnectionEstablished?.Invoke(this, extraInfo);
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

    public void ProcessMove(Move move)
    {
        Send(move.ToString());
        Listen();
    }
}