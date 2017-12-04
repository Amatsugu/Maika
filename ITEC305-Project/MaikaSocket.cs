using ITEC305_Project.Models;
using Newtonsoft.Json;
using SuperSocket.SocketBase;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using SuperWebSocket;

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

		public List<SocketUser> Users;

		private MaikaSocket()
		{
			Users = new List<SocketUser>();
		}

		public static void OnConnected(WebSocketSession session)
		{

		}

		public static void OnMessageRecieved(WebSocketSession session, string message)
		{
			var m = SocketMessage.FromJSON(message);
			switch (m.Type)
			{
				case MessageType.Join:
					m.User.Session = session;
					Socket.Users.ForEach(u => u.Session.Send(new SocketMessage
					{
						Type = MessageType.Join,
						Message = m.User.UserId //TODO: Finalize Message Contents
					}.ToString()));
					Socket.Users.Add(m.User);
					break;
				default:
					Socket.Users.ForEach(u => u.Session.Send(message));
					break;
			}
		}

		public static void OnSessionClosed(WebSocketSession session, CloseReason reason)
		{
			var user = Socket.Users.First(u => u.Session.SessionID == session.SessionID);
			Socket.Users.Remove(user);
			Socket.Users.ForEach(u => u.Session.Send(new SocketMessage
			{
				User = null,
				Type = MessageType.Leave,
				Message = user.UserId //TODO: Finalize Message Contents
			}.ToString()));
		}

		public static void SendCloseMessage(string roomId) => Socket.Users.ForEach(u =>
		{
			if (u.RoomId == roomId)
				u.Session.Send(new SocketMessage
				{
					Type = MessageType.RoomClose
				}.ToString());
		});
	}
}
