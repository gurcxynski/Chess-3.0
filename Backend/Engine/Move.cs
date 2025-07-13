using Backend.Util;
using System.Numerics;

namespace Backend.Engine;

public class Move(Vector2 start, Vector2 end, Piece movedPiece, Piece? capturedPiece = null, bool castles = false, bool promotion = false, Type? promotiontype = null, bool firstMove = false, bool isCheck = false, bool isMate = false)
{
	internal Vector2 Start { get; private init; } = start;
	internal Vector2 End { get; private init; } = end;
	internal Piece MovedPiece { get; private init; } = movedPiece;
	internal Piece? CapturedPiece { get; private init; } = capturedPiece;
	internal bool OfWhite => MovedPiece.IsWhite;
	internal Type MovePieceType => MovedPiece.GetType();
	internal Type? CapturePieceType => CapturedPiece?.GetType();
	internal Type? PromotionPieceType { get; init; } = promotiontype;
	internal bool IsCapture => CapturedPiece is not null;
	internal bool IsPromotion { get; private init; } = promotion;
	internal bool IsCastles { get; private init; } = castles;
	internal bool IsFirstMoveOfPiece { get; private init; } = firstMove;
	internal bool IsCheck { get; private init; } = isCheck;
	internal bool IsMate { get; private init; } = isMate;
	override public string ToString() => MoveHelper.ToFieldString(Start) + MoveHelper.ToFieldString(End) + (PromotionPieceType?.Name[0].ToString().ToLower() ?? string.Empty);
	public string AlgebraicNotation
	{
		get
		{
			string result = "";
			if (IsCastles) return Start.X < End.X ? "O-O" : "O-O-O";
			result += MovePieceType.Name switch
        	{
        	    "Knight" => "N",
        	    "Bishop" => "B",
        	    "Rook" => "R",
        	    "Queen" => "Q",
        	    "King" => "K",
        	    _ => "",
        	};
			if (IsCapture) result += "x";
			result += MoveHelper.ToFieldString(End);
			if (IsPromotion) result += "=" + (PromotionPieceType?.Name[0].ToString().ToLower() ?? string.Empty);
			if (IsMate) result += "#";
			else if (IsCheck) result += "+";
			return result;
		}
	}
}
