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
	public static class ITEC305Project //TODO: Implement Database transactions
	{
		public const string HOST = "itec305.luminousvector.com";
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
			throw new NotImplementedException();
		}

		internal static UserModel GetUserInfo(string id)
		{
			throw new NotImplementedException();
		}

		internal static bool CheckEmailExists(string email)
		{
			throw new NotImplementedException();
		}

		internal static bool SetUsername(string userid, UserCredentialsModel username)
		{
			throw new NotImplementedException();
		}

		internal static RoomModel GetRoomInfo(string roomId)
		{
			throw new NotImplementedException();
		}

		internal static UserModel[] GetRoomMembers(string roomId)
		{
			throw new NotImplementedException();
		}

		internal static bool SetRoomName(string roomId, string userId, string roomName)
		{
			throw new NotImplementedException();
		}

		internal static bool CloseRoom(string roomId, string userId)
		{
			throw new NotImplementedException();
		}

		internal static bool SetOwner(string roomId, string userId, string newOwnerId)
		{
			throw new NotImplementedException();
		}

		internal static bool DeleteInvite(string inviteId)
		{
			throw new NotImplementedException();
		}

		internal static string CreateInvite()
		{
			throw new NotImplementedException();
		}
	}
}
