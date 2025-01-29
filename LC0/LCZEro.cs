using Chess.Core;
using Chess.Core.Engine;
using Chess.Core.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Chess.LC0;
class LcZeroIntegration : IChessEngine
{
    private Process lcProcess;
    public event EventHandler<Move> OnMoveCalculationFinished;
    public void Start()
    {
        lcProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "LC0\\lc0.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        lcProcess.Start();
    }

    private void SendCommand(string command)
    {
        if (lcProcess != null && !lcProcess.HasExited)
        {
            lcProcess.StandardInput.WriteLine(command);
            lcProcess.StandardInput.Flush();
        }
    }

    private string ReadResponse()
    {
        if (lcProcess != null && !lcProcess.HasExited)
        {
            return lcProcess.StandardOutput.ReadLine();
        }

        return string.Empty;
    }

    public void Stop()
    {
        if (lcProcess != null && !lcProcess.HasExited)
        {
            lcProcess.Kill();
        }
    }
    public async Task CalculateMoveAsync(IEnumerable<Move> moves, int time)
    {
        var history = "position startpos moves";
        foreach (var move in moves) history += " " + move.ToString();
        Debug.WriteLine(history);
        SendCommand(history);
        SendCommand("go movetime " + time);

        Move botMove = null;
        string response;
        while ((response = await Task.Run(() => ReadResponse())) != null)
        {
            if (response.StartsWith("bestmove"))
            {
                Debug.WriteLine(response);
                botMove = MoveHelper.TryCreatingMove(response);
                break;
            }
        }

        OnMoveCalculationFinished?.Invoke(this, botMove);
    }
}
