using Core.Configuration;
using Core.Infrastructure.DI;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;

namespace Core.Infrastructure
{
    public abstract class Startup
    {
        /// <summary>
        /// The configuration.
        /// </summary>
        protected IConfigurationRoot Configuration { get; set; }

        // Called from .NET Core Pipeline

        /// <summary>
        /// Configure services for the application.
        /// </summary>
        /// <param name="services">services</param>
        // ReSharper disable once MemberCanBeProtected.Global
        // ReSharper disable once UnusedMemberHierarchy.Global
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            this.ConfigureApplicationSettingsServices(services);

            //   services.AddControllers().AddNewtonsoftJson();
            this.ConfigureAutoMapperServices(services);

            this.ConfigureSwaggerServices(services);

            this.RegisterComponents(services);

            DependencyRegistrar.RegisterAssemblies(services, this.Configuration);

            // this.IntializeCustomCache(services);
        }

        /// <summary>
        /// Configure swagger
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <param name="provider">API Version Description Provider</param>
        protected void UseSwagger(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
                             {
                                 c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Api");
                             });
        }

        /// <summary>
        /// Configure Swagger Services
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        protected void ConfigureSwaggerServices(IServiceCollection services)
        {
            services.AddSwaggerGen(
                options =>
                {
                    var info = new OpenApiInfo
                    {
                        Title = "Express Checkout",
                        Version = "v1"
                    };
                    options.SwaggerDoc("v1", info);
                    var dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                    foreach (var fi in dir.EnumerateFiles("*.xml", SearchOption.AllDirectories))
                    {
                        options.IncludeXmlComments(fi.FullName);
                    }
                });
            //services.AddSwaggerGenNewtonsoftSupport();
        }

        /// <summary>
        /// Adds the swagger authorization.
        /// </summary>
        /// <param name="options">The SwaggerGen options.</param>
        protected virtual void AddSwaggerAuthorization(SwaggerGenOptions options)
        {
        }

        /// <summary>
        /// Configure application setting services
        /// </summary>
        /// <param name="services">services</param>
        protected virtual void ConfigureApplicationSettingsServices(IServiceCollection services)
        {
            services.Configure<ResponseHandlerConfiguration>(Configuration.GetSection("ResponseHandlerConfiguration"));
        }

        /// <summary>
        /// Registrations of Business Layers.
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        protected virtual void RegisterBusinessLayer(IServiceCollection services)
        {
        }

        /// <summary>
        /// Register All custom Components.
        /// </summary>
        /// <param name="services">services</param>
        // ReSharper disable once VirtualMemberNeverOverridden.Global
        // ReSharper disable once UnusedParameter.Global
        protected virtual void RegisterCustomComponents(IServiceCollection services)
        {
        }

        /// <summary>
        /// Register All Components.
        /// </summary>
        /// <param name="services">services</param>
        private void RegisterComponents(IServiceCollection services)
        {
            this.RegisterCustomComponents(services);
            this.RegisterBusinessLayer(services);
        }

        /// <summary>
        /// Configure AutoMapper Services
        /// </summary>
        /// <param name="services">services</param>
        private void ConfigureAutoMapperServices(IServiceCollection services)
        {
        }

        //protected virtual void IntializeCustomCache(IServiceCollection services)
        //{
        //}
    }
}