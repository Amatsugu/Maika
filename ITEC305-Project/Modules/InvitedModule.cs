using System;
using System.Collections.Generic;
using System.Text;
using Nancy;
using Nancy.Security;
using Nancy.Authentication.Stateless;
using ITEC305_Project.Models;

namespace ITEC305_Project.Modules
{
    public class InvitedModule : NancyModule
    {
		public InvitedModule()
		{
			StatelessAuthentication.Enable(this, Maika.StatelessConfig);
			this.RequiresAuthentication();
			Get("/invite/{inviteId}", args =>
			{
				var invite = Maika.GetInvite((string)args.inviteId);
				if (invite != null)
					return View["invite", Maika.GetRoomInfo(invite.RoomId)];
				else
					return Response.AsRedirect("/error/404");
			});

			Post("/invite/{inviteId}", args =>
			{
				var invite = Maika.GetInvite((string)args.inviteId);
				if (invite != null)
				{
					Maika.AcceptInvite(Context.CurrentUser as UserPrincipal, invite);
					return Response.AsRedirect($"/r/{invite.RoomId}");
				}
				else
					return new Response().WithStatusCode(HttpStatusCode.NotFound);
			});
		}
    }
}
