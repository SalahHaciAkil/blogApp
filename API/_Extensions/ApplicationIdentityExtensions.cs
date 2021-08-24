using System.Text;
using API._Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace API._Extensions
{
    public static class ApplicationIdentityExtensions
    {
        public static IServiceCollection AddApplicationIdentityServices(this IServiceCollection services, IConfiguration config)
        {

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(options =>
                        {
                            options.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuer = false,
                                ValidateAudience = false,
                                IssuerSigningKey = new SymmetricSecurityKey
                                (Encoding.UTF8.GetBytes(config["TokenKey"]))
                            };
                        });

            return services;

        }

    }
}