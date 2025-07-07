using Backend.Engine.Pieces;
using Backend.Util;
using System.Numerics;
using System.Text;
using Backend.Core;

namespace Backend.Engine;

public class Board
{
	public static string DefaultFEN => "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
	internal List<Piece> Pieces { get; private init; } = [];
	internal IEnumerable<Piece> WhitePieces => Pieces.Where(piece => piece.IsWhite);
	internal IEnumerable<Piece> BlackPieces => Pieces.Where(piece => !piece.IsWhite);
	internal IEnumerable<Piece> CapturedPieces => Pieces.Where(piece => piece.IsCaptured);
	internal IEnumerable<Piece> ActivePieces => Pieces.Where(piece => !piece.IsCaptured);
	internal List<Move> ValidMoves => GetValidMoves();
	public event EventHandler OnGameOver = delegate { };
	private readonly Stack<Move> moveStack = new();
	internal IEnumerable<Move> MoveHistory => moveStack.Reverse();
	internal Queue<Move> LastFifty { get; private init; } = new();
	internal bool WhiteToMove { get; private set; }
	internal int MoveCount => moveStack.Count;
	private King whiteKing;
	private King blackKing;
	internal Board(BoardSetup setup)
	{
		FillBoard(setup);
		WhiteToMove = true;
	}
	private void FillBoard(BoardSetup setup)
	{
		foreach (var pieceData in setup.Pieces)
		{
			var piece = PieceFactory.CreatePiece(pieceData);
			Pieces.Add(piece);

			if (piece is King king)
			{
				if (king.IsWhite)
					whiteKing = king;
				else
					blackKing = king;
			}
		}
	}

	internal Piece? GetPieceAt(Vector2 pos) => Pieces.Find(piece => piece.Position == pos && !piece.IsCaptured);
	internal Piece? GetPieceAt(int x, int y) => GetPieceAt(new Vector2(x, y));
	internal List<Piece> GetAll(Type type) => Pieces.Where(piece => piece.GetType() == type).ToList();
	internal Piece GetKing(bool isWhite) => isWhite ? whiteKing : blackKing;
	internal Move? LastMove => moveStack.Count > 0 ? moveStack.Peek() : null;

	internal void ExecuteMove(Move move, bool isReal = true)
	{
		if (move.IsCapture) move.CapturedPiece!.IsCaptured = true;
		move.MovedPiece.Move(move.End);
		if (isReal) move.MovedPiece.HasMoved = true;
		if (isReal && move.IsPromotion) PromotePawn(move.MovedPiece, move.PromotionPieceType!);
		WhiteToMove = !WhiteToMove;
		moveStack.Push(move);
		LastFifty.Enqueue(move);
		if (LastFifty.Count > 50) LastFifty.Dequeue();
		if (isReal && IsMate) OnGameOver.Invoke(this, EventArgs.Empty);
		if (!move.IsCastles) return;
		if (move.End.X == 2)
		{
			Piece rook = GetPieceAt(new(0, move.Start.Y))!;
			rook.Move(new(3, move.Start.Y));
		}
		else if (move.End.X == 6)
		{
			Piece rook = GetPieceAt(new(7, move.Start.Y))!;
			rook.Move(new(5, move.Start.Y));
		}
	}

	internal void UndoMove()
	{
		if (MoveCount == 0) return;
		Move move = moveStack.Pop();
		move.MovedPiece.Move(move.Start);
		WhiteToMove = !WhiteToMove;
		if (move.IsCapture) move.CapturedPiece!.IsCaptured = false;
		if (move.IsFirstMoveOfPiece) move.MovedPiece.HasMoved = false;
		if (!move.IsCastles) return;
		if (move.End.X == 2)
		{
			Piece rook = GetPieceAt(new(3, move.Start.Y))!;
			rook.Move(new(0, move.Start.Y));
		}
		if (move.End.X == 6)
		{
			Piece rook = GetPieceAt(new(5, move.Start.Y))!;
			rook.Move(new(7, move.Start.Y));
		}
	}
	private List<Move> GetValidMoves()
	{
		List<Move> validMoves = [];
		var piecesCopy = new List<Piece>(Pieces);
		foreach (Piece piece in piecesCopy.Where(piece => piece.IsWhite == WhiteToMove))
		{
			for (int x = 0; x < 8; x++)
			{
				for (int y = 0; y < 8; y++)
				{
					Vector2 pos = new(x, y);
					if (pos == piece.Position) continue;
					Move? move = piece.TryCreatingMove(pos, this);
					if (move is not null) validMoves.Add(move);
				}
			}
		}
		return validMoves;
	}

	internal Piece PromotePawn(Piece pawn, Type type)
	{
		pawn.IsCaptured = true;
		var piece = PieceFactory.PromotePiece(pawn, type);
		Pieces.Add(piece);
		return piece;
	}

	public override string ToString()
	{
		// return FEN representation of the board
		StringBuilder fen = new();
		for (int y = 7; y >= 0; y--)
		{
			int empty = 0;
			for (int x = 0; x < 8; x++)
			{
				var piece = GetPieceAt(new(x, y));
				if (piece is null)
				{
					empty++;
					continue;
				}
				if (empty > 0)
				{
					fen.Append(empty);
					empty = 0;
				}
				fen.Append(piece.ToString());
			}
			if (empty > 0) fen.Append(empty);
			if (y > 0) fen.Append('/');
		}
		fen.Append(WhiteToMove ? " w " : " b ");
		if (!whiteKing.HasMoved && !(GetPieceAt(new Vector2(7,0))?.HasMoved ?? true)) fen.Append('K');
		if (!whiteKing.HasMoved && !(GetPieceAt(new Vector2(0, 0))?.HasMoved ?? true)) fen.Append('Q');
		if (!blackKing.HasMoved && !(GetPieceAt(new Vector2(7, 7))?.HasMoved ?? true)) fen.Append('k');
		if (!blackKing.HasMoved && !(GetPieceAt(new Vector2(0, 7))?.HasMoved ?? true)) fen.Append('q');
		if (fen[^1] == ' ') fen.Append('-');
		fen.Append(' ');
		if (LastMove is not null && LastMove.MovePieceType == typeof(Pawn) && LastMove.IsFirstMoveOfPiece) fen.Append(MoveHelper.ToFieldString(LastMove.Start + (Vector2.UnitY * (LastMove.OfWhite ? 1 : -1))));
		else fen.Append('-');
		fen.Append(' ');

		// TODO: implement counting 50 move rule
		fen.Append("0 ");

		fen.Append(MoveCount / 2 + 1); // halfmove clock, incremented after each move
		return fen.ToString();
	}

	internal bool IsInCheck => MoveHelper.IsAttackedBy(GetKing(WhiteToMove).Position, this, !WhiteToMove);
	internal bool IsChecking => MoveHelper.IsAttackedBy(GetKing(!WhiteToMove).Position, this, WhiteToMove);
	internal bool IsMate => IsInCheck && GetValidMoves().Count == 0;
	internal bool IsDraw => MoveHelper.CalculateDraw(this);
}
