using System.Collections.Generic;
namespace Chess.Core;
public class MyBot
{
    bool playingWhite;
    readonly Dictionary<PieceType, int> pieceValues = new Dictionary<PieceType, int> {
    { PieceType.Pawn, 100 },
    { PieceType.Knight, 300 },
    { PieceType.Bishop, 350 },
    { PieceType.Rook, 500 },
    { PieceType.Queen, 900 },
    { PieceType.King, int.MaxValue } };

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

    int Eval(Board board, Move move, int depth = 2) {
        int val = 0;
        if (move.IsCapture) val += pieceValues[move.CapturePieceType];
        if (move.MovePieceType != PieceType.King && move.MovePieceType != PieceType.Rook) 
        val += centerValues[(int)move.End.Y][(int)move.End.X] - 
            centerValues[(int)move.Start.Y][(int)move.Start.X];
        board.ExecuteMove(move);
        if (board.IsInCheck()) val += 50;
        if (board.MoveCount < 30) {
            if (move.IsCastles) val += 90;
            else { if (board.HasKingsideCastleRight(playingWhite)) val += 50;
            if ( board.HasQueensideCastleRight(playingWhite)) val += 30; }
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