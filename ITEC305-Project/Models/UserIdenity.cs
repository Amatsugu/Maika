using System;
using System.Collections.Generic;
using System.Text;
using Nancy;
using Nancy.Security;
using System.Security.Claims;

namespace ITEC305_Project.Models
{
    public class UserIdenity : ClaimsPrincipal
    {
		public ulong Id { get; set; }
		public string UserName { get; set; }


		public UserIdenity(ulong id, string username)
		{
			Id = id;
			UserName = username;
		}
	}
}
