using FluentValidation;
using LimbusIdentityApi.Data;
using LimbusIdentityApi.Endpoints;
using LimbusIdentityApi.Repositories;
using LimbusIdentityApi.Validations;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

var connectionString = builder.Configuration.GetConnectionString("IdentityDbConnectionString");
builder.Services.AddDbContext<IdentityDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPassiveRepository, PassiveRepository>();
builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped<IIdentityRepository, IdentityRepository>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateSkillDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreatePassiveDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateIdentityDtoValidator>();

var app = builder.Build();


app.MapPassiveEndpoints();
app.MapSkillEndpoints();
app.MapIdentityEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();


