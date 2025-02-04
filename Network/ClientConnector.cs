using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chess.Network;

public class ClientConnector : NetworkConnector
{
    struct ServerDescription
    {
        public string Name;
        public IPEndPoint EndPoint;
        public string ExtraInfo;
    }
    readonly HashSet<ServerDescription> serverInfos = new();

    public IEnumerable<(string, string)> GetServers()
    {
        foreach (var server in serverInfos)
        {
            yield return (server.Name, server.ExtraInfo);
        };
    }
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
                if (serverInfo.Length == 5 && serverInfo[0] == prefix && IPAddress.TryParse(serverInfo[2], out var serverIPAddress) && int.TryParse(serverInfo[3], out var serverPort))
                {
                    var server = new ServerDescription
                    {
                        Name = serverInfo[1],
                        EndPoint = new IPEndPoint(serverIPAddress, serverPort),
                        ExtraInfo = serverInfo[4]
                    };
                    serverInfos.Add(server);
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
        var selected = serverInfos.First(s => s.Name == serverName);
        if (selected.Name is null)
        {
            Debug.WriteLine("Server not found.");
            return;
        }
        Debug.WriteLine($"Connecting to the server: {selected}");
        tcpClient = new TcpClient();
        try
        {
            await tcpClient.ConnectAsync(selected.EndPoint);
            networkStream = tcpClient.GetStream();
            RaiseConnectionEstablished(selected.ExtraInfo);
        }
        catch (SocketException ex)
        {
            Debug.WriteLine($"Failed to connect to the server: {ex.Message}");
        }
    }
}
