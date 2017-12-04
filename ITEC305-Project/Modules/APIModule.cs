using System;
using System.Collections.Generic;
using System.Text;
using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Security;
using Nancy.ModelBinding;
using ITEC305_Project.Models;

namespace ITEC305_Project.Modules
{
    public class APIModule : NancyModule
    {
		public APIModule() : base("/api")
		{
			StatelessAuthentication.Enable(this, Maika.StatelessConfig);
			this.RequiresAuthentication();
			//User
			Get("/user/{userId}", args => Response.AsJson(Maika.GetUser((string)args.userId)));
			Post("/user/", args =>
			{
				if (Maika.SetUsername((Context.CurrentUser as UserPrincipal).Id, this.Bind<UserCredentialsModel>()))
					return new Response { StatusCode = HttpStatusCode.OK };
				else
					return new Response { StatusCode = HttpStatusCode.NotAcceptable };
			});
			Get("/user/", args => Response.AsJson(Maika.GetUser((Context.CurrentUser as UserPrincipal).Id)));

			//Room
			Get("/room", args => Response.AsJson(Maika.CreateRoom(Context.CurrentUser as UserPrincipal)));
			Get("/room/{roomId}", args => Response.AsJson(Maika.GetRoomInfo((string)args.roomId)));
			Get("/room/{roomId}/members", args => Response.AsJson(Maika.GetRoomMembers((string)args.roomId)));
			Post("/room/{roomId}/name", args =>
			{
				if(Maika.SetRoomName((string)args.roomId, (Context.CurrentUser as UserPrincipal).Id, this.Bind<RoomModel>().Name))
					return new Response { StatusCode = HttpStatusCode.OK };
				else
					return new Response { StatusCode = HttpStatusCode.NotAcceptable };
			});
			Delete("/room/{roomId}", args =>
			{
				if(Maika.CloseRoom((string)args.roomId, (Context.CurrentUser as UserPrincipal).Id))
					return new Response { StatusCode = HttpStatusCode.OK };
				else
					return new Response { StatusCode = HttpStatusCode.NotAcceptable };
			});
			Post("/room/{roomId}", args =>
			{
				if(Maika.SetOwner((string)args.roomId, (Context.CurrentUser as UserPrincipal).Id, this.Bind<UserModel>().Username))
					return new Response { StatusCode = HttpStatusCode.OK };
				else
					return new Response { StatusCode = HttpStatusCode.NotAcceptable };
			});

			//Invites
			Post("/invite", args => Response.AsJson(Maika.CreateInvite(this.Bind<RoomModel>("Name", "Owner", "Members", "IsPublic").Id)));
			Delete("/invite/{inviteId}", args =>
			{
				if (Maika.DeleteInvite((string)args.inviteId))
					return new Response { StatusCode = HttpStatusCode.OK };
				else
					return new Response { StatusCode = HttpStatusCode.NotAcceptable };
			});


		}
	}
}
