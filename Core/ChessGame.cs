using Chess.Core.Engine;
using Chess.Core.UI;
using Chess.Core.UI.Graphics;
using Chess.Core.Util;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Chess.Core;
internal class ChessGame : Panel
{
    internal static ChessGame Instance { get; private set; }
    protected readonly Board board = new(PositionLoader.LoadBoardSetup("boardSetup.json"));
    internal Vector2 BoardSize => Size - 2 * Padding;
    internal ChessGame()
    {
        Instance = this;
        Padding = Vector2.One * 10;
    }
    internal void Init()
    {
        for (int i = 0; i < 8; i++) for (int j = 0; j < 8; j++) AddChild(new BoardSquare((i + j) % 2 == 0));
        board.Pieces.ForEach((piece) =>
        {
            var icon = new PieceIcon(piece)
            {
                OnMouseReleased = (entity) =>
                {
                    System.Diagnostics.Debug.WriteLine("Mouse released at: " + piece + " " + piece.Position + " " + MouseInput.MousePosition);
                    PieceMovedByMouse(piece);
                    UpdateIcons();
                    UpdateSquares();
                },
                OnMouseDown = (entity) => PiecePickedUp(piece)
            };
            AddChild(icon);
        });
        UpdateIcons();
    }
    protected virtual void PieceMovedByMouse(Piece piece)
    {
        if (piece.IsWhite != board.WhiteToMove) return;

        var clicked = PositionConverter.ToGrid(MouseInput.MousePosition);
        if (clicked.X < 0 || clicked.X > 7 || clicked.Y < 0 || clicked.Y > 7 || clicked == piece.Position) return;

        Move move = piece.TryCreatingMove(clicked, board);
        if (move is not null)
        {
            board.ExecuteMove(move);
            if (move.IsCapture) Chess.Instance.captureSound.Play();
            else Chess.Instance.moveSound.Play();
        }


    }
    protected virtual void PiecePickedUp(Piece piece)
    {
        if (piece.IsWhite != board.WhiteToMove) return;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var pos = new Vector2(i, j);
                var move = piece.TryCreatingMove(pos, board);
                if (piece.Position != pos && move is not null)
                {
                    AddChild(new ColorField(pos, move.IsCapture ? ColorField.HighlightType.Capture : ColorField.HighlightType.Move));
                }
            }
        }
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

    protected bool IsDraggable(PieceIcon icon) => board.WhiteToMove == icon.Piece.IsWhite;
}