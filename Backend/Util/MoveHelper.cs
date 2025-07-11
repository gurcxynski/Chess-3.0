using Backend.Engine;
using Backend.Engine.Pieces;
using System.Numerics;

namespace Backend.Util;

internal static class MoveHelper
{
	internal static Move? TryCreatingMove(Board board, string move)
	{
		var start = new Vector2(move[0] - 'a', move[1] - '1');
		var end = new Vector2(move[2] - 'a', move[3] - '1');
		Type? promotion = null;
		if (move.Length == 5)
		{
			var typeName = $"Chess.Backend.Core.Engine.Pieces.{(move[4] == 'q' ? "Queen" : move[4] == 'r' ? "Rook" : move[4] == 'b' ? "Bishop" : "Knight")}";
			promotion = Type.GetType(typeName, throwOnError: true);
		}
		var piece = board.GetPieceAt(start);
		var moveObj = piece?.TryCreatingMove(end, board, promotionType: promotion);
		return moveObj;
	}
	internal static bool CheckPath(Vector2 start, Vector2 end, Board board)
	{
		if (end == start) return false;
		Vector2 direction = end - start;
		Vector2 step = new(Math.Sign(direction.X), Math.Sign(direction.Y));
		for (var current = start + step; current != end; current += step)
		{
			if (board.GetPieceAt(current) is not null) return false;
		}
		return true;
	}

	internal static bool CheckDestination(Vector2 start, Vector2 dest, Board board)
	{
		return !(board.GetPieceAt(dest) is not null && board.GetPieceAt(dest)!.IsWhite == board.GetPieceAt(start)!.IsWhite);
	}

	internal static bool IsAttackedBy(Vector2 pos, Board board, bool white, bool verifyCheck = true)
	{
		var piecesCopy = new List<Piece>(board.Pieces);
		foreach (var piece in piecesCopy)
		{
			if (piece.IsWhite != white) continue;
			if (piece.TryCreatingMove(pos, board, verifyCheck) is not null) return true;
		}
		return false;
	}
	internal static bool WillBeChecked(Move move, Board board)
	{
		board.ExecuteMove(move, false);
		bool ret = IsAttackedBy(board.GetKing(!board.WhiteToMove).Position, board, board.WhiteToMove, false);
		board.UndoMove();
		return ret;
	}
	internal static bool IsEnPassant(Vector2 origin, Vector2 target, Board board)
	{
		if (!((target - origin).Y == 1 && Math.Abs((target - origin).X) == 1)) return false;
		Move? lastMove = board.LastMove;
		return lastMove is not null && lastMove.MovedPiece is Pawn && lastMove.IsFirstMoveOfPiece &&
			lastMove.End.X == target.X && lastMove.End.Y == origin.Y;
	}
	internal static Piece? GetPieceCapturedByEnPassant(Vector2 target, Board board)
	{
		if (board.WhiteToMove) return board.GetPieceAt(target - Vector2.UnitY);
		return board.GetPieceAt(target + Vector2.UnitY);
	}

	internal static bool CalculateDraw(Board board)
	{
		if (!board.IsMate && board.ValidMoves.Count == 0) return true;
		if (board.LastFifty.Count > 50 && board.LastFifty.All(move => !move.IsCapture || move.MovePieceType == typeof(Pawn))) return true;
		return false;
	}

	internal static string ToFieldString(Vector2 vector)
	{
		return $"{(char)('a' + (int)vector.X)}{(int)vector.Y + 1}";
	}

	internal static bool IsCastles(Vector2 position, Vector2 target, Board board)
	{
		return board.GetPieceAt(position) is King && Math.Abs(position.X - target.X) == 2;
	}
	internal static string TypeToString<T>(bool white) where T : Piece
	{
		return (white ? "w" : "b") + typeof(T).Name.Split('.')[^1];
	}

	internal static bool IsPromotion(Piece piece, Vector2 target)
	{
		if (piece is not Pawn) return false;
		return piece.IsWhite && target.Y == 7 || !piece.IsWhite && target.Y == 0;
	}
}