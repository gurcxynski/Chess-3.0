using Chess.Core;
using Chess.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chess.Graphics;
internal class PieceDrawable : DrawableObject
{
    internal Piece Piece { get; private init; }
    private Vector2 parent;
    public PieceDrawable(Vector2 parent, Piece piece) 
        : base(new Rectangle((parent + Converter.GridToDraw(piece.Position)).ToPoint(), new Point(Game1.Size)),  
        Game1.self.textures.Get((piece.IsWhite ? "w" : "b") + piece.GetType().ToString().Split(".")[^1]), parent) { Piece = piece; this.parent = parent; }


    internal void UpdatePosition() => Bounds = new Rectangle((parent + Converter.GridToDraw(Piece.Position)).ToPoint(), new Point(Game1.Size));
}