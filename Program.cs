using Event.Database;
using Event.Extension;
using Event.Middleware;
using Event.Service;
using Event.Utils;
using HashidsNet;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var services = builder.Services;

// Add services to the container.
services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerBearerGen();
services.AddJwtBearerAuthentication(config);

services.AddDbContext<EventDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(EventDbContext))!));

services.AddScoped<OrganisationService>();
services.AddScoped<EventService>();
services.AddScoped<AuthService>();
services.AddScoped<TeamService>();

services.AddTransient<ExceptionMiddleware>();
services.AddScoped<IHashids>(_ => new Hashids(
    config.GetSection("HashIds:Token").Value,
    config.GetSection("HashIds").GetValue<int>("MinLength")
));
services.AddScoped<IIdResolver, HashIdResolver>();
services.AddScoped<IPasswordHasher, PasswordHasher>();

var app = builder.Build();
app.AddExceptionHandling();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();