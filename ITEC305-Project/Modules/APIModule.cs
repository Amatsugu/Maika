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
			StatelessAuthentication.Enable(this, ITEC305Project.StatelessConfig);
			this.RequiresAuthentication();
			//User
			Get("/user/{userId}", args => Response.AsJson(ITEC305Project.GetUserInfo((string)args.userId)));
			Get("/user/", args => Response.AsJson(ITEC305Project.GetUserInfo((Context.CurrentUser as UserPrincipal).Id)));
			Post("/user/", args =>
			{
				if (ITEC305Project.SetUsername((Context.CurrentUser as UserPrincipal).Id, this.Bind<UserCredentialsModel>()))
					return new Response { StatusCode = HttpStatusCode.OK };
				else
					return new Response { StatusCode = HttpStatusCode.NotAcceptable };
			});	

			//Room
			Get("/room/{roomId}", args => Response.AsJson(ITEC305Project.GetRoomInfo((string)args.roomId)));
			Get("/room/{roomId}/members", args => Response.AsJson(ITEC305Project.GetRoomMembers((string)args.roomId)));
			Post("/room/{roomId}", args =>
			{
				if(ITEC305Project.SetRoomName((string)args.roomId, (Context.CurrentUser as UserPrincipal).Id, this.Bind<RoomModel>().Name))
					return new Response { StatusCode = HttpStatusCode.OK };
				else
					return new Response { StatusCode = HttpStatusCode.NotAcceptable };
			});
			Delete("/room/{roomId}", args =>
			{
				if(ITEC305Project.CloseRoom((string)args.roomId, (Context.CurrentUser as UserPrincipal).Id))
					return new Response { StatusCode = HttpStatusCode.OK };
				else
					return new Response { StatusCode = HttpStatusCode.NotAcceptable };
			});
			Post("/room/{roomId}", args =>
			{
				if(ITEC305Project.SetOwner((string)args.roomId, (Context.CurrentUser as UserPrincipal).Id, this.Bind<UserModel>().Username))
					return new Response { StatusCode = HttpStatusCode.OK };
				else
					return new Response { StatusCode = HttpStatusCode.NotAcceptable };
			});
			Get("/room/{roomId}/canvas", args => null); //TODO: Canvas Transactions
			Post("/room/{roomId}/canvas", args => null); //TODO: Canvas Transactions
			Get("/room/{roomId}/chat", args => null); //TODO: Chat Transactions
			Post("/room/{roomId}/chat", args => null); //TODO: Chat Transactions

			//Invites
			Get("/invite", args => Response.AsJson(ITEC305Project.CreateInvite()));
			Get("/invite/{inviteId}", args => null); //TODO: Is this useful?
			Delete("/invite/{inviteId}", args =>
			{
				if (ITEC305Project.DeleteInvite((string)args.inviteId))
					return new Response { StatusCode = HttpStatusCode.OK };
				else
					return new Response { StatusCode = HttpStatusCode.NotAcceptable };
			});


		}
	}
}
