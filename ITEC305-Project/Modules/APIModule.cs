﻿using System;
using System.Collections.Generic;
using System.Text;
using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Security;

namespace ITEC305_Project.Modules
{
    public class APIModule : NancyModule
    {
		public APIModule() : base("/api")
		{
			StatelessAuthentication.Enable(this, ITEC305Project.StatelessConfig);
			this.RequiresAuthentication();
			//User
			Get("/user/{userId}", args => ITEC305Project.GetUserInfo((ulong)args.userId));
			Post("/user/{userId}", args => ITEC305Project.SetUsername((ulong)args.userId, (string)args.username)); //TODO: Model Binding, User user context

			//Room
			Get("/room/{roomId}", args => ITEC305Project.GetRoomInfo((ulong)args.roomId));
			Get("/room/{roomId}/members", args => ITEC305Project.GetRoomMembers((ulong)args.roomId));
			Post("/room/{roomId}", args => ITEC305Project.SetRoomName((ulong)args.roomId, (string)args.roomName)); //TODO: Model Binding
			Delete("/room/{roomId}", args => ITEC305Project.CloseRoom((ulong)args.roomId));
			Post("/room/{roomId}", args => ITEC305Project.SetOwner((ulong)args.roomId, (ulong)args.userId)); //TODO: Model Binding
			Get("/room/{roomId}/canvas", args => null); //TODO: Canvas Transactions
			Post("/room/{roomId}/canvas", args => null); //TODO: Canvas Transactions
			Get("/room/{roomId}/chat", args => null); //TODO: Chat Transactions
			Post("/room/{roomId}/chat", args => null); //TODO: Chat Transactions

			//Invites
			Get("/invite", args => null); //TODO: Create invite
			Get("/invite/{inviteId}", args => null); //TODO: Is this useful?
			Delete("/invite/{inviteId}", args => ITEC305Project.DeleteInvite((ulong)args.inviteId));


		}
	}
}
