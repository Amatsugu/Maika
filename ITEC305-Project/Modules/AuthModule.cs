using ITEC305_Project.Models;
using Nancy;
using Nancy.Extensions;
using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;
using Nancy.Authentication.Stateless;

namespace ITEC305_Project.Modules
{
	public class AuthModule : NancyModule
	{
		public AuthModule() : base("/auth")
		{
			StatelessAuthentication.Enable(this, Maika.StatelessConfig);
			Post("/login", args =>
			{
				if (Context.CurrentUser != null)
					return Response.AsRedirect("/");
				var login = this.Bind<UserCredentialsModel>();
				var token = Maika.ValidateUser(login);
				Console.WriteLine(token);
				if (string.IsNullOrEmpty(token))
					return new Response { StatusCode = HttpStatusCode.Unauthorized };
				else
					return new Response().WithCookie("Token", token, DateTime.Now.AddDays(5));
			});

			Get("/logout", _ => Response.AsRedirect("/").WithCookie("Token", null, DateTime.Now));

			Post("/register", _ =>
			{
				if (Context.CurrentUser != null)
					return Response.AsRedirect("/");
				var user = this.Bind<UserCredentialsModel>();
				var newUser = Maika.CreateUser(user);
				if (newUser == null)
					return new Response().WithStatusCode(HttpStatusCode.NotAcceptable);
				var auth = Maika.ValidateUser(user);
				return new Response().WithCookie("Token", auth, DateTime.Now.AddDays(5));
			});

			Get("/checkemail/{email}", args => (Maika.CheckEmailExists((string)args.email)) ? new Response { StatusCode = HttpStatusCode.NotAcceptable } : new Response { StatusCode = HttpStatusCode.OK });

		}
	}

}
