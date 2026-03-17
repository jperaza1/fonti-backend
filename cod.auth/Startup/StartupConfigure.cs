using System;
using System.Linq;
using System.Text;
using cod.domain.Authorization;
using cod.persistence.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz.Spi;
using Quartz;
using Quartz.Impl;

namespace cod.auth
{
    public partial class Startup
    {
        public void ConfigureJobs(IServiceCollection services)
        {
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

        }

        private void DatabaseConfigure(IServiceCollection services)
        {
            string valueConnection = Configuration.GetConnectionString("Default");
            services.AddDbContext<Repository>(optionsAction => optionsAction.UseSqlServer(valueConnection));
        }
        private void CorsConfigure(IServiceCollection services)
        {
            var hostWebApplication = Configuration.GetValue<string>("App:WebApplication");
            services.AddCors(options => options.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder.AllowAnyMethod().AllowAnyHeader()
                        .WithOrigins(hostWebApplication.Split(",", StringSplitOptions.RemoveEmptyEntries).ToArray())
                        .AllowCredentials();
                }));
        }
        private void IdentityConfigure(IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<Repository>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 0;
            });

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = Configuration["JWT:ValidAudience"],
                        ValidIssuer = Configuration["JWT:ValidIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"])),
                    };
                });
        }
        private void SwaggerConfigure(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                var groupName = "v2";

                options.SwaggerDoc(groupName, new OpenApiInfo
                {
                    Title = $"Authorize {groupName}",
                    Version = groupName,
                    Description = "Auth",
                    Contact = new OpenApiContact
                    {
                        Name = "Authorize",
                        Email = string.Empty,
                        Url = new Uri("http://sostruckingsoftware.com/"),
                    }
                });
            });
        }
    }
}
