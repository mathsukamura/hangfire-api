using apiemail.Service.Access;
using apiemail.Service.AWSService;
using apiemail.Service.Hash;
using hangfireapi.Configurations.Hangfire;
using hangfireapi.Service.Access;
using hangfireapi.Service.CreateUser;
using hangfireapi.Service.Email;
using hangfireapi.Service.ForgotPassword;

namespace hangfireapi.Configurations;

public static class DependencyInjection
{
    public static void ConfigureInterfaces(WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddScoped<ISendEmailService, SendEmailService>();
        services.AddScoped<IHashService, HashService>();
        services.AddScoped<IUserservice, UserService>();
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IForgotPasswordService, ForgotPasswordService >();
        services.AddScoped<IAwsService, AwsService>();
        services.AddScoped<ISendMailService, SendMailService>();
        services.AddSingleton<HangFireDependecy>();
    }
}