using GeonBit.UI.Entities;

namespace Chess.UI.Menus
{
    internal class GameCreator : Entity
    {
        internal struct GameData
        {
            internal bool SelectedWhite;
            internal int Minutes;
            internal int Seconds;
            public override readonly string ToString() => $"{Minutes}:{Seconds} {(SelectedWhite ? "White" : "Black")}";
            internal GameData(int minutes, int seconds, bool selectedWhite)
            {
                Minutes = minutes;
                Seconds = seconds;
                SelectedWhite = selectedWhite;
            }
            internal GameData(string data)
            {
                string[] split = data.Split(' ');
                Minutes = int.Parse(split[0].Split(':')[0]);
                Seconds = int.Parse(split[0].Split(':')[1]);
                SelectedWhite = split[1] == "White";
            }
        }
        readonly ColorSelector selector = new();
        internal GameData Data => new(0, 0, selector.SelectedWhite);
        internal GameCreator()
        {
            Anchor = Anchor.AutoCenter;
            Size = new(0, 0.4f);
            AddChild(selector);
            //AddChild(new TextInput());
            //AddChild(new TextInput());
        }
    }
}
