using System;
using System.Collections.Generic;
using Chess.Engine;
using Chess.Util;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;

namespace Chess.UI;
internal class PlayableArea : Panel {
    private readonly Board board = new();
    internal static PlayableArea Instance;
    internal bool WhiteTurn => board.WhiteToMove;
    bool drawWhiteDown = true;
    internal PlayableArea() : base(new Vector2(600, 600)) {
        Padding = new Vector2(20, 20);
        Instance = this;

        var background = new Image(Textures.Get("brown"), anchor: Anchor.Center)
        {
            PriorityBonus = -100,
        };

       AddChild(background);
        
        board.Pieces.ForEach((piece) => {
            var icon = new PieceIcon(piece)
            {
                OnMouseReleased = (Entity entity) =>
                {
                    Moved(entity as PieceIcon);
                    foreach (var child in Children) {
                        if (child is PieceIcon icon) icon.Update(drawWhiteDown);
                    }
                    RemoveAllChildren((Entity child) => child is ColorField && (child as ColorField).Type == ColorField.HighlightType.Move);
                },
                OnMouseDown = (Entity entity) =>
                {
                    OnPickup(entity as PieceIcon);
                }
            };
            AddChild(icon); 
            icon.Update(drawWhiteDown);
        });
        Chess.keyboardListener.KeyPressed += (sender, args) => {
            if (args.Key == Microsoft.Xna.Framework.Input.Keys.Z) {
                board.UndoMove();
                foreach (var child in Children) {
                    if (child is PieceIcon icon) icon.Update(drawWhiteDown);
                }
            }
        };
    }
    internal void Moved (PieceIcon icon) {
        var piece = icon.Piece;
        var clicked = PositionConverter.ToGrid(MouseInput.MousePosition, this, drawWhiteDown);
        if (clicked.X < 0 || clicked.X > 7 || clicked.Y < 0 || clicked.Y > 7 || clicked == piece.Position) return;
        Move move = piece.CreateMove(clicked, board);
        if (move is not null) {
            board.ExecuteMove(move);

        }
        if (board.IsInCheck) {
            AddChild(new ColorField(Color.Red, board.GetKing(WhiteTurn).Position, this, drawWhiteDown, ColorField.HighlightType.Check));
        } else {
            RemoveAllChildren((Entity child) => child is ColorField && (child as ColorField).Type == ColorField.HighlightType.Check);
        }
    }
    void OnPickup(PieceIcon piece)
    {
        var selected = piece.Piece;
        if (selected.IsWhite != WhiteTurn) return;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var pos = new Vector2(i, j);
                if (selected.Position != pos && selected.CreateMove(pos, board) is not null)
                {
                    AddChild(new ColorField(Color.Blue, pos, this, drawWhiteDown, ColorField.HighlightType.Move));
                }
            }
        }
        piece.BringToFront();
    }
    void RemoveAllChildren(Predicate<Entity> predicate)
    {
        List<Entity> toRemove = new();
        foreach (var child in Children) if (predicate(child)) toRemove.Add(child);
        foreach (var child in toRemove) RemoveChild(child);
    }
}