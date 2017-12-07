using Nancy.Authentication.Stateless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Claims;
using ITEC305_Project.Models;
using ITEC305_Project.Auth;
using Npgsql;

namespace ITEC305_Project
{
	public static class Maika
	{
		public const string HOST = "maika.luminousvector.com";
		private static readonly DBCredentials dBCredentials = DBCredentials.FromJSON("DB_Credentials.json");

		internal static StatelessAuthenticationConfiguration StatelessConfig { get; private set; } = new StatelessAuthenticationConfiguration(nancyContext =>
		{
			try
			{
				var ApiKey = nancyContext.Request.Cookies.First(c => c.Key == "Token").Value;
				Console.WriteLine($"Login Token: {ApiKey}");
				var i = Authenticator.GetUserIdenity(ApiKey);
				return i;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return null;
			}
		});

		private static T RunCommand<T>(Func<NpgsqlCommand, T> func)
		{
			using (var con = new NpgsqlConnection(dBCredentials.ConntectionString))
			{
				con.Open();
				using (var cmd = con.CreateCommand())
				{
					return func(cmd);
				}
			}
		}

		internal static string ValidateUser(UserCredentialsModel login) => RunCommand(cmd =>
		{
			cmd.CommandText = $"SELECT password, user_id, username FROM users WHERE email = '{Uri.EscapeDataString(login.Email)}'";
			using (var reader = cmd.ExecuteReader())
			{
				reader.Read();
				if (Utils.VerifyPassword(login.Password, reader.GetString(0)))
					return Authenticator.Authenticate(new UserPrincipal(reader.GetString(1), reader.GetString(2)));
				else
					return null;
			}
		});

		internal static UserModel CreateUser(UserCredentialsModel user) => RunCommand(cmd =>
		{
			if (CheckEmailExists(user.Email))
				return null;
			var id = Authenticator.GenerateToken();
			cmd.CommandText = $"INSERT INTO users (user_id, username, password, email) VALUES ('{id}', '{Uri.EscapeDataString(user.Username)}', '{Utils.HashPassword(user.Password)}', '{Uri.EscapeDataString(user.Email)}');";
			cmd.ExecuteNonQuery();
			return new UserModel
			{
				Username = user.Username,
				Id = id
			};
		});

		internal static UserModel GetUser(string userId) => RunCommand(cmd =>
		{
			cmd.CommandText = $"SELECT username FROM users WHERE user_id = '{userId}'";
			using (var reader = cmd.ExecuteReader())
			{
				if (!reader.HasRows)
					return null;
				reader.Read();
				return new UserModel
				{
					Id = userId,
					Username = Uri.UnescapeDataString(reader.GetString(0))
				};
			}
		});

		internal static bool CheckEmailExists(string email) => RunCommand(cmd =>
		{
			cmd.CommandText = $"SELECT COUNT(email) FROM users WHERE email = '{Uri.EscapeDataString(email)}'";
			return ((long)cmd.ExecuteScalar()) > 0;
		});

		internal static bool SetUsername(string userid, UserCredentialsModel user) => RunCommand(cmd =>
		{
			cmd.CommandText = $"UPDATE users SET username = '{Uri.EscapeDataString(user.Username)}' WHERE user_id = '{userid}'";
			return cmd.ExecuteNonQuery() > 0;
		});

		internal static string GetRoomMembership(string userId) => RunCommand(cmd =>
		{
			cmd.CommandText = $"SELECT room_id FROM room_member WHERE user_id = '{userId}'";
			return cmd.ExecuteScalar() as string;
		});

		internal static RoomModel CreateRoom(UserPrincipal owner) => RunCommand(cmd =>
		{
			if (owner.RoomId != null)
				LeaveRoom(owner);
			var id = Authenticator.GenerateToken();
			cmd.CommandText = $"INSERT INTO room (room_id, owner_id, title) VALUES('{id}', '{owner.Id}', 'New Room');";
			cmd.CommandText += $"INSERT INTO room_member VALUES('{id}',  '{owner.Id}');";
			cmd.ExecuteNonQuery();
			var u = GetUser(owner.Id);
			return new RoomModel
			{
				Id = id,
				Owner = u,
				IsPublic = false,
				Members = new List<UserModel>() { u },
				Name = "New Room"
			};
		});

		internal static RoomModel GetRoomInfo(string roomId) => RunCommand(cmd =>
		{
			cmd.CommandText = $"SELECT title, owner_id FROM room WHERE room_id = '{roomId}'";
			using (var reader = cmd.ExecuteReader())
			{
				if (!reader.HasRows)
					return null;
				reader.Read();
				return new RoomModel
				{
					Id = roomId,
					Name = Uri.UnescapeDataString(reader.GetString(0)),
					Owner = GetUser(reader.GetString(1)),
					Members = GetRoomMembers(roomId)
				};
			}
		});

		internal static bool JoinRoom(UserPrincipal user, string roomID) => RunCommand(cmd =>
		{
			if (user.RoomId != null)
				LeaveRoom(user);
			cmd.CommandText = $"INSERT INTO room_member VALUES('{roomID}', '{user.Id}')";
			return cmd.ExecuteNonQuery() > 0;
		});

		internal static bool LeaveRoom(UserPrincipal user) => RunCommand(cmd =>
		{
			cmd.CommandText = $"DELETE FROM room_member WHERE user_id = '{user.Id}'";
			CleanRoom(user.RoomId);
			return cmd.ExecuteNonQuery() > 0;
		});

		internal static List<UserModel> GetRoomMembers(string roomId) => RunCommand(cmd =>
		{
			cmd.CommandText = $"SELECT users.user_id, users.username FROM users, room_member WHERE users.user_id = room_member.user_id AND room_member.room_id = '{roomId}'";
			using (var reader = cmd.ExecuteReader())
			{
				if (!reader.HasRows)
					return null;
				List<UserModel> users = new List<UserModel>();
				while (reader.Read())
				{
					users.Add(new UserModel
					{
						Id = reader.GetString(0),
						Username = Uri.UnescapeDataString(reader.GetString(1))
					});
				}
				return users;
			}
		});

		internal static void CleanRoom(string roomId) => RunCommand(cmd =>
		{
			var room = GetRoomInfo(roomId);
			if (room.Members.Any(u => u.Id == room.Owner.Id))
				return 0;
			if (room.Members.Count == 0)
				cmd.CommandText = $"DELETE FROM room WHERE room_id = '{roomId}'";
			else
				cmd.CommandText = $"UPDATE room SET owner_id = '{room.Members.First().Id}'";
			return cmd.ExecuteNonQuery();
		});

		internal static bool SetRoomName(string roomId, string userId, string roomName) => RunCommand(cmd =>
		{
			cmd.CommandText = $"UPDATE room SET title = '{Uri.EscapeDataString(roomName)}' WHERE room_id = '{roomId}' AND owner_id = '{userId}'";
			return cmd.ExecuteNonQuery() > 0;
		});

		internal static bool CloseRoom(string roomId, string userId) => RunCommand(cmd =>
		{
			cmd.CommandText = $"DELETE FROM room WHERE room_id = '{roomId}' AND owner_id = '{userId}';";
			return cmd.ExecuteNonQuery() > 0;
		});

		internal static bool SetOwner(string roomId, string userId, string newOwnerId) => RunCommand(cmd =>
		{
			cmd.CommandText = $"UPDATE room SET owner_id = '{newOwnerId}' WHERE room_id = '{roomId}' AND owner_id = '{userId}'";
			return cmd.ExecuteNonQuery() > 0;
		});

		internal static InviteModel CreateInvite(string roomId) => RunCommand(cmd =>
		{
			var invite = Authenticator.GenerateToken();
			cmd.CommandText = $"INSERT INTO invite VALUES ('{invite}', '{roomId}')";
			cmd.ExecuteNonQuery();
			return new InviteModel
			{
				Id = invite,
				RoomId = roomId
			};
		});

		internal static InviteModel GetInvite(string inviteId) => RunCommand(cmd =>
		{
			cmd.CommandText = $"SELECT room_id FROM invite WHERE invite_id = '{inviteId}'";
			try
			{
				return new InviteModel
				{
					Id = inviteId,
					RoomId = cmd.ExecuteScalar() as string
				};
			}catch
			{
				return null;
			}
		});

		internal static bool AcceptInvite(UserPrincipal user, InviteModel invite) => JoinRoom(user, invite.RoomId);
			

		internal static bool DeleteInvite(string inviteId) => RunCommand(cmd =>
		{
			cmd.CommandText = $"DELETE FROM invite WHERE invite_id = '{inviteId}'";
			return cmd.ExecuteNonQuery() > 0;
		});
	}
}
