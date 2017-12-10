using Nancy.ErrorHandling;
using System;
using System.Text;
using Nancy;
using System.Linq;

namespace Maika.Bootstrap
{
	public class StatusCodeHandler : IStatusCodeHandler
	{

		private readonly HttpStatusCode[] _handledCodes = new HttpStatusCode[]
		{
			HttpStatusCode.NotFound,
			HttpStatusCode.Unauthorized,
			HttpStatusCode.Checkpoint,
			HttpStatusCode.InternalServerError
		};

		public void Handle(HttpStatusCode statusCode, NancyContext context)
		{
			//TODO: Invite Redirection
			//if(context.Request.Path.Contains(@"/i/"))
			//	context.Response = new Response().WithHeader("Location", $"/error/{(int)statusCode}").WithStatusCode(HttpStatusCode.TemporaryRedirect);

			context.Response = new Response().WithHeader("Location", $"/error/{(int)statusCode}").WithStatusCode(HttpStatusCode.TemporaryRedirect);
		}

		public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
		{
			if (context.Request.Path.Contains(@"/error/"))
				return false;
			return _handledCodes.Any(x => x == statusCode);
		}
	}
}
