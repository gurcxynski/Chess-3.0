using Chess.Core.Engine;
using Chess.Core.UI.Graphics;
using Chess.Core.Util;
using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Core;
class MoveEventArgs(IEnumerable<Move> moveHistory, int time) : EventArgs
{
    internal IEnumerable<Move> MoveHistory { get; private set; } = moveHistory;
    internal int Time { get; private set; } = time;
}
internal class ChessGame : Panel
{
    internal static ChessGame Instance { get; private set; }

    internal readonly Board Board = new(PositionLoader.LoadBoardSetup("boardSetup.json"));
    internal readonly bool IsWhitePlayer;

    IMoveReceiver Opponent { get; init; }
    internal Vector2 BoardSize => Size - 2 * Padding;

    internal ChessGame(IMoveReceiver receiver, bool white)
    {
        Instance = this;

        IsWhitePlayer = white;
        Opponent = receiver;
        Opponent.OnMoveDataReceived += (sender, e) =>
        {
            var move = MoveHelper.TryCreatingMove(System.Text.Encoding.UTF8.GetString(e.Item1));
            ExecuteMove(move);
        };

        Anchor = Anchor.Center;
        Size = new Vector2(Math.Min(UserInterface.Active.ScreenHeight, UserInterface.Active.ScreenWidth) * 0.8f); 
    }
    internal void Initialize()
    {
        for (int i = 0; i < 8; i++) for (int j = 0; j < 8; j++) AddChild(new BoardSquare(j, IsWhitePlayer ? 7 - i : i));

        Board.Pieces.ForEach((piece) =>
        {
            var icon = new PieceIcon(piece)
            {
                OnMouseReleased = (entity) =>
                {
                    PieceMovedByMouse(piece);
                    UpdateIcons();
                    UpdateSquares();
                },
                OnMouseDown = (entity) => PiecePickedUp(piece)
            };
            AddChild(icon);
        });

        UpdateIcons();
        if (!IsWhitePlayer) Opponent.Listen();
    }
    protected virtual void PieceMovedByMouse(Piece piece)
    {
        var clicked = PositionConverter.ToGrid(MouseInput.MousePosition);
        if (clicked.X < 0 || clicked.X > 7 || clicked.Y < 0 || clicked.Y > 7 || clicked == piece.Position) return;

        var move = piece.TryCreatingMove(clicked, Board);
        if (ExecuteMove(move)) Opponent.ProcessMove(move);
    }
    private bool ExecuteMove(Move move)
    {
        if (move == null) return false;
        Board.ExecuteMove(move);
        if (move.IsCapture) Chess.Instance.captureSound.Play();
        else Chess.Instance.moveSound.Play();
        if (move.IsPromotion)
        {
            var popup = new UI.PromotionPopup(move.End, move.OfWhite);
            AddChild(popup);
            var task = popup.WaitForPromotion();
            task.Wait();
        }
        UpdateIcons();
        UpdateSquares();
        return true;
    }
    protected virtual void PiecePickedUp(Piece piece)
    {
        if (!IsDraggable(piece)) return;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var pos = new Vector2(i, j);
                var move = piece.TryCreatingMove(pos, Board);
                if (piece.Position != pos && move is not null)
                {
                    GetSquare(pos).Highlight(move.IsCapture ? BoardSquare.HighlightType.Capture : BoardSquare.HighlightType.Move);
                }
            }
        }
    }
    internal void UpdateIcons()
    {
        foreach (var child in Children)
        {
            if (child is PieceIcon icon) icon.Update(IsDraggable(icon.Piece));
        }
    }
    protected void UpdateSquares()
    {
        BoardSquares.ForEach((square) => square.UnHighlight());
        if (Board.IsInCheck) GetSquare(Board.GetKing(Board.WhiteToMove).Position).Highlight(BoardSquare.HighlightType.Check);
        if (Board.MoveCount > 0)
        {
            Move lastMove = Board.LastMove;
            GetSquare(lastMove.Start).Highlight(BoardSquare.HighlightType.LastMove);
            GetSquare(lastMove.End).Highlight(BoardSquare.HighlightType.LastMove);
        }
    }
    private BoardSquare GetSquare(Vector2 pos) => Children.Where((Entity child) => { return (child as BoardSquare).Coordinates == pos; }).First() as BoardSquare;
    private List<BoardSquare> BoardSquares => Children.Where((Entity child) => child is BoardSquare).Select((Entity child) => child as BoardSquare).ToList();
    protected bool IsDraggable(Piece piece) => Board.WhiteToMove == piece.IsWhite && piece.IsWhite == IsWhitePlayer;

    internal void PromotePawn(Type type)
    {
        var piece = Board.PromotePawn(type);
        AddChild(new PieceIcon(piece)
        {
            OnMouseReleased = (entity) =>
            {
                PieceMovedByMouse(piece);
                UpdateIcons();
                UpdateSquares();
            },
            OnMouseDown = (entity) => PiecePickedUp(piece)
        });
        UpdateIcons();
        UpdateSquares();
    }
}