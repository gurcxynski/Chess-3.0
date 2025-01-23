using Chess.Core.Engine;
using Chess.Core.UI;
using Chess.Core.UI.Menus;
using Chess.Core.Util;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Core;
internal abstract class ChessGame : Panel
{
    internal static ChessGame Instance { get; private set; }
    protected readonly Board board = new(PositionLoader.LoadBoardSetup("boardSetup.json"));

    internal ChessGame(Vector2 size) : base()
    {
        Size = size;
        Anchor = Anchor.CenterLeft;
        Instance = this;
    }
    internal void Init()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var pos = new Vector2(i, j);
                AddChild(new BoardSquare(pos));
            }
        }
        board.Pieces.ForEach((piece) =>
        {
            var icon = new PieceIcon(piece)
            {
                OnMouseReleased = (entity) =>
                {
                    PieceMovedByMouse(entity as PieceIcon);
                    UpdateIcons();
                    UpdateSquares();
                },
                OnMouseDown = (entity) => PiecePickedUp(entity as PieceIcon)
            };
            AddChild(icon);
        });
        UpdateIcons();
    }
    protected virtual void PieceMovedByMouse(PieceIcon icon)
    {
        var piece = icon.Piece;
        if (piece.IsWhite != board.WhiteToMove) return;

        var clicked = PositionConverter.ToGrid(MouseInput.MousePosition);
        if (clicked.X < 0 || clicked.X > 7 || clicked.Y < 0 || clicked.Y > 7 || clicked == piece.Position) return;

        Move move = piece.TryCreatingMove(clicked, board);
        if (move is not null) board.ExecuteMove(move);

    }
    protected virtual void PiecePickedUp(PieceIcon icon)
    {
        var selected = icon.Piece;
        if (selected.IsWhite != board.WhiteToMove) return;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var pos = new Vector2(i, j);
                var move = selected.TryCreatingMove(pos, board);
                if (selected.Position != pos && move is not null)
                {
                    AddChild(new ColorField(pos, move.IsCapture ? ColorField.HighlightType.Capture : ColorField.HighlightType.Move));
                }
            }
        }
        icon.BringToFront();
    }
    protected void ProcessMove(Move move)
    {
        board.ExecuteMove(move);
        UpdateIcons();
        UpdateSquares();
    }
    void RemoveAllChildren(Predicate<Entity> predicate)
    {
        List<Entity> toRemove = [];
        foreach (var child in Children) if (predicate(child)) toRemove.Add(child);
        foreach (var child in toRemove) RemoveChild(child);
    }
    internal void UpdateIcons()
    {
        foreach (var child in Children)
        {
            if (child is PieceIcon icon) icon.Update(IsDraggable(icon));
        }
    }
    protected void UpdateSquares()
    {
        if (board.IsInCheck)
        {
            AddChild(new ColorField(board.GetKing(board.WhiteToMove).Position, ColorField.HighlightType.Check));
        }
        else
        {
            RemoveAllChildren((child) => child is ColorField && (child as ColorField).Type == ColorField.HighlightType.Check);
        }
        RemoveAllChildren((child) => child is ColorField && (child as ColorField).Type != ColorField.HighlightType.Check);
    }

    protected abstract bool IsDraggable(PieceIcon icon);
}