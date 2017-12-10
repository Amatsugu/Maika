using System;
using System.Collections.Generic;
using System.Text;
using Nancy;
using Nancy.ModelBinding;
using Maika.Models;
using Maika.Auth;

namespace Maika.Modules
{
    public class RegisterModule : NancyModule
    {
		public RegisterModule()
		{
			Get("/register", _ =>
			{
				return View["register"];
			});
		}
    }
}
