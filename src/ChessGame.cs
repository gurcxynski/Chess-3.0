using System;
using System.Collections.Generic;
using Chess.Engine;
using Chess.UI;
using Chess.Util;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;

namespace Chess;
internal abstract class ChessGame : Panel {
    internal enum GameType {
        Hotseat,
        Bot,
        Online
    }
    internal GameType Type { get; private init; } 
    internal static ChessGame Instance { get; private set; }
    protected readonly Board board = new();
    protected bool drawBlackDown;
    private Vector2 baseSize;
    internal float SizeFactor;
    internal float PaddingFactor;

    internal ChessGame(Vector2 bSize, GameType type) : base() {
        baseSize = bSize;
        Anchor = Anchor.CenterLeft;
        Instance = this;
        Type = type;

        var background = new Image(Textures.Get("brown"), anchor: Anchor.Center) { PriorityBonus = -100 };
        AddChild(background);
        
        board.Pieces.ForEach((piece) => {
            var icon = new PieceIcon(piece)
            {
                OnMouseReleased = (Entity entity) => {
                    PieceMovedByMouse(entity as PieceIcon);
                    UpdateIcons();
                    UpdateSquares();
                },
                OnMouseDown = (Entity entity) => PiecePickedUp(entity as PieceIcon)
            };
            AddChild(icon); 
        });

    }
    internal void Init() {
        Size = baseSize * SizeFactor;
        Padding = Size * PaddingFactor;
        UpdateIcons();
    }
    protected virtual void PieceMovedByMouse (PieceIcon icon) {
        var piece = icon.Piece;
        if (piece.IsWhite != board.WhiteToMove) return;

        var clicked = PositionConverter.ToGrid(MouseInput.MousePosition, this, drawBlackDown);
        if (clicked.X < 0 || clicked.X > 7 || clicked.Y < 0 || clicked.Y > 7 || clicked == piece.Position) return;

        Move move = piece.CreateMove(clicked, board);
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
                if (selected.Position != pos && selected.CreateMove(pos, board) is not null)
                {
                    AddChild(new ColorField(Color.Blue, pos, this, drawBlackDown, ColorField.HighlightType.Move));
                }
            }
        }
        icon.BringToFront();
    }
    protected void ProcessMove(Move move) {
        board.ExecuteMove(move);
        UpdateIcons();
        UpdateSquares();
    }
    void RemoveAllChildren(Predicate<Entity> predicate)
    {
        List<Entity> toRemove = new();
        foreach (var child in Children) if (predicate(child)) toRemove.Add(child);
        foreach (var child in toRemove) RemoveChild(child);
    }
    internal void UpdateIcons() {
        foreach (var child in Children) {
                if (child is PieceIcon icon) icon.Update(drawBlackDown, IsDraggable(icon));
            }
    }
    protected void UpdateSquares() {
        if (board.IsInCheck) {
            AddChild(new ColorField(Color.Red, board.GetKing(board.WhiteToMove).Position, this, drawBlackDown, ColorField.HighlightType.Check));
        } else {
            RemoveAllChildren((Entity child) => child is ColorField && (child as ColorField).Type == ColorField.HighlightType.Check);
        }
        RemoveAllChildren((Entity child) => child is ColorField && (child as ColorField).Type == ColorField.HighlightType.Move);
    }
    
    protected abstract bool IsDraggable(PieceIcon icon);
}