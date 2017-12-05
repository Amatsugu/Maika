using System;
using System.Collections.Generic;
using System.Text;
using Nancy;
using Nancy.ModelBinding;
using ITEC305_Project.Models;
using ITEC305_Project.Auth;

namespace ITEC305_Project.Modules
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
