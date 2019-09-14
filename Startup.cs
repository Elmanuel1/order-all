using Autofac;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SwaggerApp.Data;
using SwaggerApp.Models;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Reflection;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;
using Serilog.Formatting.Elasticsearch;
using SwaggerApp.Controllers.handler;
using SwaggerApp.Exceptions.handler;
using SwaggerApp.Service;
using SwaggerApp.Service.Impl.OrderService;
using static SwaggerApp.Controllers.handler.RequestResponseLoggingMiddleware;

[assembly: ApiController]
[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace SwaggerApp
{
    public class Startup
    {
        public IContainer ApplicationContainer { get; private set; }
        public Startup(IConfiguration configuration)
        {
            var udpFormatter = new ElasticsearchJsonFormatter();

            var loggerConfig = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .MinimumLevel.Information()
                .Enrich.WithProperty("applicationName", Assembly.GetExecutingAssembly().GetName().Name)
                .Enrich.WithProperty("applicationVersion",
                    Assembly.GetExecutingAssembly().GetName().Version.ToString());

            loggerConfig
                .WriteTo.Udp("127.0.0.1", 5044, AddressFamily.InterNetwork, udpFormatter);

            var logger = loggerConfig.CreateLogger();
            Log.Logger = logger;
            Serilog.Debugging.SelfLog.Enable(Console.Error);

            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
            .Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SampleContext>(options =>
                options.UseInMemoryDatabase("SampleData"));

            services.AddMvc(opts=>{
                   // opts.Filters.Add(typeof(ModelStateFeatureFilter));
            }).AddFluentValidation(fv => {
                    fv.RegisterValidatorsFromAssemblyContaining<FruitValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<MerchantValidator>();
                fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                    fv.ImplicitlyValidateChildProperties = true;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddControllersAsServices();
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60); //configure for a year
            });
            services.Configure<ApiBehaviorOptions>(options =>
            {
                // ...
                options.SuppressModelStateInvalidFilter = false;
            });
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });
            services.AddSwaggerDocument();
            services.AddTransient<IValidator<Fruit>, FruitValidator>();
            services.AddTransient<IValidator<Merchant>, MerchantValidator>();
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

            services.AddTransient<IOrderService, OrderService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup>  logger)
        {
            
            app.UseOpenApi();
            app.UseSwaggerUi3();

            if (!env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                Action<RequestProfilerModel> requestResponseHandler = requestProfilerModel =>
                {
                    Debug.Print(requestProfilerModel.Request);
                    Debug.Print(Environment.NewLine);
                    Debug.Print(requestProfilerModel.Response);
                };
                app.UseMiddleware<RequestResponseLoggingMiddleware>(requestResponseHandler);
                app.UseHsts();
                
            }

            app.UseForwardedHeaders();
            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {


                    // Use exceptionHandlerPathFeature to process the exception (for example, 
                    // logging), but do NOT expose sensitive error information directly to 
                    // the client.
                    await ExceptionHandler.HandleGlobalExceptionAsync(context);

                });
            });

            app.UseMvc();
        }
    }
}
