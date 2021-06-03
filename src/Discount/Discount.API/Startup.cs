using Discount.API.Helpers;
using Discount.API.Repositories;
using Discount.API.Repositories.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Discount.API
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
                app.UseSwaggerUI(swaggerUIOptions => swaggerUIOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "Discount.API v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(
                swaggerGenOptions => swaggerGenOptions.SwaggerDoc(
                    Version1,
                    new OpenApiInfo
                    {
                        Title = "Discount.API",
                        Version = Version1
                    }));

            services.AddSingleton(_ => Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>());
            services.AddScoped<IDiscountRepository, DiscountRepository>();
        }
    }
}