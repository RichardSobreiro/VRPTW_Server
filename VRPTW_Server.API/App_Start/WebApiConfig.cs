using Google.Maps;
using SimpleInjector.Integration.WebApi;
using System.Configuration;
using System.Web.Http;
using VRPTW.DependenciesInjector;

namespace VRPTW_Server.API.App_Start
{
	public class WebApiConfig
	{
		public static void Configure(HttpConfiguration config)
		{
			GoogleSigned.AssignAllServices(new GoogleSigned(ConfigurationManager.AppSettings["GMAPS_API_KEY"]));

			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			Injector.Begins(config);

			config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(Injector.GetContainer);
		}
	}
}