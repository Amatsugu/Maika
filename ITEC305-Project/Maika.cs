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

		internal static string ValidateUser(UserCredentialsModel login) =>
			RunCommand( cmd =>
			{
				cmd.CommandText = $"SELECT password, user_id FROM users WHERE email = '{Uri.EscapeDataString(login.Email)}'";
				using (var reader = cmd.ExecuteReader())
				{
					reader.Read();
					if (Utils.VerifyPassword(login.Password, reader.GetString(0)))
						return Authenticator.Authenticate(new UserPrincipal(reader.GetString(1), login.Username));
					else
						return null;
				}
			});

		internal static UserModel CreateUser(UserCredentialsModel user) =>
			RunCommand(cmd =>
			{
				var id = Authenticator.GenerateToken();
				cmd.CommandText = $"INSERT INTO users (user_id, username, password, email) VALUES ('{id}', '{Uri.EscapeDataString(user.Username)}', '{Utils.HashPassword(user.Password)}', '{Uri.EscapeDataString(user.Email)}');";
				cmd.ExecuteNonQuery();
				return new UserModel
				{
					Username = user.Username,
					Id = id
				};
			});

		internal static UserModel GetUser(string userId) =>
			RunCommand(cmd =>
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

		internal static bool CheckEmailExists(string email) =>
			RunCommand(cmd =>
			{
				cmd.CommandText = $"COUNT (SELECT username FROM users WHERE email = '{email}')";
				return ((int)cmd.ExecuteScalar()) > 0;
			});

		internal static string CreateInvite() //TODO: Create Invite
		{
			throw new NotImplementedException();
		}

		internal static bool SetUsername(string userid, UserCredentialsModel user) =>
			RunCommand(cmd =>
			{
				cmd.CommandText = $"UPDATE users SET username = '{Uri.EscapeDataString(user.Username)}' WHERE user_id = '{userid}'";
				return cmd.ExecuteNonQuery() > 0;
			});

		internal static RoomModel CreateRoom(string ownerId) //TODO: Create Room
		{
			throw new NotImplementedException();
		}

		internal static RoomModel GetRoomInfo(string roomId) =>
			RunCommand(cmd =>
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

		internal static bool JoinRoom(string roomID, string userId) => 
			RunCommand(cmd =>
			{
				cmd.CommandText = $"INSERT INTO room_member (room_id, user_id) VALUES ({roomID}, {userId})";
				return cmd.ExecuteNonQuery() > 0;
			});

		internal static bool LeaveRoom(string userId) =>
			RunCommand(cmd =>
			{
				cmd.CommandText = $"DELETE FROM room_member WHERE user_id = '{userId}'";
				return cmd.ExecuteNonQuery() > 0;
			});

		internal static List<UserModel> GetRoomMembers(string roomId) =>
			RunCommand(cmd =>
			{
				cmd.CommandText = $"SELECT u.user_id, u.username FROM user u LEFT JOIN room_member rm ON u.user_id = rm.user_id WHERE room_id = '{roomId }'";
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

		internal static bool SetRoomName(string roomId, string userId, string roomName) =>
			RunCommand(cmd =>
			{
				cmd.CommandText = $"UPDATE room SET title = '{Uri.EscapeDataString(roomName)}' WHERE room_id = '{roomId}' AND owner_id = '{userId}'";
				return cmd.ExecuteNonQuery() > 0;
			});

		internal static bool CloseRoom(string roomId, string userId) =>
			RunCommand(cmd =>
			{
				cmd.CommandText = $"DELETE FROM room WHERE room_id = '{roomId}' AND owner_id = '{userId}';";
				return cmd.ExecuteNonQuery() > 0;
			});

		internal static bool SetOwner(string roomId, string userId, string newOwnerId) =>
			RunCommand(cmd =>
			{
				cmd.CommandText = $"UPDATE room SET user_id = '{newOwnerId}' WHERE room_id = '{roomId}' AND user_id = '{userId}'";
				return cmd.ExecuteNonQuery() > 0;
			});

		internal static bool DeleteInvite(string inviteId) =>
			RunCommand(cmd =>
			{
				cmd.CommandText = $"DELETE FROM invite WHERE invite_id = '{inviteId}'";
				return cmd.ExecuteNonQuery() > 0;
			});
	}
}
