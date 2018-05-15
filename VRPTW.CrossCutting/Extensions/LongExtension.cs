using System;

namespace VRPTW.CrossCutting.Extensions
{
	public static class LongExtension
	{
		public static DateTime ConvertMinutesToDateTime(this long value)
		{
			DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return start.AddMinutes(value / 60);
		}
	}
}
