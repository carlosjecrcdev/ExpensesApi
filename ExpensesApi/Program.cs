using ExpensesApi.Filters;
using ExpensesApi.Interfaces;
using ExpensesApi.Middleware;
using ExpensesApi.Models.Context;
using ExpensesApi.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSqlServer<ExpensesContext>(builder.Configuration.GetConnectionString("ExpensesConnection"));
builder.Services.AddScoped<IUserAccountServices,UserAccountServices>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});

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
