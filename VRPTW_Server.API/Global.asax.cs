using System;
using System.Diagnostics;
using System.Web.Http;
using VRPTW_Server.API.App_Start;

namespace VRPTW_Server.API
{
	public class Global : System.Web.HttpApplication
	{

		protected void Application_Start(object sender, EventArgs e)
		{
			GlobalConfiguration.Configure(WebApiConfig.Configure);
		}

		protected void Session_Start(object sender, EventArgs e)
		{

		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(object sender, EventArgs e)
		{

		}

		protected void Application_Error(object sender, EventArgs e)
		{
			Exception ex = Server.GetLastError();
			if (ex != null)
			{
				Trace.TraceError(ex.ToString());
			}
		}

		protected void Session_End(object sender, EventArgs e)
		{

		}

		protected void Application_End(object sender, EventArgs e)
		{

		}
	}
}