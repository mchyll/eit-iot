using System;

namespace EitIotService.Services
{
	public interface IStatusService
	{
		DateTimeOffset StartupTime { get; }
		string RunningVersion { get; }
		string RunningEnvironment { get; }
	}
}
