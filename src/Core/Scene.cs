using MonoGame.Extended.Input.InputListeners;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Chess.Core;
internal class Scene {
    private readonly MouseListener mouseListener = new();
    List<Piece> pieces = new();
    Piece selectedPiece;
    private void FillBoard() {
        for (int i = 0; i < 8; i++) {
            pieces.Add(new Piece(PieceType.Pawn, new Vector2(i, 1)));
            pieces.Add(new Piece(PieceType.Pawn, new Vector2(i, 6)));
        }
        pieces.Add(new Piece(PieceType.Rook, new Vector2(0, 0)));
        pieces.Add(new Piece(PieceType.Rook, new Vector2(7, 0)));
        pieces.Add(new Piece(PieceType.Rook, new Vector2(0, 7)));
        pieces.Add(new Piece(PieceType.Rook, new Vector2(7, 7)));
        pieces.Add(new Piece(PieceType.Knight, new Vector2(1, 0)));
        pieces.Add(new Piece(PieceType.Knight, new Vector2(6, 0)));
        pieces.Add(new Piece(PieceType.Knight, new Vector2(1, 7)));
        pieces.Add(new Piece(PieceType.Knight, new Vector2(6, 7)));
        pieces.Add(new Piece(PieceType.Bishop, new Vector2(2, 0)));
        pieces.Add(new Piece(PieceType.Bishop, new Vector2(5, 0)));
        pieces.Add(new Piece(PieceType.Bishop, new Vector2(2, 7)));
        pieces.Add(new Piece(PieceType.Bishop, new Vector2(5, 7)));
        pieces.Add(new Piece(PieceType.Queen, new Vector2(3, 0)));
        pieces.Add(new Piece(PieceType.Queen, new Vector2(3, 7)));
        pieces.Add(new Piece(PieceType.King, new Vector2(4, 0)));
        pieces.Add(new Piece(PieceType.King, new Vector2(4, 7)));
    }
    internal Scene() {
        FillBoard();
        mouseListener.MouseClicked += (sender, args) => {
            var mousePosition = args.Position;
            foreach (var piece in pieces) {
                if (Vector2.Distance(piece.GridPosition * 100, mousePosition.ToVector2()) < 50) {
                    selectedPiece = piece;
                    break;
                }
            }
        };
        
    }

    internal void Update(GameTime gameTime)
    {
        mouseListener.Update(gameTime);
    }
    internal void Draw(SpriteBatch spriteBatch)
    {
        foreach (var piece in pieces)
        {
            piece.Draw(spriteBatch);
        }
    }
}