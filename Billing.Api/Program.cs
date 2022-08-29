using Billing.Api;
using Billing.Api.ExtensionMethods;
using Billing.Infrastructure.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Billing API",
        Description = "An ASP.NET Core Web API for managing Billings and Orders"
    });
});
builder.Services.AddControllers();
builder.Services.AddDbContext<OrdersDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Orders")));

builder.Services.RegisterAppServices();

var app = builder.Build();

await DbMigrator.Migrate(app);

app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();
app.Run();