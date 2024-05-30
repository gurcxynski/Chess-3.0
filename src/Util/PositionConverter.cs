using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;

namespace Chess.Util;
internal static class PositionConverter {
    internal static Vector2 ToGrid(Vector2 vec, Entity parent, bool drawWhiteDown) {
        var rel = vec - parent.GetActualDestRect().Location.ToVector2() - parent.Padding;
        rel /= (parent.Size - 2 * parent.Padding) / 8;
        rel.Floor();
        if (drawWhiteDown) rel = new Vector2(rel.X, 7 - rel.Y);
        return rel;
    }
    
    internal static Vector2 ToOffset(Vector2 vec, Entity parent, bool drawWhiteDown) {
        if (drawWhiteDown) vec = new Vector2(vec.X, 7 - vec.Y);
        return vec * ((parent.Size - 2 * parent.Padding) / 8);
    }
}