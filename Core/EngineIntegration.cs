using Chess.Core.Engine;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core;
class EngineIntegration(bool isWhite, int elo) : IMoveReceiver
{
    string moves;
    private Process process;
    public event EventHandler<(byte[], int)> OnMoveDataReceived;
    public event EventHandler<string> OnConnectionEstablished;
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
        Send("uci");
        while (!(ReadResponse() == "uciok")) { }
        Send("ucinewgame");
        Send("setoption name Skill Level value " + Elo);
        Send("isready");
        while (!(ReadResponse() == "readyok")) { }
    }

    private string ReadResponse()
    {
        if (process is null || process.HasExited) return string.Empty;

        return process.StandardOutput.ReadLine();
    }

    public void Stop() => process?.Kill();

    public async void Listen()
    {
        var history = $"position startpos moves {moves}";
        Send(history);
        Send("go movetime 1000");

        string response;
        while ((response = await Task.Run(ReadResponse)) != null)
        {
            if (response.StartsWith("bestmove"))
            {
                var move = response.Split(" ")[1];
                moves += move + " ";
                OnMoveDataReceived?.Invoke(this, (Encoding.UTF8.GetBytes(move), response.Length));
                break;
            }
        }
    }
    public void Send(string data)
    {
        Debug.WriteLine($"Sending data: {data}");
        if (process != null && !process.HasExited)
        {
            process.StandardInput.WriteLine(data);
            process.StandardInput.Flush();
        }
    }
    public void ProcessMove(Move move)
    {
        moves += move + " ";
        Debug.WriteLine($"Move processed: {move}");
        Listen();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}