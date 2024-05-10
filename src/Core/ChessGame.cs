using System;
using System.Collections.Generic;
using Chess.Graphics;
using Chess.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Input.InputListeners;

namespace Chess.Core;
internal class ChessGame : GameScreen {
    private readonly Board board = new();
    private readonly List<PieceDrawable> dPieces = new();
    private readonly MouseListener mouseListener = new();
    private readonly KeyboardListener keyboardListener = new();
    private readonly MyBot bot = new();

    private Piece SelectedPiece;
    internal ChessGame() {
        Bounds = new Rectangle(0, 0, 8 * Game1.Size, 8 * Game1.Size);
        Background = Game1.self.textures.Get("brown");
        board.pieces.ForEach(piece => dPieces.Add(new PieceDrawable(Bounds.Location.ToVector2(), piece)));
        mouseListener.MouseUp += (sender, args) => {
            if (!board.WhiteToMove) return;
            Vector2 pos = new(args.Position.X / Game1.Size, 7 - args.Position.Y / Game1.Size);
            Piece clicked = board.GetPieceAt(pos);
            if (SelectedPiece is null && clicked is null) return;
            if (SelectedPiece is not null && (clicked is null || clicked.IsWhite != board.WhiteToMove)) {
                if (board.ExecuteMove(new Move(SelectedPiece.Position, pos, board))) {
                    SelectedPiece = null;
                    dPieces.ForEach(piece => piece.UpdatePosition());
                }
                return;
            }
            if (clicked.IsWhite == board.WhiteToMove) SelectedPiece = clicked;

        };
        keyboardListener.KeyTyped += (sender, args) => {
            board.UndoMove();
            dPieces.ForEach(piece => piece.UpdatePosition());
        };
    }
    internal override void Update(GameTime gameTime) {
        mouseListener.Update(gameTime);
        keyboardListener.Update(gameTime);
        if (!board.WhiteToMove) {
            Move botMove = bot.Think(board);
            board.ExecuteMove(botMove);
            dPieces.ForEach(piece => piece.UpdatePosition());
        }
        
    }
    internal override void Draw(SpriteBatch spriteBatch) {
        base.Draw(spriteBatch);
        if (SelectedPiece is not null) 
            spriteBatch.FillRectangle(new RectangleF(Converter.GridToDraw(SelectedPiece.Position), new Size2(Game1.Size, Game1.Size)), new Color(0.6f, 0.6f, 0, 0.1f));
        dPieces.ForEach(piece => {if (!board.capturedPieces.Contains(piece.Piece)) piece.Draw(spriteBatch);});
    }
}
