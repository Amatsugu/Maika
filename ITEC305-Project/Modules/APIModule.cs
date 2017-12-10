using System;
using System.Collections.Generic;
using System.Text;
using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Security;
using Nancy.ModelBinding;
using Maika.Models;

namespace Maika.Modules
{
    public class APIModule : NancyModule
    {
		public APIModule() : base("/api")
		{
			StatelessAuthentication.Enable(this, MaikaCore.StatelessConfig);
			this.RequiresAuthentication();
			//User
			Get("/user/{userId}", args => Response.AsJson(MaikaCore.GetUser((string)args.userId)));
			Post("/user/", args =>
			{
				if (MaikaCore.SetUsername((Context.CurrentUser as UserPrincipal).Id, this.Bind<UserCredentialsModel>()))
					return new Response { StatusCode = HttpStatusCode.OK };
				else
					return new Response { StatusCode = HttpStatusCode.NotAcceptable };
			});
			Get("/user/", args => Response.AsJson(MaikaCore.GetUser((Context.CurrentUser as UserPrincipal).Id)));

			//Room
			Get("/room", args => Response.AsJson(MaikaCore.CreateRoom(Context.CurrentUser as UserPrincipal)));
			Get("/room/{roomId}", args => Response.AsJson(MaikaCore.GetRoomInfo((string)args.roomId)));
			Get("/room/{roomId}/members", args => Response.AsJson(MaikaCore.GetRoomMembers((string)args.roomId)));
			Post("/room/{roomId}/name", args =>
			{
				if(MaikaCore.SetRoomName((string)args.roomId, (Context.CurrentUser as UserPrincipal).Id, this.Bind<RoomModel>().Name))
					return new Response { StatusCode = HttpStatusCode.OK };
				else
					return new Response { StatusCode = HttpStatusCode.NotAcceptable };
			});
			Delete("/room/{roomId}", args =>
			{
				if(MaikaCore.CloseRoom((string)args.roomId, (Context.CurrentUser as UserPrincipal).Id))
					return new Response { StatusCode = HttpStatusCode.OK };
				else
					return new Response { StatusCode = HttpStatusCode.NotAcceptable };
			});
			Post("/room/{roomId}", args =>
			{
				if(MaikaCore.SetOwner((string)args.roomId, (Context.CurrentUser as UserPrincipal).Id, this.Bind<UserModel>().Username))
					return new Response { StatusCode = HttpStatusCode.OK };
				else
					return new Response { StatusCode = HttpStatusCode.NotAcceptable };
			});

			//Invites
			Post("/invite", args =>
			{
				var invite = MaikaCore.CreateInvite(this.Bind<RoomModel>("Name", "Owner", "Members", "IsPublic").Id, Context.CurrentUser as UserPrincipal);
				if (invite == null)
					return new Response().WithStatusCode(HttpStatusCode.Unauthorized);
				else
					return Response.AsJson(invite);

			});
			Delete("/invite/{inviteId}", args =>
			{
				if (MaikaCore.DeleteInvite((string)args.inviteId))
					return new Response { StatusCode = HttpStatusCode.OK };
				else
					return new Response { StatusCode = HttpStatusCode.NotAcceptable };
			});


		}
	}
}
