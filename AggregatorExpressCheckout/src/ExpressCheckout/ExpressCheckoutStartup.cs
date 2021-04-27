using AutoMapper;
using Core.Extensions;
using Core.Infrastructure;
using ExpressCheckout.AsyncIntializiers;
using ExpressCheckout.BusinessLayer.Abstract;
using ExpressCheckout.BusinessLayer.Concrete;
using ExpressCheckoutModule.Infrastructure.Automapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExpressCheckout
{
    public class ExpressCheckoutStartup : Startup
    {
        public ExpressCheckoutStartup(IWebHostEnvironment env)
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
            services.AddTransient<IExpressCheckoutCustomerGateValidation, ExpressCheckoutCustomerGateValidation>();
            services.AddTransient<ICustomerSavedCardValidation, CustomerSavedCardValidation>();
            services.AddTransient<IOrderValidation, OrderValidation>();
            services.AddTransient<IMerchantValidation, MerchantValidation>();
            services.AddTransient<IRazorpayValidation, RazorpayValidation>();
            services.AddTransient<IMobileAppValidation, MobileAppValidation>();
            services.AddTransient<IPayUValidation, PayUValidation>();

            services.AddAsyncInitializer<AsyncCacheInitializer>();
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
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            this.UseSwagger(app);
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