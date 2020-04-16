using EitIotService.Data;
using EitIotService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.AzureAppServices;
using System;
using System.Reflection;

namespace EitIotService
{
	public class Startup
	{
		public Startup(IConfiguration configuration, IWebHostEnvironment env)
		{
			Configuration = configuration;
			_env = env;
		}

		public IConfiguration Configuration { get; }

		private readonly IWebHostEnvironment _env;

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCors(options =>
			{
				options.AddPolicy("AllowAnyOrigin", builder =>
				{
					builder.AllowAnyOrigin();
				});
			});

			services.AddControllers();

			services.AddDbContext<SensorDataContext>(options =>
					options.UseSqlServer(Configuration.GetConnectionString("SensorDataContext")));

			services.Configure<AzureFileLoggerOptions>(Configuration.GetSection("AzureLogging"));

			var version = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
			services.AddSingleton<IStatusService>(new StatusService(DateTimeOffset.Now, version, _env.EnvironmentName));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> log)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseDefaultFiles();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseCors();

			//app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			log.LogInformation("EitIotService started");
		}
	}
}
