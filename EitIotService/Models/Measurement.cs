using System;

namespace EitIotService.Models
{
	public class Measurement
	{
		public long DatapointId { get; set; }
		public string DeviceId { get; set; }
		public DateTimeOffset Timestamp { get; set; }
		public float FillContentPercentage { get; set; }
		public float Temperature { get; set; }
	}
}
