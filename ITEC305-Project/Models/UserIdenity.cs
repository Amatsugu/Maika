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
		public string Id { get; set; }
		public string UserName { get; set; }


		public UserIdenity(string id, string username)
		{
			Id = id;
			UserName = username;
		}
	}
}
