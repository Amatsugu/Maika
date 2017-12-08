using ITEC305_Project.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using WebSockets;
using System.Threading;
using System.Net.WebSockets;

namespace ITEC305_Project
{
	public class MaikaSocket
	{
		private static readonly object semaphore = new object();
		private static MaikaSocket Socket
		{
			get
			{
				lock (semaphore)
				{
					return instance ?? (instance = new MaikaSocket());
				}
			}
		}
		private static MaikaSocket instance;
		private CancellationTokenSource ts;
		public List<SocketUser> Users;
		public List<string> draw;

		private MaikaSocket()
		{
			Users = new List<SocketUser>();
			ts = new CancellationTokenSource();
			draw = new List<string>();
		}

		public static async void OnConnected(object sender, WebSockets.WebSocket e)
		{
			var u = new SocketUser
			{
				Session = e
			};
			//Socket.Users.Add(u);
			if (Socket.draw.Count > 0)
				Socket.draw.ForEach(m =>
				{
					SendMessage(e, new SocketMessage
					{
						Type = MessageType.Draw,
						Message = m
					}.ToString());
				});
			while(true)
			{
				try
				{
					if (e.State == WebSocketState.Closed || e.State == WebSocketState.CloseReceived)
					{
						OnSessionClosed(u);
						break;
					}
					var m = await e.ReceiveTextAsync(Socket.ts.Token);
					OnMessageRecieved(u, m.Content);
				}catch
				{
					OnSessionClosed(u);
				}
			}
		}

		public static void OnMessageRecieved(SocketUser user, string message)
		{
			//Console.WriteLine(message);
			var m = SocketMessage.FromJSON(message);
			switch (m.Type)
			{
				case MessageType.Join:
					var uM = JsonConvert.DeserializeObject<UserModel>(m.Message);
					user.UserId = uM.Id;
					user.Username = uM.Username;
					Socket.Users.ForEach(u => SendMessage(u.Session, new SocketMessage
					{
						Type = MessageType.Join,
						Message = m.Message 
					}.ToString()));
					Socket.Users.Add(user);
					break;
				default:
					Socket.Users.ForEach(u =>
					{
						if (u.UserId != user.UserId)
							SendMessage(u.Session, message);
					}); 
					if (m.Type == MessageType.Draw)
						Socket.draw.Add(m.Message);
					break;
			}
		}

		public static void OnSessionClosed(SocketUser user)
		{
			Socket.Users.Remove(user);
			Socket.Users.ForEach(u => SendMessage(u.Session, new SocketMessage
			{
				User = null,
				Type = MessageType.Leave,
				Message = user.UserId //TODO: Finalize Message Contents
			}.ToString()));
		}

		public static async void SendMessage(WebSockets.WebSocket socket, string message)
		{
			await socket.SendTextAsync(message, true, Socket.ts.Token);
		}

		public static void SendCloseMessage(string roomId) => Socket.Users.ForEach(u =>
		{
			if (u.RoomId == roomId)
				SendMessage(u.Session, new SocketMessage
				{
					Type = MessageType.RoomClose
				}.ToString());
		});
	}
}
