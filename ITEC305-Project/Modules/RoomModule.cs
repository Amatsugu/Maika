using System;
using System.Collections.Generic;
using System.Text;
using Nancy;
using Nancy.Security;
using Nancy.Authentication.Stateless;

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
					return View["room", room];
				else
					return new Response { StatusCode = HttpStatusCode.NotFound };
			});
		}
    }
}
