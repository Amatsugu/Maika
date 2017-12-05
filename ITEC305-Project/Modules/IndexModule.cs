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
			StatelessAuthentication.Enable(this, Maika.StatelessConfig);
			Get("/", _ =>
			{
				return View["login"];
			});
		}
	}
}
