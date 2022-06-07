using API.Di;
using API.Middleware;
using API.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ServerLib.Database.Mysql.Context;
using ServerLib.Database.Mysql.Dao;
using Grpc.Core;
using API.Grpc;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
                options.OperationFilter<AddRequiredHeaderParameter>();
                options.CustomSchemaIds(s => s.FullName.Replace("+", "."));
            });

            services.AddHttpContextAccessor();

            services.AddGrpc(options =>
            {
                options.EnableDetailedErrors = true;
            });

            services.AddDbContext<MysqlDbContext>();

            services.AddTransient<PlayerDi>();

            services.AddTransient<DaoContext>();

            services.AddTransient<TotalDi>();

            services.AddTransient<ChatRoomService>();
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
                endpoints.MapGrpcService<ChatService>();
                endpoints.MapControllers();
            });

        }
    }
}
