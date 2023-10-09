using Microsoft.OpenApi.Models;

namespace apiemail.Configurations;

public class SwaggerConfig
{
    public static void AddSwaggerGen(WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options => {
            options.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization Header - utilizado com Bearer Authentication.\r\n\r\n"
                        + "Digite 'Bearer' [espaço] e então seu token no campo abaixo.\r\n\r\n"
                        + "Exemplo (informar sem as aspas): Bearer 12345abcdef",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                }
            );
            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                }
            );
        });
    }
}