using ITEC305_Project.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace ITEC305_Project.Auth
{
    public class Authenticator
    {
		private static readonly object semphore = new object();
		private static Authenticator authenticator
		{
			get
			{
				lock(semphore)
				{
					if (_instance == null)
						return _instance = new Authenticator();
					else
						return _instance;
				}
			}
		}
		private static Authenticator _instance;

		private Authenticator()
		{
			_activeSessions = new Dictionary<string, UserIdenity>();
		}

		private Dictionary<string, UserIdenity> _activeSessions;

		public static ClaimsPrincipal GetUser(string token) => (authenticator._activeSessions.ContainsKey(token)) ? authenticator._activeSessions[token] : null;

		public static string Authenticate(UserIdenity user)
		{
			var token = GenerateToken();
			authenticator._activeSessions.Add(token, user);
			return token;
		}

		public static string GenerateToken() => Convert.ToBase64String(Guid.NewGuid().ToByteArray());
	}
}
