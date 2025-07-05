using System.Text;
using System.Text.Json;
using Backend.Interfaces;

namespace Backend.Core;

class ChessAPI() : IStatelessOpponent
{
	private readonly HttpClient httpClient = new();
	public event EventHandler<(byte[], int)> OnMoveDataReceived = delegate { };
	public event EventHandler<string> OnConnectionEstablished = delegate { };
	public event EventHandler OnMessageSent = delegate { };
	public void Start() { }
	public void Stop() {}
	public void Listen()  { }
	public async void SendFen(string fen)
	{
		string requestUrl = $"https://stockfish.online/api/s/v2.php?fen={Uri.EscapeDataString(fen)}&depth=10";
		var response = await httpClient.GetAsync(requestUrl);

		var responseContent = await response.Content.ReadAsStringAsync();
		if (string.IsNullOrEmpty(responseContent)) return;
		var jsonDocument = JsonDocument.Parse(responseContent);
		if (!jsonDocument.RootElement.TryGetProperty("bestmove", out var bestmove))
		{
			Console.WriteLine("Best move not found in response, fen: " + fen);
			return;
		}
		var moveString = bestmove.GetString()?.Split(' ')[1];
		var moveBytes = Encoding.UTF8.GetBytes(moveString ?? string.Empty);
		OnMoveDataReceived.Invoke(this, (moveBytes, moveBytes.Length));
	}
}