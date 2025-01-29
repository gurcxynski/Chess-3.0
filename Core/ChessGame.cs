using Chess.Core.Engine;
using Chess.Core.UI;
using Chess.Core.UI.Graphics;
using Chess.Core.UI.Menus;
using Chess.Core.Util;
using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Chess.Core;
internal class ChessGame : Panel
{
    internal static ChessGame Instance { get; private set; }
    internal readonly Board Board = new(PositionLoader.LoadBoardSetup("boardSetup.json"));
    internal Vector2 BoardSize => Size - 2 * Padding;
    internal ChessGame()
    {
        Anchor = Anchor.AutoCenter;
        Instance = this;
        Padding = Vector2.One * 10;
        Chess.Bot.OnMoveCalculationFinished += (sender, move) => ExecuteMove(move);
        int smaller = System.Math.Min(UserInterface.Active.ScreenHeight, UserInterface.Active.ScreenWidth);
        Size = new Vector2(smaller, smaller) * 0.8f;
    }
    internal void Initialize()
    {
        for (int i = 0; i < 8; i++) for (int j = 0; j < 8; j++) AddChild(new BoardSquare((i + j) % 2 == 0));
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
        Chess.Bot.Start();
    }
    protected virtual void PieceMovedByMouse(Piece piece)
    {
        if (piece.IsWhite != Board.WhiteToMove) return;

        var clicked = PositionConverter.ToGrid(MouseInput.MousePosition);
        if (clicked.X < 0 || clicked.X > 7 || clicked.Y < 0 || clicked.Y > 7 || clicked == piece.Position) return;

        if (!ExecuteMove(piece.TryCreatingMove(clicked, Board))) return;
        //Chess.Bot.CalculateMoveAsync(Board.MoveHistory, 3000);
    }
    private bool ExecuteMove(Move move)
    {
        if (move == null) return false;
        Board.ExecuteMove(move);
        if (move.IsCapture) Chess.Instance.captureSound.Play();
        else Chess.Instance.moveSound.Play();
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
                    AddChild(new ColorField(pos, move.IsCapture ? ColorField.HighlightType.Capture : ColorField.HighlightType.Move));
                }
            }
        }
    }
    void RemoveAllChildren(System.Predicate<Entity> predicate)
    {
        List<Entity> toRemove = [];
        foreach (var child in Children) if (predicate(child)) toRemove.Add(child);
        foreach (var child in toRemove) RemoveChild(child);
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
        if (Board.IsInCheck)
        {
            AddChild(new ColorField(Board.GetKing(Board.WhiteToMove).Position, ColorField.HighlightType.Check));
        }
        else
        {
            RemoveAllChildren((child) => child is ColorField && (child as ColorField).Type == ColorField.HighlightType.Check);
        }
        RemoveAllChildren((child) => child is ColorField && (child as ColorField).Type != ColorField.HighlightType.Check);
    }

    protected bool IsDraggable(Piece piece) => Board.WhiteToMove == piece.IsWhite;
}