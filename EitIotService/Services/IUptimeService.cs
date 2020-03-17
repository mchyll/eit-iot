using System;

namespace EitIotService.Services
{
	public interface IUptimeService
	{
		DateTimeOffset StartupTime { get; }
	}
}
