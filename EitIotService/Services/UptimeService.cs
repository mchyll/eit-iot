using System;

namespace EitIotService.Services
{
	public class UptimeService : IUptimeService
	{
		public DateTimeOffset StartupTime { get; private set; }

		public UptimeService(DateTimeOffset startupTime)
		{
			StartupTime = startupTime;
		}
	}
}
