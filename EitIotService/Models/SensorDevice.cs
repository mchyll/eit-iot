using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EitIotService.Models
{
	/// <summary>
	/// Model used for storing sensor device information in the database.
	/// </summary>
	public class SensorDevice
	{
		[Key]
		public string DeviceId { get; set; }
		public string CollectionId { get; set; }
		public string Imei { get; set; }
		public string Imsi { get; set; }
		public string Tags { get; set; }
		public List<SensorData> DataPoints { get; set; }
	}
}
