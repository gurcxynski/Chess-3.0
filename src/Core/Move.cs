using Chess.Util;
using Microsoft.Xna.Framework;

namespace Chess.Core;
internal class Move
{
    internal Vector2 Start { get; private init; }
    internal Vector2 End { get; private init; }
    internal bool OfWhite { get; private init; }
	internal System.Type MovePieceType { get; private init; }
	internal System.Type CapturePieceType { get; private init; }
	internal System.Type PromotionPieceType { get; private init; }
	internal bool IsCapture => CapturePieceType is not null;
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
        Piece captured = board.GetPieceAt(end);
        if (captured is not null) CapturePieceType = captured.GetType();
    }
}