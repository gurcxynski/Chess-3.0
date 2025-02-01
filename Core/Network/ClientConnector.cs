using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chess.Core.Network;

public class ClientConnector : NetworkConnector
{
    readonly Dictionary<string, IPEndPoint> serverInfos = [];

    public IEnumerable<string> GetServerNames() => serverInfos.Keys;

    public override void Start()
    {
        UdpClient udpClient = new(new IPEndPoint(IPAddress.Any, UDP_DESTINATION_PORT));
        Task.Run(() => ListenAsync(udpClient, cancellationTokenSource.Token));
    }

    private async Task ListenAsync(UdpClient udpClient, CancellationToken cancellationToken)
    {
        try
        {
            while (true)
            {
                var result = await udpClient.ReceiveAsync(cancellationToken);
                var serverInfo = Encoding.UTF8.GetString(result.Buffer).Split(separator);
                if (serverInfo.Length == 4 && serverInfo[0] == prefix && IPAddress.TryParse(serverInfo[2], out var serverIPAddress) && int.TryParse(serverInfo[3], out var serverPort))
                {
                    serverInfos.Add(serverInfo[1], new IPEndPoint(serverIPAddress, serverPort));
                }
            }
        }
        catch (OperationCanceledException) { }
        finally
        {
            udpClient.Close();
        }
    }

    public async void Connect(string serverName)
    {
        if (!serverInfos.TryGetValue(serverName, out var selected))
        {
            System.Diagnostics.Debug.WriteLine("Server not found.");
            return;
        }
        Debug.WriteLine($"Connecting to the server: {selected}");
        tcpClient = new TcpClient();
        try
        {
            await tcpClient.ConnectAsync(selected);
            networkStream = tcpClient.GetStream();
            RaiseConnectionEstablished();
        }
        catch (SocketException ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to connect to the server: {ex.Message}");
        }
    }
}
