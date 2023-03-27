using System;
using System.Text;
using CommunityCompiler.Interfaces;
using WebSocketSharp;

namespace CommunityCompiler.Services
{
	public class DataService : IDataService
	{
		public DataService()
		{
		}

		public WebSocket Sock { get; set; }

        public string Connect(string ip, int port)
		{
			var url = $"wss://{ip}:{port}";
			Console.WriteLine("Connecting to " + url);
			Sock = new WebSocket($"ws://{ip}:{port}");

			Sock.OnMessage += OnMessage;

			Sock.Connect();
			return "Connect completed";
		}

		public void OnMessage(object sender, MessageEventArgs e) 
		{
			Console.WriteLine("ReceivedMessage");
			Console.WriteLine(Encoding.UTF8.GetString(e.RawData));
		}

		public void send(String data)
		{
			Sock?.Send(data);
		}
	}
}

