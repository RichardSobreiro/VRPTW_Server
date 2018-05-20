using System.Configuration;

namespace VRPTW.CrossCutting.Configuration
{
	public static class GeneralConfigurations
	{
		public static bool OPLDEBUGMODE
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

		public static int CACHE_DURATION_IN_MINUTES
		{
			get
			{
				int cacheDuration = 30;
				if (int.TryParse(ConfigurationManager.AppSettings["CACHE_DURATION_IN_MINUTES"], out cacheDuration))
				{
					return cacheDuration;
				}
				return 30;
			}
		}

		public static int TOTAL_SECONDS_LIMIT_SOLVER
		{
			get
			{
				int totalSeconds = 5;
				if (int.TryParse(ConfigurationManager.AppSettings["TOTAL_SECONDS_LIMIT_SOLVER"], out totalSeconds))
				{
					return totalSeconds;
				}
				return 5;
			}
		}
	}
}
