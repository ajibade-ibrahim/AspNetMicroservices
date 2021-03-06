using System;
using Basket.API.Repositories.Contracts;
using Basket.API.Services.Contracts;
using Basket.API.Services.gRPC;
using Discount.gRPC.Protos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Basket.API
{
    public class Startup
    {
        private const string Version1 = "v1";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.API v1"));
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddStackExchangeRedisCache(
                redisOptions =>
                {
                    var cacheSettings = Configuration.GetSection("CacheSettings").Get<CacheSettings>();
                    redisOptions.Configuration = cacheSettings.ConnectionString;
                });

            services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(
                options => options.Address = new Uri(Configuration.GetValue<string>("GrpcSettings:DiscountServiceUrl")));

            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IDiscountService, DiscountService>();

            services.AddControllers();
            services.AddSwaggerGen(
                swaggerGenOptions => swaggerGenOptions.SwaggerDoc(
                    Version1,
                    new OpenApiInfo
                    {
                        Title = "Basket.API",
                        Version = Version1
                    }));
        }
    }
}