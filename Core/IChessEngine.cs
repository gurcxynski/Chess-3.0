using Chess.Core.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{
    internal interface IChessEngine
    {
        public event EventHandler<Move> OnMoveCalculationFinished;
        public Task CalculateMoveAsync(IEnumerable<Move> moves, int time);
        public void Start(string path);
        public void Stop();
    }
}
