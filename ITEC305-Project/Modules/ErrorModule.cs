using System;
using System.Collections.Generic;
using System.Text;
using Nancy;

namespace Maika.Modules
{
    public class ErrorModule : NancyModule
    {
		public ErrorModule() : base("/error/")
		{
			Get("/{errorCode}", args =>
			{
				return View["error"].WithModel((object)args).WithStatusCode((int)args.errorCode);
			});
		}
    }
}
