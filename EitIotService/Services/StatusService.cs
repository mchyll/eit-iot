using System;
using System.Reflection;

namespace EitIotService.Services
{
	public class StatusService : IStatusService
	{
		public DateTimeOffset StartupTime { get; private set; }
		public string RunningVersion { get; private set; }
		public string RunningEnvironment { get; private set; }

		public StatusService(DateTimeOffset startupTime, string runningVersion, string runningEnvironment)
		{
			StartupTime = startupTime;
			RunningVersion = runningVersion;
			RunningEnvironment = runningEnvironment;
		}
	}
}
