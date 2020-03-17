using EitIotService.Data;
using EitIotService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EitIotService.Controllers
{
	[Route("api/NbIotWebhook")]
	[ApiController]
	public class NbIotWebhookController : ControllerBase
	{
		private SensorDataContext context;

		public NbIotWebhookController(SensorDataContext context)
		{
			this.context = context;
		}

		[HttpPost]
		public async Task<IActionResult> PostSensorData(NbIotWebhookData data)
		{
			//var rawRequest = await GetRawRequestBody(Request);
			//Debug.WriteLine($"Raw request body: {rawRequest}");
			//context.Logs.Add(new Log("NbIotWebhookController.PostSensorData", $"Raw request body: {rawRequest}"));

			context.Logs.Add(new Log("NbIotWebhookController.PostSensorData", $"Got delivery of {data.Messages.Length} messages from NBIoT cloud"));

			foreach (var message in data.Messages)
			{
				var payload = Encoding.UTF8.GetString(Convert.FromBase64String(message.Payload)).Split(' ');

				context.SensorDatas.Add(new SensorData
				{
					DeviceId = message.Device.DeviceId,
					Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(message.Received),
					Temperature = float.Parse(payload[0]),
					FillContentMeters = float.Parse(payload[1])
				});
			}

			await context.SaveChangesAsync();

			return Ok();
		}

		[HttpPost("Sensor")]
		public async Task<IActionResult> PostSensor(NbIotDevice device)
		{
			//var rawRequest = await GetRawRequestBody(Request);
			//Debug.WriteLine($"Raw request body: {rawRequest}");
			//context.Logs.Add(new Log("NbIotWebhookController.PostSensor", $"Raw request body: {rawRequest}"));

			context.Logs.Add(new Log("NbIotWebhookController.PostSensor", $"Adding new sensor with id {device.DeviceId}"));

			context.SensorDevices.Add(new SensorDevice
			{
				DeviceId = device.DeviceId,
				CollectionId = device.CollectionId,
				Imei = device.Imei,
				Imsi = device.Imsi,
				Tags = JsonSerializer.Serialize(device.Tags)
			});
			await context.SaveChangesAsync();

			return Ok($"Device {device.DeviceId} in collection {device.CollectionId} was added.");
		}

		private async Task<string> GetRawRequestBody(HttpRequest request)
		{
			if (request.Body.CanSeek)
			{
				request.Body.Position = 0;
			}

			using var stream = new StreamReader(request.Body, Encoding.UTF8);
			return await stream.ReadToEndAsync();
		}
	}
}
