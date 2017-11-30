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
			StatelessAuthentication.Enable(this, ITEC305Project.StatelessConfig);
			this.RequiresAuthentication();
			Get("/", _ =>
			{
				Console.WriteLine(Context?.CurrentUser.Identity.Name);
				return "";
				//return View["index", new { user = Context.CurrentUser}];
			});
		}
	}
}
