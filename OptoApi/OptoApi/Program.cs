﻿using OptoApi.Middlewares;
using OptoApi.Options;
using OptoApi.Repositories;
using OptoApi.Services;
using OptoApi.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<DatabaseOptions>(
    builder.Configuration.GetSection(DatabaseOptions.SectionName));

builder.Services.AddTransient<IProductsService, ProductsService>();
builder.Services.AddTransient<ProductValidator>();
builder.Services.AddTransient<IEmployeesService, EmployeesService>();
builder.Services.AddTransient<EmployeeValidator>();
builder.Services.AddTransient<IBranchesService, BranchesService>();
builder.Services.AddTransient<BranchValidator>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IEmployeeRepository,EmployeeRepository>();
builder.Services.AddTransient<IBranchRepository,BranchRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<LoggingMiddleware>();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();

