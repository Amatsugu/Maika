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
			Get("/user/{userId}", args => Response.AsJson(Maika.GetUserInfo((string)args.userId)));
			Get("/user/", args => Response.AsJson(Maika.GetUserInfo((Context.CurrentUser as UserPrincipal).Id)));
			Post("/user/", args =>
			{
				if (Maika.SetUsername((Context.CurrentUser as UserPrincipal).Id, this.Bind<UserCredentialsModel>()))
					return new Response { StatusCode = HttpStatusCode.OK };
				else
					return new Response { StatusCode = HttpStatusCode.NotAcceptable };
			});	

			//Room
			Get("/room/{roomId}", args => Response.AsJson(Maika.GetRoomInfo((string)args.roomId)));
			Get("/room/{roomId}/members", args => Response.AsJson(Maika.GetRoomMembers((string)args.roomId)));
			Post("/room/{roomId}", args =>
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
			Get("/room/{roomId}/canvas", args => null); //TODO: Canvas Transactions
			Post("/room/{roomId}/canvas", args => null); //TODO: Canvas Transactions
			Get("/room/{roomId}/chat", args => null); //TODO: Chat Transactions
			Post("/room/{roomId}/chat", args => null); //TODO: Chat Transactions

			//Invites
			Get("/invite", args => Response.AsJson(Maika.CreateInvite()));
			Get("/invite/{inviteId}", args => null); //TODO: Is this useful?
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
