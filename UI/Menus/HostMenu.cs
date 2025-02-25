using Chess.Core;
using Chess.Network;
using GeonBit.UI.Entities;

namespace Chess.UI.Menus
{
    internal class HostMenu : Menu
    {
        public HostMenu() : base("Host Game", [])
        {
            GameCreator creator = new();
            AddToPanel(creator);
            TextInput textInput = new();
            AddToPanel(textInput);
            ServerConnector server = new();
            MyButton hostButton = new("Start", () =>
            {
                server.OnConnectionEstablished += (object sender, string extraInfo) =>
                {
                    System.Diagnostics.Debug.WriteLine("Connection established.");
                    StateMachine.StartGame(creator.Data, server);
                };
                server.Start(textInput.Value, creator.Data.ToString());
            });
            AddToPanel(hostButton);
            AddToPanel(new MyButton("Back", () =>
            {
                server.Stop();
                StateMachine.ToMenu<OnlineMenu>();
            }));
        }
    }
}