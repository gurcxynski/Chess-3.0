using Microsoft.Xna.Framework;

namespace Chess.Core;
internal class Move
{
    internal Vector2 Start { get; private init; }
    internal Vector2 End { get; private init; }
    internal bool OfWhite { get; private init; }
	internal PieceType MovePieceType { get; private init; }
	internal PieceType CapturePieceType { get; private init; }
	internal PieceType PromotionPieceType { get; private init; }
	internal bool IsCapture { get; private init; }
	internal bool IsEnPassant { get; private init; }
	internal bool IsPromotion { get; private init; }
	internal bool IsCastles { get; private init; }
    internal Move(Vector2 start, Vector2 end, Board board)
    {
        Start = start;
        End = end;
        OfWhite = board.GetPieceAt(start).IsWhite;
        MovePieceType = board.GetPieceAt(start).Type;
        IsCapture = board.GetPieceAt(end) != null;
        if (IsCapture) CapturePieceType = board.GetPieceAt(end).Type;
    }
}