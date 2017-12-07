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

			Get("/", args =>
			{
				var user = Context.CurrentUser as UserPrincipal;
				if(user.RoomId == null)
				{
					var room = Maika.CreateRoom(user);
					room.Join(user);
					return Response.AsRedirect($"/r/{room.Id}");
				}
				else
					return Response.AsRedirect($"/r/{user.RoomId}");
			});

			Get("/{roomId}", args =>
			{
				var room = Maika.GetRoomInfo((string)args.roomId);
				if (room != null)
				{
					var user = Context.CurrentUser as UserPrincipal;
					if (room.IsMember(user))
						return View["room", room];
					if (room.IsPublic)
					{
						room.Join(Context.CurrentUser as UserPrincipal);
						return View["room", room];
					}else
						return new Response { StatusCode = HttpStatusCode.Checkpoint }; 
				}
				else
					return new Response { StatusCode = HttpStatusCode.NotFound };
			});
		}
    }
}
