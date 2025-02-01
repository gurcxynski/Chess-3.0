using Chess.Core.Engine;
using Chess.Core.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Chess.Core;
class EngineIntegration
{
    private Process process;
    public event EventHandler<Move> OnMoveCalculationFinished;
    public void Start(string path)
    {
        process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = path,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        SendCommand("uci");
        while (!(ReadResponse() == "uciok")) { }
        SendCommand("ucinewgame");
        SendCommand("isready");
        while (!(ReadResponse() == "readyok")) { }
        ChessGame.Instance.OnPlayerMove += async (sender, args) => await CalculateMoveAsync(args.MoveHistory, args.Time);
    }

    private void SendCommand(string command)
    {
        if (process != null && !process.HasExited)
        {
            process.StandardInput.WriteLine(command);
            process.StandardInput.Flush();
        }
    }

    private string ReadResponse()
    {
        if (process != null && !process.HasExited)
        {
            return process.StandardOutput.ReadLine();
        }

        return string.Empty;
    }

    public void Stop()
    {
        if (process != null && !process.HasExited)
        {
            process.Kill();
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