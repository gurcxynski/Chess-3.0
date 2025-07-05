using System.Diagnostics;
using System.Text;

namespace Backend.Core;

class EngineIntegration(string path) : Interfaces.IStatefulOpponent
{
    public enum OptionType
    {
        Check,
        Spin,
        Combo,
        Button,
        String
    }
    public readonly struct Option(string name, OptionType type, string def, string min, string max, IReadOnlyList<string> values)
    {
        public readonly string Name = name;
        public readonly OptionType Type = type;
        public readonly string Default = def;
        public readonly string Min = min;
        public readonly string Max = max;
        public readonly IReadOnlyList<string> Values = values;
    }
    string moves = string.Empty;
    private readonly Process process = new()
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
    public event EventHandler<(byte[], int)> OnMoveDataReceived = delegate { };
    public event EventHandler<string> OnConnectionEstablished = delegate { };
    public event EventHandler OnMessageSent = delegate { };
    public void Start()
    {
        process.Start();
        Send("uci");
        while (!(ReadResponse() == "uciok")) { }
        Send("ucinewgame");
        Send("isready");
        while (!(ReadResponse() == "readyok")) { }
    }

    private string ReadResponse()
    {
        if (process is null || process.HasExited) return string.Empty;
        string response = process.StandardOutput.ReadLine() ?? string.Empty;
        Debug.WriteLine($"Received data: {response}");
        return response;
    }

    public void Stop() => process.Kill();

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
                OnMoveDataReceived.Invoke(this, (Encoding.UTF8.GetBytes(move), move.Length));
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
    public void ProcessMove(Engine.Move move)
    {
        moves += move + " ";
        Debug.WriteLine($"Move processed: {move}");
        Listen();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public void SetOptions(IEnumerable<KeyValuePair<string, string>> options)
    {
        foreach (var (name, value) in options)
            Send($"setoption name {name} value {value}");
    }
    public void PushOptionButton(string name) => Send($"setoption name {name}");

    public List<Option> GetOptions()
    {
        process.Start();

        List<Option> options = [];

        Send("uci");
        string response = ReadResponse();
        while (!response.StartsWith("uciok"))
        {
            if (!response.StartsWith("option")) { response = ReadResponse(); continue; }
            var split = response.Split(" ");
            string name = string.Empty;
            int i = 2;
            while (split[i] != "type")
            {
                name += split[i++] + " ";
            }
            name = name.Trim();
            var type = split[i + 1];
            string def = string.Empty;
            string min = string.Empty;;
            string max = string.Empty;;
            List<string> values = [];
            for (; i < split.Length; i++) {
                var value = split[i];
                switch (value)
                {
                    case "default": def = split[++i]; break;
                    case "min": min = split[++i]; break;
                    case "max": max = split[++i]; break;
                    case "var": values.Add(split[++i]); break;
                }
            }
            OptionType typeEnum = type switch
            {
                "check" => OptionType.Check,
                "spin" => OptionType.Spin,
                "combo" => OptionType.Combo,
                "button" => OptionType.Button,
                "string" => OptionType.String,
                _ => throw new NotImplementedException(),
            };
            Option option = new(name, typeEnum, def, min, max, values);
            options.Add(option);
            response = ReadResponse();
        }
        process.Kill();
        return options;
    }
}