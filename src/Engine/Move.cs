using Chess.Util;
using Microsoft.Xna.Framework;

namespace Chess.Engine;
internal class Move
{
    internal Vector2 Start { get; private init; }
    internal Vector2 End { get; private init; }
    internal bool OfWhite { get; private init; }
	internal System.Type MovePieceType { get; private init; }
	internal System.Type CapturePieceType { get; private init; }
	internal System.Type PromotionPieceType { get; private init; }
	internal bool IsCapture => CapturePieceType is not null;
    internal Piece CapturedPiece { get; private init; }
	internal bool IsEnPassant { get; private init; }
	internal bool IsPromotion { get; private init; }
	internal bool IsCastles { get; private init; }
    internal bool IsFirstMoveOfPiece { get; private init; } = false;
    internal Move(Vector2 start, Vector2 end, Board board, bool castles = false, bool enPassant = false, bool promotion = false, bool firstMove = false)
    {
        Start = start;
        End = end;
        OfWhite = board.WhiteToMove;
        IsCastles = castles;
        IsEnPassant = enPassant;
        IsPromotion = promotion;
        IsFirstMoveOfPiece = firstMove;
        Piece piece = board.GetPieceAt(start);
        MovePieceType = piece.GetType();
        CapturedPiece = IsEnPassant ? board.GetPieceAt(end + new Vector2(0, OfWhite ? -1 : 1)) : board.GetPieceAt(end);
        if (CapturedPiece is not null) CapturePieceType = CapturedPiece.GetType();
    }
}