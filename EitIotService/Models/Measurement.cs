using System;

namespace EitIotService.Models
{
	/// <summary>
	/// Data Transfer Object (DTO) for serializing and sending measurement data through the public web API.
	/// </summary>
	public class Measurement
	{
		public long DatapointId { get; set; }
		public string DeviceId { get; set; }
		public DateTimeOffset Timestamp { get; set; }
		public float FillContentPercentage { get; set; }
		public float Temperature { get; set; }
	}
}
