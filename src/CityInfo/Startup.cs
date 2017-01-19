using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using CityInfo.Services;
using Microsoft.Extensions.Configuration;
using CityInfo.Entities;
using Microsoft.EntityFrameworkCore;
using CityInfo.Models;

namespace CityInfo
{
    public class Startup
    {
        public static IConfigurationRoot configuration;
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                ;

            configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddMvcOptions(o => o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter()));
            //.AddJsonOptions(o =>
            //{
            //    if (o.SerializerSettings.ContractResolver != null)
            //    {
            //        var castResolver = o.SerializerSettings.ContractResolver as DefaultContractResolver;
            //        castResolver.NamingStrategy = null;
            //    }
            //});

            //services.AddC
            var connectionstring = Startup.configuration["ConnectionStrings:CityInfoDBConnectionString"];

            services.AddDbContext<CityInfoContext>(o => o.UseSqlServer(connectionstring));
            services.AddScoped<ICityInfoRepository, CityInfoRepository>();

            ///compiler directive

#if DEBUG
            services.AddTransient<IMailServiceInterface, LocalMailService>();
#else
            services.AddTransient<IMailServiceInterface,CloudMailService>();
#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, CityInfoContext ctx)
        {
            ctx.EnsureSeedDataForContext();
            loggerFactory.AddConsole();

            loggerFactory.AddDebug();
            //loggerFactory.AddProvider(new NLog.Extensions.Logging.NLogLoggerProvider());
            loggerFactory.AddNLog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseStatusCodePages();
            app.UseMvc();
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<City, CityWithoutPointsOfIntersestDto>();
                cfg.CreateMap<City, CityDto>();
                cfg.CreateMap<PointOfIntersect, PointOfIntersectDto>();
                cfg.CreateMap<PointOfIntersectForCreationDto, PointOfIntersect>();
                cfg.CreateMap<PointOfIntersectForUpdateDto, PointOfIntersect>();
                cfg.CreateMap<PointOfIntersect,PointOfIntersectForUpdateDto>();
            });

            //app.Run((context) =>
            //{
            //    throw new Exception("Example Exception");
            //});
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
