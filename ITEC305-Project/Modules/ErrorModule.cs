using System;
using System.Collections.Generic;
using System.Text;
using Nancy;

namespace ITEC305_Project.Modules
{
    public class ErrorModule : NancyModule
    {
		public ErrorModule() : base("/error/")
		{
			Get("/{errorCode}", args =>
			{
				return View["error", args];
			});
		}
    }
}
