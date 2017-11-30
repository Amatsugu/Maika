using ITEC305_Project.Models;
using Nancy;
using Nancy.Extensions;
using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITEC305_Project.Modules
{
	public class AuthModule : NancyModule
	{
		public AuthModule() : base("/auth")
		{
			Post("/login", args =>
			{
				var login = this.Bind<LoginCredentialsModel>();
				var token = ITEC305Project.ValidateUser(login);
				Console.WriteLine(token);
				if (string.IsNullOrEmpty(token))
					return new Response { StatusCode = HttpStatusCode.Unauthorized };
				else
					return new Response().WithCookie("Token", token, DateTime.Now.AddDays(5));
			});

			Get("/logout", _ => Response.AsRedirect("/").WithCookie("Token", null, DateTime.Now));

			Post("/register", _ =>
			{
				var user = this.Bind<LoginCredentialsModel>();
				ITEC305Project.CreateUser(user);
				var auth = ITEC305Project.ValidateUser(user);
				return new Response().WithCookie("Token", auth, DateTime.Now.AddDays(5));
			});

			Get("/checkemail/{email}", args => (ITEC305Project.CheckEmailExists((string)args.email)) ? new Response { StatusCode = HttpStatusCode.NotAcceptable } : new Response { StatusCode = HttpStatusCode.OK });

		}
	}

}
