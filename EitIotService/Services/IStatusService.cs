using System;

namespace EitIotService.Services
{
	/// <summary>
	/// Service interface for providing helpful information during testing and development.
	/// </summary>
	public interface IStatusService
	{
		DateTimeOffset StartupTime { get; }
		string RunningVersion { get; }
		string RunningEnvironment { get; }
	}
}
