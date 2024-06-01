using System.Text.Json;
using System.Text.Json.Serialization;
using BulletinBoard.Application.Options;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Application.Services;
using BulletinBoard.Infrastructure.Context;
using BulletinBoard.Infrastructure.Repositories;
using BulletinBoard.WebAPI.Services;
using BulletinBoard.WebAPI.Tools;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        b =>
        {
            b.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<ITenantFactory, TenantFactory>();
builder.Services.AddScoped<ITenant, Tenant>();
builder.Services.AddSingleton<IImageService, ImageService>();
builder.Services.Configure<BulletinsConfigurationOptions>(
    builder.Configuration.GetSection("BulletinsConfigurationOptions"));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/swagger.json", "v1");
        options.DocExpansion(DocExpansion.List);
    });
}
else
{
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseCors("AllowAllOrigins");
app.MapControllers();
app.UseStaticFiles();
app.UseExceptionHandler();

app.Run();