using Chess.Core.Engine;
using Chess.Core.Engine.Pieces;
using Chess.Core.Util;
using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading.Tasks;

namespace Chess.Core.UI;

internal class PromotionPopup : Panel
{
    private TaskCompletionSource<Type> promotionTaskCompletionSource;

    class PromotionIcon<T> : Image where T : Piece
    {
        internal PromotionIcon(bool white)
        {
            Texture = GetTexture<T>(white);
            Anchor = Anchor.AutoCenter;
            Size = new(0.2f, 0.2f);
            OnClick = (entity) =>
            {
                ChessGame.Instance.PromotePawn(typeof(T));
                (Parent as PromotionPopup).PromotionCompleted(typeof(T));
                Parent?.Parent?.RemoveChild(Parent);
            };
        }
    }

    internal PromotionPopup(Vector2 square, bool white)
    {
        Anchor = Anchor.TopLeft;
        Offset = PositionConverter.ToOffset(square);
        promotionTaskCompletionSource = new TaskCompletionSource<Type>();

        AddChild(new PromotionIcon<Queen>(white));
        AddChild(new PromotionIcon<Rook>(white));
        AddChild(new PromotionIcon<Knight>(white));
        AddChild(new PromotionIcon<Bishop>(white));
    }

    static Texture2D GetTexture<T>(bool white) where T : Piece
    {
        return Resources.Instance.LoadTexture("pieces\\" + MoveHelper.TypeToString<T>(white));
    }

    internal async Task<Type> WaitForPromotion()
    {
        return await promotionTaskCompletionSource.Task;
    }

    private void PromotionCompleted(Type pieceType)
    {
        promotionTaskCompletionSource.SetResult(pieceType);
    }
}
