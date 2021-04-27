using AutoMapper;
using Core.Extensions;
using Core.Infrastructure;
using DynamicRouting.AsyncIntializiers;
using DynamicRouting.BusinessLayer.Abstract;
using DynamicRouting.BusinessLayer.Concrete;
using DynamicRouting.JSExecutionEngineSetupInitializer;
using ExpressCheckoutModule.Infrastructure.Automapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicRouting
{
    public class DynamicRoutingStartup : Startup
    {
        public DynamicRoutingStartup(IWebHostEnvironment env)
        {
            this.Configuration = WireUp.AddAppSettings(env.EnvironmentName).Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public override void ConfigureServices(IServiceCollection services)
        {
            WireUp.WireUpCore(services);
            services.AddControllers();
            base.ConfigureServices(services);
        }

        protected override void RegisterBusinessLayer(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutomapperMappingProfile));
            services.AddTransient<IRoutingEngineService,RoutingEngineService>();
            services.AddTransient<IRoutingGatewaySortingStartegy, DefaultRoutingGatewaySortingStartegy>();
            services.AddTransient<SimpleRoutingLogicExceutionHandlersService>()
               .AddTransient<IRoutingLogicExceutionHandlersService, SimpleRoutingLogicExceutionHandlersService>(s => s.GetService<SimpleRoutingLogicExceutionHandlersService>());

            services.AddTransient<CustomizeRoutingLogicExceutionHandlersService>()
                .AddTransient<IRoutingLogicExceutionHandlersService, CustomizeRoutingLogicExceutionHandlersService>(s => s.GetService<CustomizeRoutingLogicExceutionHandlersService>());

            services.AddTransient<SpecialRoutingLogicExecutionHandlerService>()
                 .AddTransient<IRoutingLogicExceutionHandlersService, SpecialRoutingLogicExecutionHandlerService>(s => s.GetService<SpecialRoutingLogicExecutionHandlerService>());
          
            services.AddAsyncInitializer<AsyncCacheInitializer>();

            services.AddSingleton<JSExecutionInitializer>();

            services.AddMvc().AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ConfigureCustomExceptionMiddleware();
            //app.UseStaticFiles();
           // app.UseHttpsRedirection();

            app.UseRouting();

          //  app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

          //  this.UseSwagger(app);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        //public override void ConfigureServices(IServiceCollection services)
        //{
        //    WireUp.WireUpCore(services);

        //    services.AddControllers();
        //    base.ConfigureServices(services);
        //}

        //protected override void RegisterBusinessLayer(IServiceCollection services)
        //{
        //    services.AddScoped<IExpressCheckoutCustomerGateBusinessLayer, ExpressCheckoutCustomerGateBusinessLayer>();
        //}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    }
}
