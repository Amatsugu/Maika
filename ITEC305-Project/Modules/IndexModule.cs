using System;
using System.Collections.Generic;
using System.Text;
using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Security;
using ITEC305_Project.Models;

namespace ITEC305_Project.Modules
{
    public class IndexModule : NancyModule
    {
		public IndexModule()
		{
			Console.WriteLine("INDEX");
			StatelessAuthentication.Enable(this, ITEC305Project.StatelessConfig);
			this.RequiresAuthentication();
			Get("/", _ =>
			{
				Console.WriteLine("GET");
				Console.WriteLine((Context?.CurrentUser as UserIdenity)?.UserName);
				return "index";
				//return View["index", new { user = Context.CurrentUser}];
			});
		}
	}
}
