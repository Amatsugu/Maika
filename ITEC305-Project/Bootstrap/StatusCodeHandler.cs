using Nancy.ErrorHandling;
using System;
using System.Text;
using Nancy;
using System.Linq;
using ITEC305_Project.Models;

namespace ITEC305_Project.Bootstrap
{
	public class StatusCodeHandler : IStatusCodeHandler
	{

		private readonly HttpStatusCode[] _handledCodes = new HttpStatusCode[]
		{
			//HttpStatusCode.NotFound,
			//HttpStatusCode.Unauthorized
		};

		public void Handle(HttpStatusCode statusCode, NancyContext context)
		{
			if (statusCode == HttpStatusCode.Unauthorized && context.Request.Path == "/")
			{
				context.Response = new Response //TODO: ERROR Codes
				{

				};
			}
			else
			{
				context.Response = null; //TODO: ERROR Codes
			}
		}

		public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context) => _handledCodes.Any(x => x == statusCode);
	}
}
