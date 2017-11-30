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
	class ITEC305Project //TODO: Implement Database transactions
	{
		public const string HOST = "itec305.luminousvector.com";

		internal static StatelessAuthenticationConfiguration StatelessConfig { get; private set; } = new StatelessAuthenticationConfiguration(nancyContext =>
		{
			try
			{
				var ApiKey = nancyContext.Request.Cookies.First(c => c.Key == "Token").Value;
				Console.WriteLine($"Login Token: {ApiKey}");
				return Authenticator.GetUser(ApiKey);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return null;
			}
		});

		private static NpgsqlConnection GetConnection()
		{
			var conn = new NpgsqlConnection(Credentials.ConntectionString);
			conn.Open();
			return conn;
		}

		internal static string ValidateUser(LoginCredentialsModel login) //TODO: Validate User
		{
			Authenticator.Authenticate(new UserIdenity(Authenticator.GenerateToken(), login.Username));
		}

		internal static void CreateUser(LoginCredentialsModel user)
		{
			throw new NotImplementedException();
		}

		internal static dynamic GetUserInfo(string id)
		{
			throw new NotImplementedException();
		}

		internal static bool CheckEmailExists(string email)
		{
			throw new NotImplementedException();
		}

		internal static object SetUsername(string userid, string username)
		{
			throw new NotImplementedException();
		}

		internal static object GetRoomInfo(string roomId)
		{
			throw new NotImplementedException();
		}

		internal static object GetRoomMembers(string roomId)
		{
			throw new NotImplementedException();
		}

		internal static object SetRoomName(string roomId, string roomName)
		{
			throw new NotImplementedException();
		}

		internal static object CloseRoom(string roomId)
		{
			throw new NotImplementedException();
		}

		internal static object SetOwner(string roomId, string userId)
		{
			throw new NotImplementedException();
		}

		internal static object DeleteInvite(string inviteId)
		{
			throw new NotImplementedException();
		}
	}
}
