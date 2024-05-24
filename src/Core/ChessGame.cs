using System;
using System.Collections.Generic;
using System.Linq;
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
        internal struct AnimationData {
            internal Move move;
            internal Piece piece;
            internal bool reverse;
        }
        readonly List<AnimationData> data;
        readonly Board board;
        internal bool Animating => data.Count > 0;
        const double length = 100;
        TimeSpan start;
        double progress;
        internal IEnumerable<Piece> Pieces => data.Select(d => d.piece);
        internal Animator(Board board) {
            this.board = board;
            data = new();
        }
        internal Vector2 Position(Piece p){
            var d = data.Find(d => d.piece == p);
            Console.WriteLine(p.GetType().Name);
            return Vector2.Lerp(
                Converter.GridToDraw(d.reverse ? d.move.End : d.move.Start), 
                Converter.GridToDraw(d.reverse ? d.move.Start : d.move.End), (float)progress);
        } 
        internal void Start(TimeSpan time, Move move, bool reverse = false) {
            Console.WriteLine("added" + board.GetPieceAt(reverse ? move.End : move.Start));
            start = time;
            data.Add(new() { move = move, piece = board.GetPieceAt(reverse ? move.End : move.Start), reverse = reverse });
        }
        internal List<AnimationData> Update(GameTime gameTime)
        {
            var ret = new List<AnimationData>();
            if (!Animating) return ret;
            progress = (gameTime.TotalGameTime - start).TotalMilliseconds / length;
            if (progress >= 1) {
                ret.AddRange(data);
                data.Clear();
                start = default;
                progress = 0;
            }
            return ret;
        }
    }


    private Piece SelectedPiece;
    internal ChessGame() {
        Bounds = new Rectangle(0, 0, 8 * Game1.Size, 8 * Game1.Size);
        Background = Game1.self.textures.Get("brown");
        animator = new Animator(board);
        board.Pieces.ForEach(piece => dPieces.Add(new PieceDrawable(Bounds.Location.ToVector2(), piece)));
        mouseListener.MouseUp += (sender, args) => {
            Vector2 pos = new(args.Position.X / Game1.Size, 7 - args.Position.Y / Game1.Size);
            if (pos.X < 0 || pos.X > 7 || pos.Y < 0 || pos.Y > 7) return;
            Piece clicked = board.GetPieceAt(pos);
            if (SelectedPiece is not null && (clicked is null || clicked.IsWhite != board.WhiteToMove)) {
                Move move = SelectedPiece.CreateMove(pos, board);
                if (move is not null) {       
                    board.ExecuteMove(move);
                    dPieces.ForEach(piece => piece.UpdatePosition());
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
        mouseListener.MouseWheelMoved += (sender, args) => {
            if (board.MoveCount <= 1) return;
            SelectedPiece = null;
            animator.Start(args.Time, board.LastMove, true);
            animator.Start(args.Time, board.PreLastMove(), true);
        };
    }
    internal override void Update(GameTime gameTime) {
        var animations = animator.Update(gameTime);
        if (animations.Count > 0) {
            animations.ForEach((animation) => {
                if (animation.reverse) {
                    board.UndoMove();
                } else {
                    board.ExecuteMove(animation.move);
                }
            });
            dPieces.ForEach(piece => piece.UpdatePosition());
        }
        if (animator.Animating) return;
        if (board.IsMate) {
            return;
        }
        //if (!board.WhiteToMove) {
        //    Move botMove = bot.Think(board);
        //    animator.Start(gameTime.TotalGameTime, botMove);
        //}
        mouseListener.Update(gameTime);
        keyboardListener.Update(gameTime);
        
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
        { if (piece.Piece != SelectedPiece && !animator.Pieces.Contains(piece.Piece) && !board.CapturedPieces.Contains(piece.Piece)) piece.Draw(spriteBatch); });

        dPieces.Find(piece => piece.Piece == SelectedPiece)?.DrawAt(spriteBatch, heldRelative + Mouse.GetState().Position.ToVector2());

        dPieces.ForEach(piece => 
        { if (animator.Pieces.Contains(piece.Piece)) piece.DrawAt(spriteBatch, animator.Position(piece.Piece)); });
    }
}
