using Maika.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using WebSockets;
using System.Threading;
using System.Net.WebSockets;

namespace Maika
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
		public Dictionary<string, List<string>> drawHistory;

		private MaikaSocket()
		{
			Users = new List<SocketUser>();
			ts = new CancellationTokenSource();
			drawHistory = new Dictionary<string, List<string>>();
		}

		public static async void OnConnected(object sender, WebSockets.WebSocket e)
		{
			var u = new SocketUser
			{
				Session = e
			};
			//Socket.Users.Add(u);
			bool open = true;
			
			while(open)
			{
				try
				{
					if (e.State == WebSocketState.Closed || e.State == WebSocketState.CloseReceived)
					{
						OnSessionClosed(u);
						open = false;
						break;
					}
					var m = await e.ReceiveTextAsync(Socket.ts.Token);
					OnMessageRecieved(u, m.Content);
				}catch
				{
					OnSessionClosed(u);
					open = false;
					break;
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
					Console.WriteLine($"User Join: {uM.Username} | {uM.Id}");
					user.UserId = uM.Id;
					user.Username = uM.Username;
					user.RoomId = uM.RoomId;
					if (!Socket.drawHistory.ContainsKey(uM.RoomId))
						Socket.drawHistory.Add(uM.RoomId, new List<string>());
					//Send exsisting Drawing
					Socket.drawHistory[uM.RoomId].ForEach(d =>
					{
						SendMessage(user.Session, new SocketMessage
						{
							Type = MessageType.Draw,
							Message = d
						});
					});
					//Send Join Message
					Socket.Users.ForEach(u =>
					{
						if(u.RoomId == uM.RoomId)
							SendMessage(u.Session, new SocketMessage
							{
								Type = MessageType.Join,
								Message = m.Message
							});
					});
					//Send User List
					Socket.Users.Add(user);
					SendMessage(user.Session, new SocketMessage
					{
						Type = MessageType.JoinInfo,
						Message = JsonConvert.SerializeObject(from SocketUser u in Socket.Users where u.RoomId == user.RoomId select new UserModel { Id = u.UserId, Username = u.Username })
					});
					break;
				default:
					Socket.Users.ForEach(u =>
					{
						if (u.RoomId != user.RoomId)
							return;
						if (u.UserId != user.UserId)
							SendMessage(u.Session, message);
					}); 
					if (m.Type == MessageType.Draw)
						Socket.drawHistory[user.RoomId].Add(m.Message);
					break;
			}
		}

		public static void OnSessionClosed(SocketUser user)
		{
			Console.WriteLine($"User Leave: {user.Username} | {user.UserId}");
			Socket.Users.Remove(user);
			Socket.Users.ForEach(u =>
			{
				if (u.RoomId != user.RoomId)
					return;
				SendMessage(u.Session, new SocketMessage
				{
					User = null,
					Type = MessageType.Leave,
					Message = JsonConvert.SerializeObject(new UserModel
					{
						Id = user.UserId,
						Username = user.Username
					})
				});
			});
			var keys = Socket.drawHistory.Keys.ToArray();
			foreach (string room in keys)
			{
				if(Socket.Users.All(u => u.RoomId != room))
				{
					Socket.drawHistory.Remove(room);
				}
			}
		}

		public static void SendMessage(WebSockets.WebSocket socket, SocketMessage message) => SendMessage(socket, message.ToString());

		public static async void SendMessage(WebSockets.WebSocket socket, string message)
		{
			try
			{
				if (socket.State != WebSocketState.Open)
					return;
				await socket.SendTextAsync(message, true, Socket.ts.Token);
			}catch(Exception e)
			{
				Console.WriteLine(e.Message);
			}
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
