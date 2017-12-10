using System;
using System.Collections.Generic;
using System.Text;
using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Security;
using Maika.Models;

namespace Maika.Modules
{
    public class IndexModule : NancyModule
    {
		public IndexModule()
		{
			StatelessAuthentication.Enable(this, MaikaCore.StatelessConfig);
			Get("/", _ =>
			{
				if (Context.CurrentUser != null)
					return Response.AsRedirect("/r/");
				return View["login"];
			});
		}
	}
}
