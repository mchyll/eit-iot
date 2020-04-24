using System;

namespace EitIotService.Models
{
	/// <summary>
	/// Model for a debug log entry used for testing and development.
	/// </summary>
	public class Log
	{
		public long Id { get; set; }
		public string Location { get; set; }
		public string Message { get; set; }
		public DateTimeOffset Timestamp { get; set; }

		public Log(string location, string msg)
		{
			Location = location;
			Message = msg;
			Timestamp = DateTimeOffset.Now;
		}

		public Log()
		{
		}
	}
}
