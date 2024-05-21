using System;
using System.Collections.Generic;
using System.Linq;
using Chess.Pieces;
using Chess.Util;
using Microsoft.Xna.Framework;

namespace Chess.Core;
internal class Board {
    internal List<Piece> Pieces { get; private init; } = new();
    internal IEnumerable<Piece> WhitePieces => Pieces.Where(piece => piece.IsWhite);
    internal IEnumerable<Piece> BlackPieces => Pieces.Where(piece => !piece.IsWhite);
    internal List<Piece> CapturedPieces { get; private init; } = new();
    private readonly Stack<Move> moveList = new();
    internal bool WhiteToMove { get; private set; }
    internal int MoveCount => moveList.Count;
    private King white;
    private King black;
    internal Board() {
        FillBoard();
        WhiteToMove = true;
    }
    private void FillBoard() {
        for (int x = 0; x < 8; x++) {
            Pieces.Add(new Pawn(new(x, 1)));
            Pieces.Add(new Pawn(new(x, 6), false));
        }
        Pieces.Add(new Rook(new(0, 0)));
        Pieces.Add(new Rook(new(7, 0)));
        Pieces.Add(new Rook(new(0, 7), false));
        Pieces.Add(new Rook(new(7, 7), false));
        Pieces.Add(new Knight(new(1, 0)));
        Pieces.Add(new Knight(new(6, 0)));
        Pieces.Add(new Knight(new(1, 7), false));
        Pieces.Add(new Knight(new(6, 7), false));
        Pieces.Add(new Bishop(new(2, 0)));
        Pieces.Add(new Bishop(new(5, 0)));
        Pieces.Add(new Bishop(new(2, 7), false));
        Pieces.Add(new Bishop(new(5, 7), false));
        Pieces.Add(new Queen(new(3, 0)));
        Pieces.Add(new Queen(new(3, 7), false));
        white = new King(new(4, 0));
        black = new King(new(4, 7), false);
        Pieces.Add(white);
        Pieces.Add(black);
    }
    internal Piece GetPieceAt(Vector2 pos) {
        return Pieces.Find(piece => piece.Position == pos);
    }
    internal List<Piece> GetAll(Type type) =>  Pieces.Where(piece => piece.GetType() == type).ToList();
    internal Piece GetKing(bool isWhite) => isWhite ? white : black;
    internal Move LastMove => moveList.Count > 0 ? moveList.Peek() : null;

    internal void ExecuteMove(Move move) {
        Piece piece = GetPieceAt(move.Start);
        if (move.IsCapture) {
            CapturedPieces.Add(move.Captured);
            Pieces.Remove(move.Captured);
        }
        piece.Move(move.End);
        WhiteToMove = !WhiteToMove;
        moveList.Push(move);
        if (move.IsCastles) {
            if (move.End.X == 2) {
                Piece rook = GetPieceAt(new(0, move.Start.Y));
                rook.Move(new(3, move.Start.Y));
            }
            if (move.End.X == 6) {
                Piece rook = GetPieceAt(new(7, move.Start.Y));
                rook.Move(new(5, move.Start.Y));
            }
        }
        return;
    }

    internal void UndoMove() {
        if (MoveCount == 0) return;
        Move move = moveList.Pop();
        Piece piece = GetPieceAt(move.End);
        piece.Move(move.Start);
        WhiteToMove = !WhiteToMove;
        if (move.IsCapture) {
            CapturedPieces.Remove(move.Captured);
            Pieces.Add(move.Captured);
        }
        if (move.IsFirstMoveOfPiece) piece.HasMoved = false;
        if (move.IsCastles) {
            if (move.End.X == 2) {
                Piece rook = GetPieceAt(new(3, move.Start.Y));
                rook.Move(new(0, move.Start.Y));
            }
            if (move.End.X == 6) {
                Piece rook = GetPieceAt(new(5, move.Start.Y));
                rook.Move(new(7, move.Start.Y));
            }
        }
    }
    internal List<Move> GetValidMoves() {
        List<Move> validMoves = new();
        var piecesCopy = new List<Piece>(Pieces);
        foreach (Piece piece in piecesCopy.Where(piece => piece.IsWhite == WhiteToMove)) {
            for (int x = 0; x < 8; x++) {
                for (int y = 0; y < 8; y++) {
                    Vector2 pos = new(x, y);
                    if (pos == piece.Position) continue;
                    Move move = piece.CreateMove(pos, this);
                    if (move is not null) validMoves.Add(move);
                }
            }
        }
        return validMoves;
    }

    internal bool HasKingsideCastleRight(bool playingWhite)
    {
        var move = GetKing(playingWhite).CreateMove(new(6, playingWhite ? 0 : 7), this);
        return move is not null;
    }

    internal bool HasQueensideCastleRight(bool playingWhite)
    {
        var move = GetKing(playingWhite).CreateMove(new(2, playingWhite ? 0 : 7), this);
        return move is not null;
    }

    internal bool IsInCheck => MoveHelper.IsAttackedBy(GetKing(WhiteToMove).Position, this, !WhiteToMove);
    internal bool IsChecking => MoveHelper.IsAttackedBy(GetKing(!WhiteToMove).Position, this, WhiteToMove);
    internal bool IsMate => IsInCheck && GetValidMoves().Count == 0;
}
