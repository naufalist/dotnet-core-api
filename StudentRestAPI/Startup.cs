using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using StudentRestAPI.Models;
using StudentRestAPI.Redis;
using StudentRestAPI.Services;
using StudentRestAPI.StudentData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRestAPI
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
            /*
             * DbContext
             */
            //services.AddDbContextPool<StudentDbContext>(options => 
            //    options.UseMySQL(Configuration.GetConnectionString("MySqlDb"))
            //);
            services.AddDbContextPool<AppDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("PostgresDb"))
            );

            services.AddControllers();

            /*
             * MockStudentData || SqlStudentData
             */
            //services.AddSingleton<IStudentData, MockStudentData>();
            //services.AddScoped<IStudentData, SqlStudentData>();
            services.AddTransient<StudentService>();
            services.AddTransient<ProjectService>();
            services.AddTransient<SupervisorService>();

            /*
             * Redis
             */
            services.AddSingleton<IConnectionMultiplexer>(x =>
                ConnectionMultiplexer.Connect(Configuration.GetValue<string>("RedisConnection")
            ));
            services.AddSingleton<IRedisCache, RedisCacheService>();

            /*
             * Swagger
             */
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "Student REST API v1",
                        Description = "Student REST API for showing Swagger",
                        Version = "v1"
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Student REST API");
            });
        }
    }
}
