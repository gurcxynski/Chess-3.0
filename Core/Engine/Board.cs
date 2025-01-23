using Chess.Core.Engine.Pieces;
using Chess.Core.UI.Menus;
using Chess.Core.Util;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Core.Engine;

internal class Board
{
    internal List<Piece> Pieces { get; private init; } = [];
    internal IEnumerable<Piece> WhitePieces => Pieces.Where(piece => piece.IsWhite);
    internal IEnumerable<Piece> BlackPieces => Pieces.Where(piece => !piece.IsWhite);
    internal IEnumerable<Piece> CapturedPieces => Pieces.Where(piece => piece.IsCaptured);
    internal IEnumerable<Piece> ActivePieces => Pieces.Where(piece => !piece.IsCaptured);
    internal List<Move> ValidMoves => GetValidMoves();

    private readonly Stack<Move> moveHistory = new();
    internal Queue<Move> LastFifty { get; private init; } = new();
    internal bool WhiteToMove { get; private set; }
    internal int MoveCount => moveHistory.Count;
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

    internal Piece GetPieceAt(Vector2 pos) => Pieces.Find(piece => piece.Position == pos && !piece.IsCaptured);
    internal List<Piece> GetAll(Type type) => Pieces.Where(piece => piece.GetType() == type).ToList();
    internal Piece GetKing(bool isWhite) => isWhite ? whiteKing : blackKing;
    internal Move LastMove => moveHistory.Count > 0 ? moveHistory.Peek() : null;

    internal void ExecuteMove(Move move, bool isReal = true)
    {
        if (move.IsCapture) move.CapturedPiece.IsCaptured = true;
        move.MovedPiece.Move(move.End);
        WhiteToMove = !WhiteToMove;
        moveHistory.Push(move);
        LastFifty.Enqueue(move);
        if (LastFifty.Count > 50) LastFifty.Dequeue();
        if (isReal && IsMate) StateMachine.ToMenu<StartMenu>();
        if (!move.IsCastles) return;
        if (move.End.X == 2)
        {
            Piece rook = GetPieceAt(new(0, move.Start.Y));
            rook.Move(new(3, move.Start.Y));
        }
        else if (move.End.X == 6)
        {
            Piece rook = GetPieceAt(new(7, move.Start.Y));
            rook.Move(new(5, move.Start.Y));
        }
    }

    internal void UndoMove()
    {
        if (MoveCount == 0) return;
        Move move = moveHistory.Pop();
        move.MovedPiece.Move(move.Start);
        WhiteToMove = !WhiteToMove;
        if (move.IsCapture) move.CapturedPiece.IsCaptured = false;
        if (move.IsFirstMoveOfPiece) move.MovedPiece.HasMoved = false;
        if (!move.IsCastles) return;
        if (move.End.X == 2)
        {
            Piece rook = GetPieceAt(new(3, move.Start.Y));
            rook.Move(new(0, move.Start.Y));
        }
        if (move.End.X == 6)
        {
            Piece rook = GetPieceAt(new(5, move.Start.Y));
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
                    Move move = piece.TryCreatingMove(pos, this);
                    if (move is not null) validMoves.Add(move);
                }
            }
        }
        return validMoves;
    }

    internal bool IsInCheck => MoveHelper.IsAttackedBy(GetKing(WhiteToMove).Position, this, !WhiteToMove);
    internal bool IsChecking => MoveHelper.IsAttackedBy(GetKing(!WhiteToMove).Position, this, WhiteToMove);
    internal bool IsMate => IsInCheck && GetValidMoves().Count == 0;
    internal bool IsDraw => MoveHelper.CalculateDraw(this);
}
