using Microsoft.Xna.Framework;

namespace Chess.Util;
internal static class Converter {
    internal static Vector2 GridToDraw(Vector2 org) => Game1.Size * new Vector2(org.X, 7 - org.Y);
}