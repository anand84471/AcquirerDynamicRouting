using AutoMapper;
using Core.Extensions;
using Core.Infrastructure;
using ExpressCheckoutModule.Infrastructure.Automapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO.Compression;
using TorrentPay.AsyncIntializiers;
using TorrentPayController.BusinessLayer.Abstract;
using TorrentPayController.BusinessLayer.Concrete;

namespace TorrentPayController
{
    public class TorrentPayStartup:Startup
    {
        public TorrentPayStartup(IWebHostEnvironment env)
        {
            this.Configuration = WireUp.AddAppSettings(env.EnvironmentName).Build();
        }
        public override void ConfigureServices(IServiceCollection services)
        {
            
            WireUp.WireUpCore(services);
            services.AddControllers();
            base.ConfigureServices(services);
            services.AddResponseCompression();
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
            });

           
        }
        protected override void RegisterBusinessLayer(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutomapperMappingProfile));
            services.AddTransient<ITorrentPayValidation, TorrentPayValidation>();
            services.AddAsyncInitializer<AsyncCacheInitializer>();
            services.AddMvc().AddNewtonsoftJson();
        }
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
            app.UseResponseCompression();

            app.UseHttpsRedirection();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

           

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

        }
    }
}
