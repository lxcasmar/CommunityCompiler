using System;
using CommunityCompiler.Interfaces;
using WebSocketSharp;

namespace CommunityCompiler.Services
{
	public class DataService : IDataService
	{
		public DataService()
		{
		}

		public string Connect(string ip, int port)
		{
			Sock = new WebSocket($"ws://{ip}:{port}");

			Sock.OnMessage += ((sender, e) =>
			{
				Console.WriteLine("Received: " + e.Data);
			});

			//Sock.Connect();
			// having issues with this connect method?

			//Sock.Send("HELLO");
			return "completed";
		}

		public WebSocket Sock { get; set; }
	}
}

