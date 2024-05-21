using System;
using System.Collections.Generic;
using Chess.Graphics;
using Chess.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Input.InputListeners;

namespace Chess.Core;
internal class ChessGame : GameScreen {
    private readonly Board board = new();
    private readonly List<PieceDrawable> dPieces = new();
    private readonly MouseListener mouseListener = new();
    private readonly KeyboardListener keyboardListener = new();
    private readonly MyBot bot = new();
    private readonly Animator animator;
    private Vector2 heldRelative;
    private class Animator {
        internal bool animating = false;
        TimeSpan start;
        const int time = 150;
        internal Move move;
        internal Piece piece;
        internal Vector2 Position => Vector2.Lerp(Converter.GridToDraw(move.Start), Converter.GridToDraw(move.End), progress);
        float progress;
        internal void Start(TimeSpan time, Move move, Piece piece) {
            start = time;
            this.move = move;
            this.piece = piece;
            animating = true;
        }
        void Stop() {
            animating = false;
            progress = 0;
            move = null;
            piece = null;
            start = TimeSpan.Zero;
        }
        #nullable enable
        internal Move? Update(GameTime gameTime)
        {
            if (!animating) return null;
            progress = (float)((gameTime.TotalGameTime - start).TotalMilliseconds / time);
            if (progress >= 1) {
                var m = move;
                Stop();
                return m;
            }
            return null;
        }
        #nullable disable
    }

    

    private Piece SelectedPiece;
    internal ChessGame() {
        Bounds = new Rectangle(0, 0, 8 * Game1.Size, 8 * Game1.Size);
        Background = Game1.self.textures.Get("brown");
        animator = new Animator();
        board.Pieces.ForEach(piece => dPieces.Add(new PieceDrawable(Bounds.Location.ToVector2(), piece)));
        mouseListener.MouseUp += (sender, args) => {
            Vector2 pos = new(args.Position.X / Game1.Size, 7 - args.Position.Y / Game1.Size);
            Piece clicked = board.GetPieceAt(pos);
            if (SelectedPiece is not null && (clicked is null || clicked.IsWhite != board.WhiteToMove)) {
                Move move = SelectedPiece.CreateMove(pos, board);
                if (move is not null) {
                    animator.Start(args.Time, move, SelectedPiece);
                }
            }
            SelectedPiece = null;

        };
        mouseListener.MouseDown += (sender, args) => {
            Vector2 pos = new(args.Position.X / Game1.Size, 7 - args.Position.Y / Game1.Size);
            Piece clicked = board.GetPieceAt(pos);
            if (clicked is null) return;
            if (clicked.IsWhite == board.WhiteToMove) {
                SelectedPiece = clicked;
                heldRelative = Converter.GridToDraw(pos) - args.Position.ToVector2();
            }
        };
        keyboardListener.KeyTyped += (sender, args) => {
            SelectedPiece = null;
            board.UndoMove();
            board.UndoMove();
            dPieces.ForEach(piece => piece.UpdatePosition());
        };
    }
    internal override void Update(GameTime gameTime) {
        var move = animator.Update(gameTime);
        if (move is not null) {
            board.ExecuteMove(move);
            dPieces.ForEach(piece => piece.UpdatePosition());
            return;
        }
        if (animator.animating) return;
        if (board.IsMate) {
            return;
        }
        mouseListener.Update(gameTime);
        keyboardListener.Update(gameTime);
        if (!board.WhiteToMove) {
            Move botMove = bot.Think(board);
            animator.Start(gameTime.TotalGameTime, botMove, board.GetPieceAt(botMove.Start));
        }
        
    }
    internal override void Draw(SpriteBatch spriteBatch) {
        base.Draw(spriteBatch);

        if (board.IsInCheck) 
            spriteBatch.FillRectangle(
                new RectangleF(Converter.GridToDraw(board.GetKing(board.WhiteToMove).Position), new Size2(Game1.Size, Game1.Size)), Color.Red * 0.8f);
        if (board.IsChecking) 
            spriteBatch.FillRectangle(
                new RectangleF(Converter.GridToDraw(board.GetKing(!board.WhiteToMove).Position), new Size2(Game1.Size, Game1.Size)), Color.Red * 0.8f);            
        
        if (SelectedPiece is not null) {
            spriteBatch.FillRectangle(
                new RectangleF(Converter.GridToDraw(SelectedPiece.Position), new Size2(Game1.Size, Game1.Size)), Color.Yellow * 0.8f);
        
            for (int i = 0; i < 8; i++) {
                for (int j = 0; j < 8; j++) {
                    Vector2 pos = new(i, j);
                    if (SelectedPiece.Position != pos && SelectedPiece.CreateMove(pos, board) is not null) {
                        spriteBatch.FillRectangle(
                            new RectangleF(Converter.GridToDraw(pos), new Size2(Game1.Size, Game1.Size)), Color.Cyan * 0.8f);
                    }
                }
            }
        }
            
        dPieces.ForEach(piece => 
        { if (piece.Piece != SelectedPiece && piece.Piece != animator.piece && !board.CapturedPieces.Contains(piece.Piece)) piece.Draw(spriteBatch); });

        dPieces.Find(piece => piece.Piece == SelectedPiece)?.DrawAt(spriteBatch, heldRelative + Mouse.GetState().Position.ToVector2());

        dPieces.Find(piece => piece.Piece == animator.piece)?.DrawAt(spriteBatch, animator.Position);
    }
}
