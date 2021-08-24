using System;
using System.Text;
using API._Data;
using API._Helpers;
using API._Interfaces;
using API._Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
            services.AddScoped<ITokenService, TokenService>();
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            services.AddScoped<IPostsRepo, PostsRepo>();
            services.AddScoped<IUserRepo, UserRepo>();
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