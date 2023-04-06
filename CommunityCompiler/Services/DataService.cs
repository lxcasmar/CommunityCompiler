using System;
using System.Runtime.ConstrainedExecution;
using System.Text;
using CommunityCompiler.Interfaces;
using WebSocketSharp;

namespace CommunityCompiler.Services
{
	/// <summary>
	/// The <c>DataService</c> class is how the client application will communicate
	/// with the server. The client is thin, so the majority of the workload related to
	/// data handling is done by the server. 
	/// </summary>
	public class DataService : IDataService
	{
		protected WebSocket Sock { get; set; }

		protected TaskCompletionSource<string> stcs { get; set; }

		protected TaskCompletionSource<bool> btcs { get; set; }

		protected readonly string _ParamDelimiter = "##";

		public DataService()
		{
			stcs = new TaskCompletionSource<string>();
			btcs = new TaskCompletionSource<bool>();
		}

		/// <summary>
		/// Connects to the specified address via web socket
		/// </summary>
		/// <param name="ip">IP address of the server</param>
		/// <param name="port">Port on which the server is running</param>
        public void Connect(string ip, int port)
		{
			var url = $"wss://{ip}:{port}";
			Console.WriteLine("Connecting to " + url);
			Sock = new WebSocket($"ws://{ip}:{port}");

			Sock.OnMessage += OnMessage;

			Sock.Connect();
			Console.WriteLine("Successfully Connected");
		}

		public virtual void OnMessage(object sender, MessageEventArgs e) 
		{
			Console.WriteLine("ReceivedMessage:");
			Console.WriteLine(Encoding.UTF8.GetString(e.RawData));
		}

		public void Send(String data)
		{
			if (!Connected())
			{
				Console.WriteLine($"[ERROR] Could not send {data}. Not conencted to server");
				return;
			}
			Sock?.Send(data);
		}

		public bool Connected()
		{
			return Sock.IsAlive;
		}
	}
}

