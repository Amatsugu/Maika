using System;
using System.Net.WebSockets;
using Nancy;
using Nancy.Hosting.Self;
using SuperSocket.WebSocket;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase;

namespace ITEC305_Project
{
    class Program
    {
        static void Main(string[] args)
        {
			var uri = new Uri("http://localhost:4321");
			var host = new NancyHost(uri);
			host.Start();
			Console.WriteLine($"Hosting on {uri}...");
			var socket = new WebSocketServer();
			socket.Setup(new RootConfig(), new ServerConfig
			{
				Name = "MaikaSocket",
				Ip = "Any",
				Port = 4322,
				Mode = SocketMode.Tcp
			});
			socket.NewSessionConnected += MaikaSocket.OnConnected;
			socket.NewMessageReceived += MaikaSocket.OnMessageRecieved;
			socket.SessionClosed += MaikaSocket.OnSessionClosed;
			socket.Start();
			Console.ReadLine();
			host.Stop();
			socket.Stop();
		}
	}
}
