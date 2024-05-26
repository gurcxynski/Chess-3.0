using Chess.Util;
using Chess.Engine;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using System;

namespace Chess.UI;
internal class ChessGame : Panel {
    private readonly Board board = new();
    internal static ChessGame Instance;
    internal bool WhiteTurn => board.WhiteToMove;
    bool drawWhiteDown = true;
    internal ChessGame() : base(new Vector2(800, 800)) {
        Instance = this;
    
        Padding = new Vector2(20, 20);

        var background = new Image(Textures.Get("brown"), anchor: Anchor.Center);
        AddChild(background);
        
        board.Pieces.ForEach((piece) => {
            var icon = new PieceIcon(piece)
            {
                OnStopDrag = (Entity entity) =>
                {
                    Moved(piece);
                }
            };
            AddChild(icon); 
            icon.Update(drawWhiteDown);
        });
    }
    internal void Moved (Piece piece) {
        Vector2 clicked = ToGrid(MouseInput.MousePosition);
        Move move = piece.CreateMove(clicked, board);
        if (move is not null) {
            board.ExecuteMove(move);
        }
        foreach (var child in Children) {
            if (child is PieceIcon icon) {
                icon.Update(drawWhiteDown);
            }
        }
    }
    private Vector2 ToGrid(Vector2 vec) {
        var rel = vec - GetActualDestRect().Location.ToVector2() - Padding;
        rel /= (Size - 2 * Padding) / 8;
        rel.Floor();
        if (drawWhiteDown) rel = new Vector2(rel.X, 7 - rel.Y);
        return rel;
    }
}
