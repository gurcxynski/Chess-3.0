using Microsoft.Xna.Framework;

namespace Chess.Core.Util;
internal static class PositionConverter
{
    internal static Vector2 ToGrid(Vector2 vec)
    {
        var rel = vec - ChessGame.Instance.GetActualDestRect().Location.ToVector2() - ChessGame.Instance.Padding;
        rel /= ChessGame.Instance.BoardSize / 8;
        rel.Floor();
        rel = new Vector2(rel.X, 7 - rel.Y);
        return rel;
    }

    internal static Vector2 ToOffset(Vector2 square)
    {
        square = new Vector2(square.X, 7 - square.Y);
        return ChessGame.Instance.BoardSize / 8 * square;
    }
}