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
	public static class Maika //TODO: Implement Database transactions
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

		internal static RoomModel CreateRoom(string ownerId)
		{
			throw new NotImplementedException();
		}

		private static NpgsqlConnection GetConnection()
		{
			var conn = new NpgsqlConnection(dBCredentials.ConntectionString);
			conn.Open();
			return conn;
		}

		internal static string ValidateUser(UserCredentialsModel login) //TODO: Validate User
		{
			return Authenticator.Authenticate(new UserPrincipal(Authenticator.GenerateToken(), login.Username));
		}

		internal static UserModel CreateUser(UserCredentialsModel user)
		{
			using (var con = GetConnection())
			{
				using (var cmd = con.CreateCommand())
				{
					var id = Authenticator.GenerateToken();
					cmd.CommandText = $"INSERT INTO project.user (user_id, username, password, email) VALUES ('{id}', '{user.Username}', '{Utils.HashPassword(user.Password)}', '{user.Email}');";
					cmd.ExecuteNonQuery();
					return new UserModel
					{
						Username = user.Username,
						Id = id
					};
				}
			}
		}

		internal static UserModel GetUserInfo(string userId)
		{
			using (var con = GetConnection())
			{
				using (var cmd = con.CreateCommand())
				{
					cmd.CommandText = $"SELECT username FROM project.user WHERE user_id = '{userId}'";
					return new UserModel
					{
						Id = userId,
						Username = cmd.ExecuteScalar() as string
					};
				}
			}
		}

		internal static bool CheckEmailExists(string email)
		{
			using (var con = GetConnection())
			{
				using (var cmd = con.CreateCommand())
				{
					cmd.CommandText = $"COUNT (SELECT username FROM project.user WHERE email = '{email}')";
					return ((int)cmd.ExecuteScalar()) > 0;
				}
			}
		}

		internal static string CreateInvite()
		{
			throw new NotImplementedException();
		}

		internal static bool SetUsername(string userid, UserCredentialsModel username)
		{
			using (var con = GetConnection())
			{
				using (var cmd = con.CreateCommand())
				{
					cmd.CommandText = $"UPDATE project.user SET username = '{username}' WHERE user_id = '{userid}'";
					return cmd.ExecuteNonQuery() > 0;
				}
			}
		}

		internal static RoomModel GetRoomInfo(string roomId)
		{
			using (var con = GetConnection())
			{
				using (var cmd = con.CreateCommand())
				{
					cmd.CommandText = $"SELECT title, owner_id FROM project.room WHERE room_id = '{roomId}'";
					using (var reader = cmd.ExecuteReader())
					{
						reader.Read();
						return new RoomModel
						{
							Id = roomId,
							Name = reader.GetString(0),
							Owner = GetUserInfo(reader.GetString(1)),
							Members = GetRoomMembers(roomId)
						};
					}
				}
			}
		}

		internal static List<UserModel> GetRoomMembers(string roomId)
		{
			using (var con = GetConnection())
			{
				using (var cmd = con.CreateCommand())
				{
					cmd.CommandText = $"SELECT u.user_id, u.username FROM project.user u LEFT JOIN project.room_member rm ON u.user_id = rm.user_id WHERE room_id = '{roomId }'";
					using (var reader = cmd.ExecuteReader())
					{
						List<UserModel> users = new List<UserModel>();
						while (reader.Read())
						{
							users.Add(new UserModel
							{
								Id = reader.GetString(0),
								Username = reader.GetString(1)
							});
						}
						return users;
					}
				}
			}
		}

		internal static bool SetRoomName(string roomId, string userId, string roomName)
		{
			using (var con = GetConnection())
			{
				using (var cmd = con.CreateCommand())
				{
					cmd.CommandText = $"UPDATE project.room SET title = '{roomName}' WHERE room_id = '{roomId}' AND owner_id = '{userId}'";
					return cmd.ExecuteNonQuery() > 0;
				}
			}
		}

		internal static bool CloseRoom(string roomId, string userId)
		{
			using (var con = GetConnection())
			{
				using (var cmd = con.CreateCommand())
				{
					cmd.CommandText = $"DELETE FROM project.room WHERE room_id = '{roomId}' AND owner_id = '{userId}';";
					cmd.CommandText += $"DELETE FROM project.room_member WHERE room_id = '{roomId}';";
					return cmd.ExecuteNonQuery() > 0;
				}
			}
		}

		internal static bool SetOwner(string roomId, string userId, string newOwnerId)
		{
			using (var con = GetConnection())
			{
				using (var cmd = con.CreateCommand())
				{
					cmd.CommandText = $"UPDATE project.room SET user_id = '{newOwnerId}' WHERE room_id = '{roomId}' AND user_id = '{userId}'";
					return cmd.ExecuteNonQuery() > 0;
				}
			}
		}

		internal static bool DeleteInvite(string inviteId)
		{
			using (var con = GetConnection())
			{
				using (var cmd = con.CreateCommand())
				{
					cmd.CommandText = $"DELETE FROM project.invite WHERE invite_id = '{inviteId}'";
					return cmd.ExecuteNonQuery() > 0;
				}
			}
		}
	}
}