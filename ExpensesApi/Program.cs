using ExpensesApi.Interfaces;
using ExpensesApi.Middleware;
using ExpensesApi.Models.Context;
using ExpensesApi.Models.Mapping;
using ExpensesApi.Services;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSqlServer<ExpensesContext>(builder.Configuration.GetConnectionString("ExpensesConnection"));
builder.Services.AddScoped<IUserAccountServices,UserAccountServices>();
builder.Services.AddScoped<IBudgetServices, BudgetServices>();
builder.Services.AddScoped<ICategoryServices, CategoryServices>();
builder.Services.AddScoped<IExpenseServices, ExpenseServices>();

builder.Services.AddAutoMapper(typeof(UserAccountProfile));
builder.Services.AddAutoMapper(typeof(BudgetProfile));
builder.Services.AddAutoMapper(typeof(CategoryProfile));
builder.Services.AddAutoMapper(typeof(ExpenseServices));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
