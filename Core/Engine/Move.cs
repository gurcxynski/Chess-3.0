using Chess.Core.Util;
using Microsoft.Xna.Framework;
using System;

namespace Chess.Core.Engine
{
    internal class Move(Vector2 start, Vector2 end, Piece movedPiece, Piece capturedPiece = null, bool castles = false, bool promotion = false, bool firstMove = false)
    {
        internal Vector2 Start { get; private init; } = start;
        internal Vector2 End { get; private init; } = end;
        internal Piece MovedPiece { get; private init; } = movedPiece;
        internal Piece CapturedPiece { get; private init; } = capturedPiece;
        internal bool OfWhite => MovedPiece.IsWhite;
        internal Type MovePieceType => MovedPiece.GetType();
        internal Type CapturePieceType => CapturedPiece?.GetType();
        internal Type PromotionPieceType { get; private init; }
        internal bool IsCapture => CapturedPiece is not null;
        internal bool IsPromotion { get; private init; } = promotion;
        internal bool IsCastles { get; private init; } = castles;
        internal bool IsFirstMoveOfPiece { get; private init; } = firstMove;
        override public string ToString() => IsCastles ? "O-O": MoveHelper.ToFieldString(Start) + MoveHelper.ToFieldString(End);
    }
}
