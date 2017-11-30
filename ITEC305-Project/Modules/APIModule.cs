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
			Get("/room/{roomId}", args => ITEC305Project.GetRoomInfo(args.roomId));
			Get("/room/{roomId}/members", args => ITEC305Project.GetRoomMembers(args.roomId));
			Post("/room/{roomId}", args => ITEC305Project.SetRoomName(args.roomId, (string)args.roomName)); //TODO: Model Binding
			Delete("/room/{roomId}", args => ITEC305Project.CloseRoom(args.roomId));
			Post("/room/{roomId}", args => ITEC305Project.SetOwner(args.roomId, args.userId)); //TODO: Model Binding
			Get("/room/{roomId}/canvas", args => null); //TODO: Canvas Transactions
			Post("/room/{roomId}/canvas", args => null); //TODO: Canvas Transactions
			Get("/room/{roomId}/chat", args => null); //TODO: Chat Transactions
			Post("/room/{roomId}/chat", args => null); //TODO: Chat Transactions

			//Invites
			Get("/invite", args => null); //TODO: Create invite
			Get("/invite/{inviteId}", args => null); //TODO: Is this useful?
			Delete("/invite/{inviteId}", args => ITEC305Project.DeleteInvite(args.inviteId));


		}
	}
}
