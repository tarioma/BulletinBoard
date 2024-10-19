using Bulletins.Application.Extensions;
using Bulletins.Dal;
using Bulletins.Extensions;
using Bulletins.Tools;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Postgres");

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddControllersWithJsonOptions();
builder.Services.AddCors(options => options.AddPolicy("AllowAllOrigins", b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connectionString));
builder.Services.ApplyMigrations();
builder.Services.AddRepositories();
builder.Services.AddUseCases();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();
app.UseCors("AllowAllOrigins");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
    app.UseHttpsRedirection();
}

// Swagger доступен и в Development и в Production
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.UseExceptionHandler();

app.Run();