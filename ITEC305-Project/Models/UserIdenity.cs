using System;
using System.Collections.Generic;
using System.Text;
using Nancy;
using Nancy.Security;
using System.Security.Claims;
using System.Security.Principal;

namespace Maika.Models
{
    public class UserPrincipal : ClaimsPrincipal
    {
		public string Id { get; set; }
		public string RoomId => MaikaCore.GetRoomMembership(Id);

		public UserPrincipal(string id, string username) : base(new GenericIdentity(username, "stateless"))
		{
			Id = id;
		}
	}
}
