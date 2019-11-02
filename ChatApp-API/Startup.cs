using ChatApp.API.Data.Auth;
using ChatApp.API.JWT;
using ChatApp.API.JWT.Chaves;
using ChatApp.API.JWT.Handlers;
using ChatApp.API.Models;
using ChatApp.Data;
using DatingApp.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

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
            services.AddSingleton<Sockets>();
            var serviceProvider = services.BuildServiceProvider();
            var jwtHandler = serviceProvider.GetService<IJwtHandler>();
            
            services.AddAuthentication(x => 
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = jwtHandler.Parameters;
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;

                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        if(context.Request.Headers.ContainsKey("sec-websocket-protocol"))
                        {
                            StringValues protocolsValues;
                            context.Request.Headers.TryGetValue("sec-websocket-protocol", out protocolsValues);
                            var protocols = protocolsValues.ToString().Split(',');

                            if(protocols.Length == 2)
                            {
                                if (protocols[0] == "jwt")
                                {
                                    context.Request.Headers["sec-websocket-protocol"] = protocols[0];
                                    context.Token = protocols[1].Trim();
                                }
                            }
                        }
                        return Task.CompletedTask;
                    }
                };
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

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 1000
            };

            var jwtHandler = app.ApplicationServices.GetService<IJwtHandler>();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();

            app.UseWebSockets(webSocketOptions);

            app.UseMvcWithDefaultRoute();
        }
    }
}
