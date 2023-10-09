
using apiemail.Configurations;
using Hangfire;
using hangfireapi.Configurations;
using hangfireapi.Configurations.Hangfire;
using hangfireapi.Data;
using hangfireapi.Infra.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

HangFireConfig.ConfigureHangFire(builder);

EfConfig.AddDbContext(builder);

DependencyInjection.ConfigureInterfaces(builder);

JwtConfig.AddAuthentication(builder);

SwaggerConfig.AddSwaggerGen(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHangfireDashboard();

app.UseHangfireServer();

var serviceProvider = app.Services.GetRequiredService<IServiceProvider>();

GlobalConfiguration.Configuration.UseActivator(new HangfireActivator(serviceProvider));

app.UseMiddleware<MiddlewareAuthentication>();

app.UseMiddleware<MiddlewareAutorization>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();