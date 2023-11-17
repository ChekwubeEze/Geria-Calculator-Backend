using Geria.Core.Application.Configurations;
using Geria.Core.Auth;
using Geria.Core.Auth.Token;
using Geria.Core.Infrastructure.Services.Calculatormanagement;
using Geria.Core.Infrastructure.Services.UserManagement;
using Geria.Data.Domain.Model.Calculator.Repositories;
using Geria.Data.Domain.Model.UserManagement.Entities;
using Geria.Data.Domain.Model.UserManagement.Repositories;
using Geria.Infrastructure;
using Geria.Infrastructure.Infrastructure;
using Geria.Infrastructure.Repositories;
using GeriaCalculatorApp.Extentions;
using GeriaCalculatorApp.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var Configuration = builder.Configuration;
var connectionString = Configuration.GetConnectionString("Geria");

builder.Services.AddCustomAuthentication(Configuration);

builder.Services.AddCors(policy =>
{
    policy.AddPolicy("poc", p =>
    {
        p.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin();
        //.AllowCredentials();
    });
});


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IInputDataRepository, InputDataRepository >();

builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IUserhelper, Userhelper>();
builder.Services.AddScoped<ICalculatorServices, CalculatorServices>();
builder.Services.AddScoped<IUserManagmentServices, UserManagmentServices>();
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IJwtHandler, JwtHandler>();
builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();


builder.Services.Configure<JwtOptions>(Configuration.GetSection(nameof(JwtOptions)));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Geria-API", Version = "v1", });
    c.OperationFilter<AuthenticationOperationFilter>();
});

builder.Services.AddScoped<IDatabaseFactory>(t => new DatabaseFactory(connectionString));


builder.Services.AddDbContext<GeriaCalculatorContext>(options => options.UseSqlServer((Configuration.GetConnectionString("Geria")),
             v => v.MigrationsAssembly(typeof(GeriaCalculatorContext).GetTypeInfo().Assembly.GetName().Name)));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("poc");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
