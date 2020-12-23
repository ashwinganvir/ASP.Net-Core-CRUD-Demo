
using CRUD_Demo.Business.Operational;
using CRUD_Demo.Common.Settings.Models;
using CRUD_Demo.Web.Middlewares;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using Serilog;

using System;

namespace CRUD_Demo.Web
{
    public class Startup
    {
        /// <summary>
        /// Represents appsettings.json
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Represents ASP.NET hosting environment
        /// </summary>
        public IWebHostEnvironment Environment { get; }

        public Startup(IWebHostEnvironment env)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = configBuilder.Build();
            Environment = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom
                .Configuration(Configuration)
                .CreateLogger();

            services.AddSingleton(Log.Logger);

            IConfigurationSection connectSection = Configuration.GetSection("ConnectSettings");
            services.Configure<ConnectModel>(connectSection);
            try
            {
                services.AddApplication(Configuration);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Error adding Application Components.");
            }

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "CRUD_Demo API", Version = "v1" });
                var serviceProvider = services.BuildServiceProvider();
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<LoggingMiddleware>();

            // Log that the application is starting
            using (IServiceScope serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger>();

                logger.Information("The CRUD_Demo service is running.");

                try
                {
                    app.UseApplication();
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Database error.");
                }
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("../swagger/v1/swagger.json", "CRUS_Demo API v1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
