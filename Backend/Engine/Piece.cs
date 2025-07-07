using Backend.Util;
using System.Numerics;

namespace Backend.Engine;

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
	internal Move? TryCreatingMove(int col, int row, Board board, bool verifyCheck = true, Type? promotionType = null) => TryCreatingMove(new (col, row), board, verifyCheck, promotionType);
	internal Move? TryCreatingMove(Vector2 target, Board board, bool verifyCheck = true, Type? promotionType = null)
	{
		if (target == Position) return null;
		if (IsCaptured) return null;
		if (!CheckBasicMovement(target - Position, board)) return null;
		if (!CanJumpOver && !MoveHelper.CheckPath(Position, target, board)) return null;
		if (!MoveHelper.CheckDestination(Position, target, board)) return null;
		Piece? capturedPiece = MoveHelper.IsEnPassant(Position, target, board) ? MoveHelper.GetPieceCapturedByEnPassant(target, board) : board.GetPieceAt(target);
		Move move = new(Position, target, this, capturedPiece, firstMove: !HasMoved, castles: MoveHelper.IsCastles(Position, target, board), promotion: MoveHelper.IsPromotion(this, target))
		{
			PromotionPieceType = promotionType
		};
		if (verifyCheck && MoveHelper.WillBeChecked(move, board)) return null;
		return move;
	}
	protected abstract bool CheckBasicMovement(Vector2 direction, Board board);
	public override string ToString()
	{
		return GetType().Name switch
		{
			"Pawn" => IsWhite ? "P" : "p",
			"Rook" => IsWhite ? "R" : "r",
			"Knight" => IsWhite ? "N" : "n",
			"Bishop" => IsWhite ? "B" : "b",
			"Queen" => IsWhite ? "Q" : "q",
			"King" => IsWhite ? "K" : "k",
			_ => string.Empty,
		};
	}
	public string UnicodeIcon()
	{
		return GetType().Name switch
		{
			"Pawn" => IsWhite ? "\u2659" : "\u265F",
			"Rook" => IsWhite ? "\u2656" : "\u265C",
			"Knight" => IsWhite ? "\u2658" : "\u265E",
			"Bishop" => IsWhite ? "\u2657" : "\u265D",
			"Queen" => IsWhite ? "\u2655" : "\u265B",
			"King" => IsWhite ? "\u2654" : "\u265A",
			_ => string.Empty,
		};
	}
}