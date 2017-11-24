using System;
using System.Collections.Generic;
using System.Text;
using Nancy;
using Nancy.Security;
using Nancy.Authentication.Stateless;

namespace ITEC305_Project.Modules
{
    public class Room : NancyModule
    {
		public Room() : base("/r")
		{
			StatelessAuthentication.Enable(this, ITEC305Project.StatelessConfig);
			this.RequiresAuthentication();
			Get("/", _ => View["room"]); //TODO: Room Model
		}
    }
}
