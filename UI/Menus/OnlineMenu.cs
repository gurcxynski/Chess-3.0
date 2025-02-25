using Chess.Core;
using Chess.Network;
using GeonBit.UI.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chess.UI.Menus
{
    internal class OnlineMenu : Menu
    {
        private readonly ClientConnector client;

        public OnlineMenu() : base("Online Game", [])
        {
            client = new ClientConnector();
            client.OnConnectionEstablished += (object sender, string extraInfo) =>
            {
                System.Diagnostics.Debug.WriteLine($"Connection established: {extraInfo}");
                GameCreator.GameData gameData = new(extraInfo);
                gameData.SelectedWhite = !gameData.SelectedWhite;
                StateMachine.StartGame(gameData, client);
            };
            client.Start();

            SelectList servers = new();
            Task.Run(() =>
            {
                while (true)
                {
                    var serverList = client.GetServers();
                    foreach (var s in serverList)
                    {
                        if (!servers.Items.Any(x => x == s.Item1)) servers.AddItem(s.Item1);
                    }
                    foreach (var s in servers.Items)
                    {
                        if (!serverList.Any(x => x.Item1 == s)) servers.RemoveItem(s);
                    }
                    Thread.Sleep(1000);
                }
            });

            MyButton connect = new("Join", () =>
            {
                client.Connect(servers.SelectedValue);
            });
            connect.Enabled = false;
            servers.OnValueChange = (entity) =>
            {
                connect.Enabled = true;
            };

            AddToPanel(servers);
            AddToPanel(connect); 
            AddToPanel(new MyButton("Host", () =>
            {
                client.Stop();
                StateMachine.ToMenu<HostMenu>();
            }));
            AddToPanel(new MyButton("Back", () =>
            {
                client.Stop();
                StateMachine.ToMenu<NewGameMenu>();
            }));
        }
    }
}
