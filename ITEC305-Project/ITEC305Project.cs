using Nancy.Authentication.Stateless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Claims;
using ITEC305_Project.Models;
using ITEC305_Project.Auth;

namespace ITEC305_Project
{
	class ITEC305Project //TODO: Implement Database transactions
	{
		internal static StatelessAuthenticationConfiguration StatelessConfig { get; private set; } = new StatelessAuthenticationConfiguration(nancyContext =>
		{
			try
			{
				ulong ApiKey = ulong.Parse(nancyContext.Request.Cookies.First(c => c.Key == "Token").Value);
				Console.WriteLine($"Login Token: {ApiKey}");
				return Authenticator.GetUser(ApiKey);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return null;
			}
		});

		internal static string ValidateUser(LoginCredentialsModel login)
		{
			throw new NotImplementedException();
		}

		internal static void CreateUser(LoginCredentialsModel user)
		{
			throw new NotImplementedException();
		}

		internal static dynamic GetUserInfo(ulong id)
		{
			throw new NotImplementedException();
		}

		internal static bool CheckEmailExists(string email)
		{
			throw new NotImplementedException();
		}

		internal static object SetUsername(ulong userid, string username)
		{
			throw new NotImplementedException();
		}

		internal static object GetRoomInfo(ulong roomId)
		{
			throw new NotImplementedException();
		}

		internal static object GetRoomMembers(ulong roomId)
		{
			throw new NotImplementedException();
		}

		internal static object SetRoomName(ulong roomId, string roomName)
		{
			throw new NotImplementedException();
		}

		internal static object CloseRoom(ulong roomId)
		{
			throw new NotImplementedException();
		}

		internal static object SetOwner(ulong roomId, ulong userId)
		{
			throw new NotImplementedException();
		}

		internal static object DeleteInvite(ulong inviteId)
		{
			throw new NotImplementedException();
		}
	}
}
