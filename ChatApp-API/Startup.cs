using ChatApp.API.Data.Auth;
using ChatApp.API.JWT;
using ChatApp.API.JWT.Chaves;
using ChatApp.API.JWT.Handlers;
using ChatApp.API.Models;
using ChatApp.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<DataContext>(dC => dC.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddCors();
            services.AddScoped<IAuthRepository<User>, AuthRepository>();
            services.AddSingleton<PrivateRSA>();
            services.AddSingleton<PublicRSA>();
            services.Configure<JwtSettings>(Configuration.GetSection("jwt"));
            services.AddSingleton<IJwtHandler, JwtHandler>();

            var serviceProvider = services.BuildServiceProvider();
            var jwtHandler = serviceProvider.GetService<IJwtHandler>();
            
            services.AddAuthentication(x => 
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(j =>
            {
                j.TokenValidationParameters = jwtHandler.Parameters;
                j.SaveToken = true;
                j.RequireHttpsMetadata = false;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Use(async (ctx, next) =>
            {
                await next();
                if (ctx.Response.StatusCode == 204)
                {
                    ctx.Response.ContentLength = 0;
                }
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var jwtHandler = app.ApplicationServices.GetService<IJwtHandler>();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();
        }
    }
}
