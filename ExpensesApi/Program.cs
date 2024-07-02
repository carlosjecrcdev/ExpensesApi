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
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
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

builder.Services.AddCors(op =>
{
    op.AddDefaultPolicy(app =>
    {
        app.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
