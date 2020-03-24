using System.Collections.Generic;

namespace EitIotService.Models
{
	public class NbIotWebhookData
	{
		public NbIotMessage[] Messages { get; set; }
	}

	public class NbIotMessage
	{
		public NbIotDevice Device { get; set; }
		public string Payload { get; set; }
		public long Received { get; set; }
		public string Type { get; set; }
		public string Transport { get; set; }
		public NbIotCoapMetadata CoapMetaData { get; set; }
		public NbIotUdpMetadata UdpMetaData { get; set; }
	}

	public class NbIotDevice
	{
		public string DeviceId { get; set; }
		public string CollectionId { get; set; }
		public string Imei { get; set; }
		public string Imsi { get; set; }
		public Dictionary<string, string> Tags { get; set; }
		public NbIotDeviceNetwork Network { get; set; }
	}

	public class NbIotDeviceNetwork
	{
		public string AllocatedIp { get; set; }
		public long AllocatedAt { get; set; }
		public object CellId { get; set; }
	}

	public class NbIotCoapMetadata
	{
		public string Method { get; set; }
		public string Path { get; set; }
	}

	public class NbIotUdpMetadata
	{
		public int LocalPort { get; set; }
		public int RemotePort { get; set; }
	}
}
