using Chess.Core;
using Microsoft.Xna.Framework;

namespace Chess.Util;
internal static class PositionConverter
{
    internal static bool UpsideDown => !ChessGame.Instance.IsWhitePlayer;
    internal static Vector2 ToGrid(Vector2 position)
    {
        var rel = position - ChessGame.Instance.GetActualDestRect().Location.ToVector2() - ChessGame.Instance.Padding;
        rel /= ChessGame.Instance.BoardSize / 8;
        rel.Floor();
        if (!UpsideDown) rel.Y = 7 - rel.Y;
        return rel;
    }

    internal static Vector2 ToOffset(Vector2 square)
    {
        if (!UpsideDown) square.Y = 7 - square.Y;
        return ChessGame.Instance.BoardSize / 8 * square;
    }
}