using System;
using System.Net.WebSockets;
using Nancy;
using Nancy.Hosting.Self;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase;
using System.Threading.Tasks;
using System.Threading;
using SuperWebSocket;

namespace ITEC305_Project
{
	class Program
	{
		static void Main(string[] args)
		{
			var ts = new CancellationTokenSource();
			var ct = ts.Token;
			var nancy = new Task(() =>
			{
				Console.WriteLine("Starting Host");
				var uri = new Uri("http://localhost:4321");
				var host = new NancyHost(uri);
				host.Start();
				Console.WriteLine($"Hosting on {uri}...");
				while (true)
					Thread.Sleep(1000);
			}, ts.Token);
			var webSocket = new Task(() =>
			{
				Console.WriteLine("Starting WebSocket Server");
				var socket = new WebSocketServer();
				socket.Setup(new RootConfig(), new ServerConfig
				{
					Name = "MaikaSocket",
					Ip = "Any",
					Mode = SocketMode.Tcp,
					Port = 4322
				});
				socket.NewSessionConnected += MaikaSocket.OnConnected;
				socket.NewMessageReceived += MaikaSocket.OnMessageRecieved;
				socket.SessionClosed += MaikaSocket.OnSessionClosed;
				if (!socket.Start())
				{
					Console.WriteLine("Error");
					return;
				}
				Console.WriteLine("WebSocket Server Started on port 4322");
				while (true)
					Thread.Sleep(1000);
			}, ts.Token);
			nancy.Start();
			webSocket.Start();
			Console.ReadLine();
			ts.Cancel();
		}
	}
}