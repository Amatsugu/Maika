using Nancy.ErrorHandling;
using System;
using System.Text;
using Nancy;
using System.Linq;

namespace ITEC305_Project.Bootstrap
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
			context.Response = new Response().WithHeader("Location", $"/error/{(int)statusCode}").WithStatusCode(HttpStatusCode.TemporaryRedirect);
		}

		public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context) => _handledCodes.Any(x => x == statusCode);
	}
}
