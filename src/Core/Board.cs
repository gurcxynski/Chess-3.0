using System;
using System.Collections.Generic;
using System.Linq;
using Chess.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chess.Core;
internal class Board {
    private readonly List<Piece> pieces = new();
    private readonly Stack<Move> moveList = new();
    internal bool? WhiteToMove { get; private set; }
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
        return pieces.Find(piece => piece.GridPosition == pos);
    }
    public static void DrawBoard(SpriteBatch spriteBatch)
    {
        Texture2D texture = Game1.self.textures.Get("brown");
        Rectangle destinationRectangle = new(0, 0, 8 * Game1.Size, 8 * Game1.Size);
        spriteBatch.Draw(texture, destinationRectangle, texture.Bounds, Color.White);
    }
    public void DrawPieces(SpriteBatch spriteBatch) => pieces.ForEach(piece => piece.Draw(spriteBatch));

    internal bool ExecuteMove(Move move) {
        if (move.OfWhite != WhiteToMove) return false;
        if (!Validator.CheckTheory(move)) return false;
        if (move.IsCapture) {
            Piece captured = GetPieceAt(move.End);
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
        if (move.IsCapture) {
            pieces.Add(new Piece(move.CapturePieceType, move.End, !move.OfWhite));
        }
        WhiteToMove = !WhiteToMove;
    }
}
