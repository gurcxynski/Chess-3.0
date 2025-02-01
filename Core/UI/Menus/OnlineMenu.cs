using Chess.Core.Network;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Media.Protection.PlayReady;

namespace Chess.Core.UI.Menus
{
    internal class OnlineMenu : Menu
    {
        public OnlineMenu() : base([])
        {
            MyButton join = new MyButton("Join", () =>
            {
                ClientConnector client = new ClientConnector();
                client.OnConnectionEstablished += (object sender, EventArgs e) =>
                {
                    StateMachine.StartGame();
                };
                client.Start();
                SelectList servers = new SelectList();
                AddToPanel(servers);
                Task.Run(() =>
                {
                    while (true)
                    {
                        var server = client.GetServerNames();
                        foreach (var s in server)
                        {
                            if (!servers.Items.Contains(s)) servers.AddItem(s);
                        }
                        foreach (var s in servers.Items)
                        {
                            if (!server.Contains(s)) servers.RemoveItem(s);
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
                TextInput textInput = new TextInput();
                AddToPanel(textInput);
                MyButton hostButton = new MyButton("Start", () =>
                {
                    ServerConnector server = new ServerConnector(); 
                    server.OnConnectionEstablished += (object sender, EventArgs e) =>
                    {
                        StateMachine.StartGame();
                    };
                    server.Start();
                });
                AddToPanel(hostButton);
            });
            AddToPanel(join);
            AddToPanel(host);
        }
    }
}
