using API.Middleware;
using API.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using ServerLib.Database.Mysql.Context;
using System;
using ServerLib.Database.Mysql.Dao;
using API.Di;

namespace API
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

            services.AddControllers();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
                options.OperationFilter<AddRequiredHeaderParameter>();
                options.CustomSchemaIds(s => s.FullName.Replace("+", "."));
            });

            services.AddHttpContextAccessor();

            /*services.AddDbContext<MysqlDbContext>(
                options => options.UseMySql(Configuration.GetConnectionString("Portfolio"), new MySqlServerVersion(new Version(8, 0, 28)))
            );*/

            services.AddDbContext<MysqlDbContext>();

            services.AddTransient<PlayerDi>();

            services.AddTransient<DaoContext>();

            services.AddTransient<TotalDi>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            app.UseMiddleware<ResponseMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
