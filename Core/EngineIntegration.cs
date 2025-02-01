using Chess.Core.Engine;
using Chess.Core.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace Chess.Core;
class EngineIntegration(bool isWhite, int elo) : IMoveReceiver
{
    Stack<Move> moves = new();
    private Process process;
    public event EventHandler<(byte[], int)> OnMoveDataReceived;
    public event EventHandler OnConnectionEstablished;
    public event EventHandler OnMessageSent;
    public bool EnginePlaysWhite { get; } = isWhite;
    public int Elo { get; } = elo;
    public void Start()
    {
        process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "ChessEngines\\stockfish\\stockfish.exe",
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
        SendCommand("setoption name Skill Level value " + Elo);
        SendCommand("isready");
        while (!(ReadResponse() == "readyok")) { }
    }

    private string ReadResponse()
    {
        if (process != null && !process.HasExited)
        {
            var response = process.StandardOutput.ReadLine();
            Debug.WriteLine("Data received: " + response);
            return response;
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

    public async void Listen()
    {
        var history = "position startpos moves";
        foreach (var move in moves) history += " " + move.ToString();
        SendCommand(history);
        SendCommand("go movetime 1000");

        string response;
        while ((response = await Task.Run(ReadResponse)) != null)
        {
            if (response.StartsWith("bestmove"))
            {
                OnMoveDataReceived?.Invoke(this, (System.Text.Encoding.UTF8.GetBytes(response.Split(" ")[1]), response.Length));
                break;
            }
        }
    }
    private void SendCommand(string command)
    {
        byte[] data = System.Text.Encoding.UTF8.GetBytes(command);
        Send(data);
        Debug.WriteLine("Data sent: " + command);
    }
    public void Send(byte[] data)
    {
        string command = System.Text.Encoding.UTF8.GetString(data);
        if (process != null && !process.HasExited)
        {
            process.StandardInput.WriteLine(command);
            process.StandardInput.Flush();
        }
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}