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
	internal void MoveTo(Vector2 position)
	{
		Position = position;
	}
	internal bool TryCreatingMove(out Move move, int col, int row, Board board, bool verifyCheck = true, Type? promotionType = null, bool setMoveFlags = true) => TryCreatingMove(out move, new (col, row), board, verifyCheck, promotionType, setMoveFlags);
	internal bool TryCreatingMove(out Move move, Vector2 target, Board board, bool verifyCheck = true, Type? promotionType = null, bool setMoveFlags = true)
	{
		move = Move.Empty;
		if (target == Position) return false;
		if (IsCaptured) return false;
		if (!CheckBasicMovement(target - Position, board)) return false;
		if (!CanJumpOver && !MoveHelper.CheckPath(Position, target, board)) return false;
		if (!MoveHelper.CheckDestination(Position, target, board)) return false;
		Piece? capturedPiece = MoveHelper.IsEnPassant(Position, target, board) ? MoveHelper.GetPieceCapturedByEnPassant(target, board) : board.GetPieceAt(target);
		var (check, mate) = MoveHelper.IsCheckOrMate(Position, target, board);
		move = new(Position, target, this,
			capturedPiece: capturedPiece,
			firstMove: !HasMoved,
			castles: MoveHelper.IsCastles(Position, target, board),
			promotion: MoveHelper.IsPromotion(this, target),
			promotiontype: promotionType,
			isCheck: setMoveFlags && check,
			isMate: setMoveFlags && mate);
		if (verifyCheck && MoveHelper.WillBeChecked(move, board)) return false;
		return true;
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