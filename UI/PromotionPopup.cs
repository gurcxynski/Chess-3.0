using Chess.Engine;
using Chess.Engine.Pieces;
using Chess.Util;
using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Chess.UI;

internal class PromotionPopup : Panel
{
    public EventHandler<Type> OnPromotionCompleted;

    class PromotionIcon<T> : Image where T : Piece
    {
        internal PromotionIcon(bool white)
        {
            Texture = GetTexture<T>(white);
            Anchor = Anchor.AutoCenter;
            Size = new(0f, 0.25f);
            OnClick = (entity) =>
            {
                Parent?.Parent?.RemoveChild(Parent);
                (Parent as PromotionPopup).OnPromotionCompleted?.Invoke(this, typeof(T));
            };
        }
    }

    internal PromotionPopup(Vector2 square, bool white)
    {
        Anchor = Anchor.TopLeft;
        Offset = PositionConverter.ToOffset(square);
        Size = new(0.2f, 0.8f);
        AddChild(new PromotionIcon<Queen>(white));
        AddChild(new PromotionIcon<Rook>(white));
        AddChild(new PromotionIcon<Knight>(white));
        AddChild(new PromotionIcon<Bishop>(white));
    }

    static Texture2D GetTexture<T>(bool white) where T : Piece
    {
        return Resources.Instance.LoadTexture("pieces\\" + MoveHelper.TypeToString<T>(white));
    }

}
