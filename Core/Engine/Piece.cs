using Chess.Core.Util;
using Microsoft.Xna.Framework;

namespace Chess.Core.Engine;
internal abstract class Piece(Vector2 position, bool isWhite = true)
{
    internal Vector2 Position { get; private set; } = position;
    internal bool IsWhite { get; } = isWhite;
    internal bool HasMoved { get; set; } = false;
    internal bool IsCaptured { get; set; } = false;
    protected virtual bool CanJumpOver { get; } = false;
    internal void Move(Vector2 pos)
    {
        Position = pos;
        HasMoved = true;
    }
    internal Move TryCreatingMove(Vector2 target, Board board, bool verifyCheck = true)
    {
        if (IsCaptured) return null;
        if (!CheckBasicMovement(target - Position, board)) return null;
        if (!CanJumpOver && !MoveHelper.CheckPath(Position, target, board)) return null;
        if (!MoveHelper.CheckDestination(Position, target, board)) return null;
        Move move = new(Position, target, this, board.GetPieceAt(target), firstMove: !HasMoved);
        if (verifyCheck && MoveHelper.WillBeChecked(move, board)) return null;
        return move;
    }
    protected abstract bool CheckBasicMovement(Vector2 direction, Board board);
}
