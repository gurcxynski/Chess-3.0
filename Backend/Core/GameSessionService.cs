using System;
using System.Collections.Generic;

namespace Backend.Core
{
	public class GameSessionService
	{
		private readonly Dictionary<string, GameSession> _activeSessions = new();
		private static readonly object _lock = new object();

		public string CreateBotGameSession(bool playerIsWhite)
		{
			lock (_lock)
			{
				var sessionId = Guid.NewGuid().ToString();
				var session = new GameSession
				{
					SessionId = sessionId,
					GameType = GameType.BotGame,
					PlayerIsWhite = playerIsWhite,
					CreatedAt = DateTime.UtcNow,
					IsValid = true
				};
				
				_activeSessions[sessionId] = session;
				return sessionId;
			}
		}

		public string CreateLocalGameSession()
		{
			lock (_lock)
			{
				var sessionId = Guid.NewGuid().ToString();
				var session = new GameSession
				{
					SessionId = sessionId,
					GameType = GameType.LocalGame,
					CreatedAt = DateTime.UtcNow,
					IsValid = true
				};
				
				_activeSessions[sessionId] = session;
				return sessionId;
			}
		}

		public GameSession? GetSession(string sessionId)
		{
			lock (_lock)
			{
				if (_activeSessions.TryGetValue(sessionId, out var session))
				{
					// Check if session is still valid (e.g., not expired)
					if (session.IsValid && DateTime.UtcNow - session.CreatedAt < TimeSpan.FromHours(2))
					{
						return session;
					}
					else
					{
						// Remove expired session
						_activeSessions.Remove(sessionId);
					}
				}
				return null;
			}
		}

		public void InvalidateSession(string sessionId)
		{
			lock (_lock)
			{
				if (_activeSessions.TryGetValue(sessionId, out var session))
				{
					session.IsValid = false;
				}
			}
		}

		public void RemoveSession(string sessionId)
		{
			lock (_lock)
			{
				_activeSessions.Remove(sessionId);
			}
		}
	}

	public class GameSession
	{
		public string SessionId { get; set; } = string.Empty;
		public GameType GameType { get; set; }
		public bool PlayerIsWhite { get; set; } = true;
		public DateTime CreatedAt { get; set; }
		public bool IsValid { get; set; }
	}

	public enum GameType
	{
		LocalGame,
		BotGame,
		OnlineGame
	}
}
