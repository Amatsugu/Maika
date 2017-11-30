using System;
using System.Collections.Generic;
using System.Text;
using Nancy;
using Nancy.Security;
using Nancy.Authentication.Stateless;
using ITEC305_Project.Models;

namespace ITEC305_Project.Modules
{
    public class RoomModule : NancyModule
    {
		public RoomModule() : base("/r")
		{
			StatelessAuthentication.Enable(this, Maika.StatelessConfig);
			this.RequiresAuthentication();
			Get("/{roomId}", args =>
			{
				var room = Maika.GetRoomInfo((string)args.roomId);
				if (room != null)
				{
					if (room.IsPublic)
					{
						room.Join(Context.CurrentUser as UserPrincipal);
						return View["room", room];
					}else
						return new Response { StatusCode = HttpStatusCode.Checkpoint }; //TODO: Invite Required message
				}
				else
					return new Response { StatusCode = HttpStatusCode.NotFound };
			});
		}
    }
}
