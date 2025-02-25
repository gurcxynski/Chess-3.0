using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chess.Network;

public class ServerConnector : NetworkConnector
{
    string serverName;
    string extraInfo;
    const int broadcastInterval = 1500;
    UdpClient udpClient;
    public override void Start()
    {
        tcpListener = new TcpListener(new IPEndPoint(GetLocalIPAddress(), 0));
        tcpListener.Start();
        udpClient = new()
        {
            EnableBroadcast = true
        };

        Task.Run(() => ListenForConnectionAsync(cancellationTokenSource.Token));
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
    private async Task ListenForConnectionAsync(CancellationToken cancellationToken)
    {
        try
        {
            Debug.WriteLine($"Listening for incoming connections at: {tcpListener.LocalEndpoint}");
            while (true)
            {
                var client = await tcpListener.AcceptTcpClientAsync(cancellationToken);
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
        catch (OperationCanceledException) { }
    }
}