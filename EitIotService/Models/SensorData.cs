using System;

namespace EitIotService.Models
{
	/// <summary>
	/// Model used for storing a datapoint from the sensor devices in the database.
	/// </summary>
	public class SensorData
	{
		public long Id { get; set; }
		public string DeviceId { get; set; }
		public SensorDevice Device { get; set; }
		public DateTimeOffset Timestamp { get; set; }
		public float FillContentMeters { get; set; }
		public float Temperature { get; set; }
	}
}
