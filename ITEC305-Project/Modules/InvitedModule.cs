using System;
using System.Collections.Generic;
using System.Text;
using Nancy;
using Nancy.Security;
using Nancy.Authentication.Stateless;
using Maika.Models;

namespace Maika.Modules
{
    public class InvitedModule : NancyModule
    {
		public InvitedModule()
		{
			StatelessAuthentication.Enable(this, MaikaCore.StatelessConfig);
			this.RequiresAuthentication();
			Get("/i/{inviteId}", args =>
			{
				var invite = MaikaCore.GetInvite((string)args.inviteId);
				if (invite != null)
					return View["invite", MaikaCore.GetRoomInfo(invite.RoomId)];
				else
					return Response.AsRedirect("/error/404");
			});

			Post("/i/{inviteId}", args =>
			{
				var invite = MaikaCore.GetInvite((string)args.inviteId);
				if (invite != null)
				{
					MaikaCore.AcceptInvite(Context.CurrentUser as UserPrincipal, invite);
					return Response.AsRedirect($"/r/{invite.RoomId}");
				}
				else
					return new Response().WithStatusCode(HttpStatusCode.NotFound);
			});
		}
    }
}
