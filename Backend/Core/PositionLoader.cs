using System.Text.Json;

namespace Backend.Core;

internal struct BoardSetup
{
	public List<PieceData> Pieces { get; set; }
}
internal struct PieceData
{
	public string Type { get; set; }
	public int[] Position { get; set; }
	public string Color { get; set; }
}

internal static class PositionLoader
{
	internal static BoardSetup LoadBoardSetup(string file)
	{
		string json = File.ReadAllText(file);
		return JsonSerializer.Deserialize<BoardSetup>(json);
	}
}
