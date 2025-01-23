using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;

namespace Chess.Core.Util;
internal static class PositionConverter
{
    internal static Vector2 ToGrid(Vector2 vec)
    {
        var rel = vec - ChessGame.Instance.GetActualDestRect().Location.ToVector2() - ChessGame.Instance.Padding;
        rel /= (ChessGame.Instance.Size - (2 * ChessGame.Instance.Padding)) / 8;
        rel.Floor();
        return rel;
    }

    internal static Vector2 ToOffset(Vector2 vec)
    {
        return vec * ((ChessGame.Instance.Size - (2 * ChessGame.Instance.Padding)) / 8);
    }
}