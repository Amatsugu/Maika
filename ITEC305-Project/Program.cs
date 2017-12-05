using System;
using System.Net.WebSockets;
using Nancy;
using Nancy.Hosting.Self;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using WebSockets;
//using SuperSocket.WebSocket;\
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
				socket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4322));
				socket.StartAccept();
				socket.Connected += MaikaSocket.OnConnected;
				Console.WriteLine("WebSocket Server Started on port 4322");
				while (true)
					Thread.Sleep(1000);
			}, ts.Token);
			nancy.Start();
			webSocket.Start();
			Console.ReadLine();
			ts.Cancel();
		}

		private static void Socket_ConnectedAsync(object sender, WebSockets.WebSocket e)
		{
			var ts = new CancellationTokenSource();
			while(true)
				Console.WriteLine(e.ReceiveTextAsync(ts.Token).GetAwaiter().GetResult().Content);
			//throw new NotImplementedException();
		}
	}
}