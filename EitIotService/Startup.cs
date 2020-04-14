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

namespace EitIotService
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

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

			services.AddSingleton<IUptimeService>(new UptimeService(DateTimeOffset.Now));
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
