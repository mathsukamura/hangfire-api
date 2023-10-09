using hangfireapi.Data;
using Microsoft.EntityFrameworkCore;

namespace hangfireapi.Configurations;

public static class EfConfig
{

    public static void AddDbContext(WebApplicationBuilder builder)
    {
        var services = builder.Services;
        services.AddDbContext<HangContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
        }, ServiceLifetime.Scoped);
    }
}