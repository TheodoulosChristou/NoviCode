using Microsoft.EntityFrameworkCore;
using NoviCode.NoviCodeDbContext; // 🔍 possible issue here
using NoviCode.Services;
using Scalar.AspNetCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi(); // From Scalar.AspNetCore

builder.Services.AddDbContext<NoviCodeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICurrencyRateService, CurrencyRateService>();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddHostedService<ECBCurrencyBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // Swagger UI or Scalar API docs
    app.MapScalarApiReference("/docs");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
