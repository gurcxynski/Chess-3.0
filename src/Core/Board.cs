using System.Collections.Generic;
using System.Linq;
using Chess.Util;
using Microsoft.Xna.Framework;

namespace Chess.Core;
internal class Board {
    internal List<Piece> pieces { get; private init; } = new();
    internal List<Piece> capturedPieces { get; private init; } = new();
    private readonly Stack<Move> moveList = new();
    internal bool WhiteToMove { get; private set; }
    internal int MoveCount => moveList.Count;
    internal Board() {
        FillBoard();
        WhiteToMove = true;
    }
    private void FillBoard() {
        for (int i = 0; i < 8; i++) {
            pieces.Add(new Piece(PieceType.Pawn, new Vector2(i, 1)));
            pieces.Add(new Piece(PieceType.Pawn, new Vector2(i, 6), false));
        }
        pieces.Add(new Piece(PieceType.Rook, new Vector2(0, 0)));
        pieces.Add(new Piece(PieceType.Rook, new Vector2(7, 0)));
        pieces.Add(new Piece(PieceType.Knight, new Vector2(1, 0)));
        pieces.Add(new Piece(PieceType.Knight, new Vector2(6, 0)));
        pieces.Add(new Piece(PieceType.Bishop, new Vector2(2, 0)));
        pieces.Add(new Piece(PieceType.Bishop, new Vector2(5, 0)));
        pieces.Add(new Piece(PieceType.Queen, new Vector2(3, 0)));
        pieces.Add(new Piece(PieceType.King, new Vector2(4, 0)));
        
        pieces.Add(new Piece(PieceType.Rook, new Vector2(0, 7), false));
        pieces.Add(new Piece(PieceType.Rook, new Vector2(7, 7), false));
        pieces.Add(new Piece(PieceType.Knight, new Vector2(1, 7), false));
        pieces.Add(new Piece(PieceType.Knight, new Vector2(6, 7), false));
        pieces.Add(new Piece(PieceType.Bishop, new Vector2(2, 7), false));
        pieces.Add(new Piece(PieceType.Bishop, new Vector2(5, 7), false));
        pieces.Add(new Piece(PieceType.Queen, new Vector2(3, 7), false));
        pieces.Add(new Piece(PieceType.King, new Vector2(4, 7), false));
    }
    internal Piece GetPieceAt(Vector2 pos) {
        return pieces.Find(piece => piece.Position == pos);
    }

    internal bool ExecuteMove(Move move) {
        if (move.OfWhite != WhiteToMove) return false;
        if (!MoveValidator.CheckTheory(move)) return false;
        if (!MoveValidator.CheckPath(move, this)) return false;
        if (move.IsCapture) {
            Piece captured = GetPieceAt(move.End);
            capturedPieces.Add(captured);
            pieces.Remove(captured);
        }
        Piece piece = GetPieceAt(move.Start);
        piece.Move(move.End);
        WhiteToMove = !WhiteToMove;
        moveList.Push(move);
        return true;
    }

    internal void UndoMove() {
        if (MoveCount == 0) return;
        Move move = moveList.Pop();
        Piece piece = GetPieceAt(move.End);
        piece.Move(move.Start);
        WhiteToMove = !WhiteToMove;
        if (move.IsCapture) {
            Piece captured = capturedPieces.Last();
            capturedPieces.Remove(captured);
            pieces.Add(captured);
        }
    }
    internal List<Move> GetValidMoves() {
        List<Move> validMoves = new();
        foreach (Piece piece in pieces.Where(piece => piece.IsWhite == WhiteToMove)) {
            for (int x = 0; x < 8; x++) {
                for (int y = 0; y < 8; y++) {
                    Vector2 pos = new(x, y);
                    if (pos == piece.Position) continue;
                    Move move = new(piece.Position, pos, this);
                    if (MoveValidator.CheckTheory(move) && MoveValidator.CheckPath(move, this)) {
                        validMoves.Add(move);
                    }
                }
            }
        }
        return validMoves;
    }

    internal bool HasKingsideCastleRight(bool playingWhite)
    {
        return false;
    }

    internal bool HasQueensideCastleRight(bool playingWhite)
    {
        return false;
    }

    internal bool IsInCheck()
    {
        return false;
    }
}
