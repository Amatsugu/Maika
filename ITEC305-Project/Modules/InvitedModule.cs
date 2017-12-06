using System;
using System.Collections.Generic;
using System.Text;
using Nancy;

namespace ITEC305_Project.Modules
{
    public class InvitedModule : NancyModule
    {
		public InvitedModule()
		{
			Get("/invite/{inviteId}", args =>
			{
				return View["invite"];
			});
		}
    }
}
