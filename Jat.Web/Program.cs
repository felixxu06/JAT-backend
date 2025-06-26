using Jat.Entities;
using Jat.IRepositories;
using Jat.IServices;
using Jat.Repositories;
using Jat.Services;
using Jat.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Register repositories
builder.Services.AddScoped<IApplicantRepository, ApplicantRepository>();
builder.Services.AddScoped<IJobRepository, JobRepository>();

// Register services
builder.Services.AddScoped<IApplicantService, ApplicantService>();
builder.Services.AddScoped<IJobService, JobService>();

// Register UserContext
builder.Services.AddScoped<UserContext>();

// Register InMemoryDatabase as singleton
builder.Services.AddSingleton<InMemoryDatabase>();

// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
Console.WriteLine($"Allowed Origins: {string.Join(", ", allowedOrigins ?? Array.Empty<string>())}");
builder.Services.AddCors(options =>
{
    options.AddPolicy("ConfiguredCors", policy =>
    {
        policy.WithOrigins(allowedOrigins ?? Array.Empty<string>())
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

// Add UserContextMiddleware to the pipeline
app.UseMiddleware<UserContextMiddleware>();

// Use CORS
app.UseCors("ConfiguredCors");

app.UseAuthorization();

app.MapControllers();

app.Run();
