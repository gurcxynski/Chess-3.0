using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using static Chess.Core.PositionLoader;

namespace Chess.Core;
internal class BoardSetup
{
    public List<PieceData> Pieces { get; set; }
}

internal class PositionLoader
{
    internal class PieceData
    {
        public string Type { get; set; }
        public int[] Position { get; set; }
        public string Color { get; set; }
    }

    internal static BoardSetup LoadBoardSetup(string file)
    {
        string json = File.ReadAllText(file);
        return JsonSerializer.Deserialize<BoardSetup>(json);
    }
}
