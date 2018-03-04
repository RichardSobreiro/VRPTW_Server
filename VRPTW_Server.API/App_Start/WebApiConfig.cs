using Google.Maps;
using System.Configuration;
using System.Web.Http;

namespace VRPTW_Server.API.App_Start
{
	public class WebApiConfig
	{
		public static void Configure(HttpConfiguration config)
		{
			GoogleSigned.AssignAllServices(new GoogleSigned(ConfigurationManager.AppSettings["GMAPS_API-KEY"]));

			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
		}
	}
}