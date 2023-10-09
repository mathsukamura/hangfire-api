using Hangfire;
using Hangfire.PostgreSql;

namespace hangfireapi.Configurations.Hangfire;

public abstract class HangFireConfig
{
    public static void ConfigureHangFire(WebApplicationBuilder builder)
    {
        var services = builder.Services;
        
        services.AddHangfire(x => x.UsePostgreSqlStorage
            (builder.Configuration.GetConnectionString("Default")));
        
        services.AddHangfireServer();
    }
}