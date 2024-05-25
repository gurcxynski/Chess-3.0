using Chess.UI;
using Chess.Util;
using Chess.Engine;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;

namespace Chess.Core;
internal class ChessGame : Panel {
    private readonly Board board = new();

    public ChessGame() : base(new Vector2(600, 600)) {
        Image background = new(Textures.Get("brown"));
        AddChild(background);
        board.Pieces.ForEach(piece => AddChild(new PieceIcon(piece)));
    }
    
}
