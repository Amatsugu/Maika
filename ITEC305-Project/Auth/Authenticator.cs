using Maika.Extensions;
using Maika.Models;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Claims;
using System.Text;

namespace Maika.Auth
{
    public class Authenticator
    {
		private static readonly object semaphore = new object();
		private static Authenticator Auth
		{
			get
			{
				lock(semaphore)
				{
					return _instance ?? (_instance = new Authenticator());
				}
			}
		}
		private static Authenticator _instance;

		private Authenticator()
		{
			_activeSessions = new Dictionary<string, UserPrincipal>();
		}

		private Dictionary<string, UserPrincipal> _activeSessions;

		public static UserPrincipal GetUserIdenity(string token) => (Auth._activeSessions.ContainsKey(token)) ? Auth._activeSessions[token] : null;

		public static string Authenticate(UserPrincipal user)
		{
			var token = GenerateToken();
			Auth._activeSessions.Add(token, user);
			return token;
		}

		public static string GenerateToken() => Guid.NewGuid().ToBase62();
	}
}
