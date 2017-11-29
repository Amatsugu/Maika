using ITEC305_Project.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace ITEC305_Project.Auth
{
    public static class Authenticator
    {
		private static Dictionary<string, UserIdenity> _activeSessions;

		public static ClaimsPrincipal GetUser(string token) => (_activeSessions.ContainsKey(token)) ? _activeSessions[token] : null;

		public static string Authenticate(UserIdenity user)
		{
			var token = GenerateToken();
			_activeSessions.Add(token, user);
			return token;
		}

		public static string GenerateToken() => Convert.ToBase64String(Guid.NewGuid().ToByteArray());
	}
}
