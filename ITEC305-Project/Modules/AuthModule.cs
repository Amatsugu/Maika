using Maika.Models;
using Nancy;
using Nancy.Extensions;
using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;
using Nancy.Authentication.Stateless;

namespace Maika.Modules
{
	public class AuthModule : NancyModule
	{
		public AuthModule() : base("/auth")
		{
			StatelessAuthentication.Enable(this, MaikaCore.StatelessConfig);
			Post("/login", args =>
			{
				if (Context.CurrentUser != null)
					return Response.AsRedirect("/");
				var login = this.Bind<UserCredentialsModel>();
				var token = MaikaCore.ValidateUser(login);
				Console.WriteLine(token);
				if (string.IsNullOrEmpty(token))
					return new Response { StatusCode = HttpStatusCode.Unauthorized };
				else
					return Response.AsRedirect("/r").WithCookie("Token", token, DateTime.Now.AddDays(5));
			});

			Get("/logout", _ => Response.AsRedirect("/").WithCookie("Token", null, DateTime.Now));

			Post("/register", _ =>
			{
				if (Context.CurrentUser != null)
					return Response.AsRedirect("/");
				var user = this.Bind<UserCredentialsModel>();
				var newUser = MaikaCore.CreateUser(user);
				if (newUser == null)
					return new Response().WithStatusCode(HttpStatusCode.NotAcceptable);
				var auth = MaikaCore.ValidateUser(user);
				return Response.AsRedirect("/").WithCookie("Token", auth, DateTime.Now.AddDays(5));
			});

			Get("/checkemail/{email}", args => (MaikaCore.CheckEmailExists((string)args.email)) ? new Response { StatusCode = HttpStatusCode.NotAcceptable } : new Response { StatusCode = HttpStatusCode.OK });

		}
	}

}
