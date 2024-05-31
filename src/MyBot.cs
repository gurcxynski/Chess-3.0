using System.Collections.Generic;
using Chess.Engine;
using Chess.Engine.Pieces;
using System;
using System.Linq;
namespace Chess;
public class MyBot
{
    public MyBot(bool white) {
        playingWhite = white;
    }
    public bool playingWhite;

    readonly int[][] centerValues = new int[][] {
        new int[] { 000, 000, 000, 000, 000, 000, 000, 000 },
        new int[] { 000, 033, 033, 033, 033, 033, 033, 000 },
        new int[] { 000, 033, 100, 100, 100, 100, 033, 000 },
        new int[] { 000, 033, 100, 200, 200, 100, 033, 000 },
        new int[] { 000, 033, 100, 200, 200, 100, 033, 000 },
        new int[] { 000, 033, 100, 100, 100, 100, 033, 000 },
        new int[] { 000, 033, 033, 033, 033, 033, 033, 000 },
        new int[] { 000, 000, 000, 000, 000, 000, 000, 000 }
    };

    static int GetValue(Type type) {
        return type switch {
            Type t when t == typeof(Pawn) => 100,
            Type t when t == typeof(Knight) => 300,
            Type t when t == typeof(Bishop) => 350,
            Type t when t == typeof(Rook) => 500,
            Type t when t == typeof(Queen) => 900,
            Type t when t == typeof(King) => int.MaxValue,
            _ => 0
        };
    }
    int Eval(Board board, Move move, int depth = 0) {
        int val = 0;
        var myPieces = playingWhite ? board.WhitePieces : board.BlackPieces;
        var pieceValueSum = myPieces.Sum((piece) => { if (piece.GetType() != typeof (King)) return GetValue(piece.GetType()); return 0;});
        if (move.IsCapture) val += GetValue(move.CapturePieceType) * (40 / pieceValueSum);
        if (move.MovePieceType != typeof(King) && move.MovePieceType != typeof(Rook)) 
        val += centerValues[(int)move.End.Y][(int)move.End.X] - 
            centerValues[(int)move.Start.Y][(int)move.Start.X];
        board.ExecuteMove(move);
        if (board.IsInCheck) val += 50;
        if (board.MoveCount < 30) {
            if (move.IsCastles) val += 90;
            else { 
                //if (move.IsFirstMoveOfPiece && move.MovePieceType == typeof(King)) val -= 80;
                //if (board.HasKingsideCastleRight(playingWhite)) val += 50;
                //if (board.HasQueensideCastleRight(playingWhite)) val += 30; 
            }
        }
        if (depth == 0) { board.UndoMove(); return val; }
        List<Move> moves = board.GetValidMoves();
        int max = int.MinValue;
        foreach (var m in moves) {
            int v = Eval(board, m, depth - 1);
            if (v > max) max = v;
        }
        val += (int)(-0.95 * max);
        board.UndoMove();
        return val;
    }
    internal Move Think(Board board)
    {
        playingWhite = board.WhiteToMove;
        int max = int.MinValue;
        List<Move> moves = board.GetValidMoves();
        Move bestMove = moves[0];
        foreach (var move in moves) {
            int val = Eval(board, move);
            if (val > max) {
                max = val;
                bestMove = move;
            }
        }
        return bestMove;
    }
}