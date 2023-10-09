using Hangfire;

namespace hangfireapi.Configurations.Hangfire;

public class HangFireDependecy
{
    private readonly IServiceProvider _serviceProvider;

    public HangFireDependecy(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void HangFireDependency()
    {
        GlobalConfiguration.Configuration.UseActivator(new HangfireActivator(_serviceProvider));
    }
}