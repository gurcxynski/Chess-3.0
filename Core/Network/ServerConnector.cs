using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chess.Core.Network;

public class ServerConnector : NetworkConnector
{
    string serverName;
    string extraInfo;
    const int broadcastInterval = 1500;
    public override void Start()
    {
        tcpListener = new TcpListener(new IPEndPoint(GetLocalIPAddress(), 0));
        tcpListener.Start();
        UdpClient udpClient = new()
        {
            EnableBroadcast = true
        };

        Task.Run(ListenForConnectionAsync);
        Task.Run(() => BroadcastServerInfoAsync(udpClient, cancellationTokenSource.Token));
    }
    public void Start(string name, string extra)
    {
        serverName = name;
        extraInfo = extra;
        Start();
    }

    private async Task BroadcastServerInfoAsync(UdpClient udpClient, CancellationToken cancellationToken)
    {
        try
        {
            IPEndPoint endPoint = tcpListener.LocalEndpoint as IPEndPoint;
            string serverInfo = string.Join(separator, prefix, serverName, endPoint.Address, endPoint.Port, extraInfo);
            IPEndPoint destination = new(IPAddress.Broadcast, UDP_DESTINATION_PORT);
            Debug.WriteLine($"Broadcasting server info: {serverInfo}");
            while (true)
            {
                await udpClient.SendAsync(Encoding.UTF8.GetBytes(serverInfo), serverInfo.Length, destination);
                await Task.Delay(broadcastInterval, cancellationToken);
            }
        }
        catch (OperationCanceledException) { }
        finally
        {
            udpClient.Close();
        }
    }
    private async Task ListenForConnectionAsync()
    {
        try
        {
            Debug.WriteLine($"Listening for incoming connections at: {tcpListener.LocalEndpoint}");
            while (true)
            {
                var client = await tcpListener.AcceptTcpClientAsync();
                if (client != null)
                {
                    tcpClient = client;
                    networkStream = tcpClient.GetStream();
                    RaiseConnectionEstablished(extraInfo);
                    cancellationTokenSource.Cancel();
                    break;
                }
            }
        }
        catch (SocketException ex)
        {
            Debug.WriteLine($"SocketException: {ex.Message}");
        }
    }
}