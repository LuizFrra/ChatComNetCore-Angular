using DatingApp.API.Data.Auth;
using DatingApp.API.JWT;
using DatingApp.API.JWT.Chaves;
using DatingApp.API.JWT.Handlers;
using DatingApp.API.Models;
using DatingApp.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp
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
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var jwtHandler = app.ApplicationServices.GetService<IJwtHandler>();

            app.UseCors(x => x.AllowAnyOrigin());

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();
        }
    }
}
