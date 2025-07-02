using Chess.Backend.Core;
using Chess.Backend.Engine;
using System.Numerics;
using System.Reflection;

namespace Chess.Backend.Util
{
    internal static class PieceFactory
    {
        internal static Piece CreatePiece(PieceData data)
        {
            Type pieceType = Type.GetType($"Chess.Backend.Engine.Pieces.{data.Type}", throwOnError: true);

            ConstructorInfo constructor = pieceType.GetConstructor([typeof(Vector2), typeof(bool)]) ?? throw new ArgumentException($"No matching constructor found for type: {data.Type}");

            return (Piece)constructor.Invoke([new Vector2(data.Position[0], data.Position[1]), data.Color == "white"]);
        }
        internal static Piece PromotePiece(Piece piece, Type type)
        {
            ConstructorInfo constructor = type.GetConstructor([typeof(Vector2), typeof(bool)]) ?? throw new ArgumentException($"No matching constructor found for type: {type}");

            return (Piece)constructor.Invoke([piece.Position, piece.IsWhite]);
        }
    }
}
