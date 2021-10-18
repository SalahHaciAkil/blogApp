using System;
using System.Text;
using API._Data;
using API._Helpers;
using API._Interfaces;
using API._Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace API._Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.Configure<SMTP>(config.GetSection("SMTP"));
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<LogUserActivity>();
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddDbContext<DataContext>(options =>
           {
               var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

               string connStr;

                // Depending on if in development or production, use either Heroku-provided
                // connection string, or development connection string from env var.
                if (env == "Development")
               {
                    // Use connection string from file.
                    connStr = config.GetConnectionString("DefaultConnection");
               }
               else
               {
                    // Use connection string provided at runtime by Heroku.
                    var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

                    // Parse connection URL to connection string for Npgsql
                    connUrl = connUrl.Replace("postgres://", string.Empty);
                   var pgUserPass = connUrl.Split("@")[0];
                   var pgHostPortDb = connUrl.Split("@")[1];
                   var pgHostPort = pgHostPortDb.Split("/")[0];
                   var pgDb = pgHostPortDb.Split("/")[1];
                   var pgUser = pgUserPass.Split(":")[0];
                   var pgPass = pgUserPass.Split(":")[1];
                   var pgHost = pgHostPort.Split(":")[0];
                   var pgPort = pgHostPort.Split(":")[1];

                   connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb}; SSL Mode=Require; Trust Server Certificate=true";

               }

                // Whether the connection string came from the local development configuration file
                // or from the environment variable from Heroku, use it to set up your DbContext.
                options.UseNpgsql(connStr);
           });
            services.AddSwaggerGen(c =>
           {
               c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
               var jwtSecurityScheme = new OpenApiSecurityScheme
               {
                   Scheme = "bearer",
                   BearerFormat = "JWT",
                   Name = "JWT Authentication",
                   In = ParameterLocation.Header,
                   Type = SecuritySchemeType.Http,
                   Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                   Reference = new OpenApiReference
                   {
                       Id = JwtBearerDefaults.AuthenticationScheme,
                       Type = ReferenceType.SecurityScheme
                   }
               };

               c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

               c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    { jwtSecurityScheme, Array.Empty<string>() }
               });

           });
            return services;

        }

    }
}