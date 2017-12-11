using System;
using System.Net.WebSockets;
using Nancy;
using Nancy.Hosting.Self;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using WebSockets;
using System.IO;
using Newtonsoft.Json;

namespace Maika
{
	class Program
	{
		static void Main(string[] args)
		{
			//var ts = new CancellationTokenSource();
			//var ct = ts.Token;
			var nancy = new Task(() =>
			{
				Console.WriteLine("Starting Host");
				try
				{
					var uri = new Uri("http://localhost:5321");
					var host = new NancyHost(uri);
					host.Start();
					Console.WriteLine($"Hosting on {uri}...");
				}catch(Exception e)
				{
					//Console.WriteLine(e.Message);
					Console.WriteLine(e.Message);
				}
				while (true)
					Thread.Sleep(1000);
			});
			var webSocket = new Task(() =>
			{
				Console.WriteLine("Starting WebSocket Server");
				var socket = new WebSocketServer();
				socket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4322));
				//socket.ClientKeepAliveInterval = new TimeSpan(1, 0, 0);
				socket.StartAccept();
				socket.Connected += MaikaSocket.OnConnected;
				Console.WriteLine("WebSocket Server Started on port 4322");
				while (true)
					Thread.Sleep(1000);
			});
			nancy.Start();
			webSocket.Start();
			Console.ReadLine();
		}
	}
}