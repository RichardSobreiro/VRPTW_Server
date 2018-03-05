using System.Configuration;

namespace VRPTW.CrossCutting.Configuration
{
	public static class GeneralConfigurations
	{
		public static bool OplDebugMode
		{
			get
			{
				bool oplDebugMode = false;
				if(bool.TryParse(ConfigurationManager.AppSettings["OPL_DEBUG_MODE"], out oplDebugMode))
				{
					return oplDebugMode;
				}
				return false;
			}
		}
	}
}
