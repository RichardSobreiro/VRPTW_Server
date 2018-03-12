using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace VRPTW_Server.API.Controllers
{
	public class BaseController : ApiController
    {
		[ApiExplorerSettings(IgnoreApi = true)]
		public void NotFound(string message)
		{
			var response = new HttpResponseMessage(HttpStatusCode.NotFound)
			{
				Content = new StringContent(message),
			};

			throw new HttpResponseException(response);
		}

		[ApiExplorerSettings(IgnoreApi = true)]
		public HttpResponseMessage BusinessError(Exception ex)
		{
			return new HttpResponseMessage()
			{
				StatusCode = HttpStatusCode.BadRequest,
				Content = new StringContent(ex.Source),
				ReasonPhrase = ex.Message
			};
		}
	}
}
