using EitIotService.Data;
using EitIotService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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
		private ILogger<NbIotWebhookController> log;

		public NbIotWebhookController(SensorDataContext context, ILogger<NbIotWebhookController> log)
		{
			this.context = context;
			this.log = log;
		}

		// POST: api/NbIotWebhook
		[HttpPost]
		public async Task<IActionResult> PostSensorData(NbIotWebhookData data)
		{
			//var rawRequest = await GetRawRequestBody(Request);
			//Debug.WriteLine($"Raw request body: {rawRequest}");
			//context.Logs.Add(new Log("NbIotWebhookController.PostSensorData", $"Raw request body: {rawRequest}"));

			log.LogInformation($"Got delivery of {data.Messages.Length} messages from NBIoT cloud");
			context.Logs.Add(new Log("NbIotWebhookController.PostSensorData",
				$"Got delivery of {data.Messages.Length} messages from NBIoT cloud: {JsonSerializer.Serialize(data)}"));

			foreach (var message in data.Messages)
			{
				// Ensure the sensor device exists in DB
				if (await context.SensorDevices.FindAsync(message.Device.DeviceId) == null)
				{
					await PostSensor(message.Device);
				}

				// Decode the payload and store the datapoint in DB
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

			return Ok("Data accepted and stored.");
		}

		// POST: api/NbIotWebhook/Sensor
		[HttpPost("Sensor")]
		public async Task<IActionResult> PostSensor(NbIotDevice device)
		{
			//var rawRequest = await GetRawRequestBody(Request);
			//Debug.WriteLine($"Raw request body: {rawRequest}");
			//context.Logs.Add(new Log("NbIotWebhookController.PostSensor", $"Raw request body: {rawRequest}"));

			log.LogInformation($"Adding new sensor with id {device.DeviceId}");
			context.Logs.Add(new Log("NbIotWebhookController.PostSensor", $"Adding new sensor: {JsonSerializer.Serialize(device)}"));

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
