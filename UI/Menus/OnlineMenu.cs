using Chess.Core;
using Chess.Network;
using Chess.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Media.Protection.PlayReady;

namespace Chess.UI.Menus
{
    internal class OnlineMenu : Menu
    {
        public OnlineMenu() : base([])
        {
            MyButton join = new MyButton("Join", () =>
            {
                ClientConnector client = new ClientConnector();
                client.OnConnectionEstablished += (object sender, string extraInfo) =>
                {
                    System.Diagnostics.Debug.WriteLine($"Connection established: {extraInfo}");
                    StateMachine.StartGame(!bool.Parse(extraInfo), client);
                };
                client.Start();
                SelectList servers = new SelectList();
                AddToPanel(servers);
                Task.Run(() =>
                {
                    while (true)
                    {
                        var server = client.GetServers();
                        foreach (var s in server)
                        {
                            if (!servers.Items.Any(x => x == s.Item1)) servers.AddItem(s.Item1);
                        }
                        foreach (var s in servers.Items)
                        {
                            if (!server.Any(x => x.Item1 == s)) servers.RemoveItem(s);
                        }
                        Thread.Sleep(1000);
                    }
                });
                MyButton connect = new MyButton("Connect", () =>
                {
                    client.Connect(servers.SelectedValue);
                });
                AddToPanel(connect);

            });
            MyButton host = new MyButton("Host", () =>
            {
                ColorSelector colorSelector = new ColorSelector();
                AddToPanel(colorSelector);
                TextInput textInput = new TextInput();
                AddToPanel(textInput);
                MyButton hostButton = new MyButton("Start", () =>
                {
                    ServerConnector server = new ServerConnector();
                    server.OnConnectionEstablished += (object sender, string extraInfo) =>
                    {
                        System.Diagnostics.Debug.WriteLine("Connection established.");
                        StateMachine.StartGame(colorSelector.WhiteSelected, server);
                    };
                    server.Start(textInput.Value, colorSelector.WhiteSelected.ToString());
                });
                AddToPanel(hostButton);
            });
            AddToPanel(join);
            AddToPanel(host);
        }
    }
}
