using ExpensesApi.Interfaces;
using ExpensesApi.Middleware;
using ExpensesApi.Models.Context;
using ExpensesApi.Models.Mapping;
using ExpensesApi.Services;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;
using ExpensesApi.Models.Dtos;
using ExpensesApi.Models.Validations;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSqlServer<ExpensesContext>(builder.Configuration.GetConnectionString("ExpensesConnection"));
builder.Services.AddScoped<IUserAccountServices,UserAccountServices>();
builder.Services.AddScoped<IBudgetServices, BudgetServices>();
builder.Services.AddScoped<ICategoryServices, CategoryServices>();
builder.Services.AddScoped<IExpenseServices, ExpenseServices>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IValidator<UserAccountDto>, UserAccountValidator>();
builder.Services.AddScoped<IValidator<ExpenseDto>,ExpenseValidator>();
builder.Services.AddScoped<IValidator<CategoryDto>, CategoryValidator>();
builder.Services.AddScoped<IValidator<BudgetDto>, BudgetValidator>();

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
