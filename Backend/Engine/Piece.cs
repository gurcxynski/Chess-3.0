using Chess.Util;
using System.Numerics;
using System;

namespace Chess.Engine;
public abstract class Piece(Vector2 position, bool isWhite = true)
{
    internal Vector2 Position { get; private set; } = position;
    internal bool IsWhite { get; } = isWhite;
    internal bool HasMoved { get; set; } = false;
    internal bool IsCaptured { get; set; } = false;
    protected virtual bool CanJumpOver { get; } = false;
    internal void Move(Vector2 position)
    {
        Position = position;
    }
    internal Move TryCreatingMove(Vector2 target, Board board, bool verifyCheck = true, Type promotionType = null)
    {
        if (target == Position) return null;
        if (IsCaptured) return null;
        if (!CheckBasicMovement(target - Position, board)) return null;
        if (!CanJumpOver && !MoveHelper.CheckPath(Position, target, board)) return null;
        if (!MoveHelper.CheckDestination(Position, target, board)) return null;
        Piece capturedPiece = MoveHelper.IsEnPassant(Position, target, board) ? MoveHelper.GetPieceCapturedByEnPassant(target, board) : board.GetPieceAt(target);
        Move move = new(Position, target, this, capturedPiece, firstMove: !HasMoved, castles: MoveHelper.IsCastles(Position, target, board), promotion: MoveHelper.IsPromotion(this, target))
        {
            PromotionPieceType = promotionType
        };
        if (verifyCheck && MoveHelper.WillBeChecked(move, board)) return null;
        return move;
    }
    protected abstract bool CheckBasicMovement(Vector2 direction, Board board);
    public override string ToString() => (IsWhite ? "w" : "b") + GetType().ToString().Split('.')[^1];
}