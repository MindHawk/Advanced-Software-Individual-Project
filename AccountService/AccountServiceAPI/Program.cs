using AccountServiceDAL;
using AccountServiceLogic;
using AccountServiceMessageBus;
using AccountServiceModels.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connectionString = builder.Configuration.GetConnectionString("PostgresConnectionString");
builder.Services.AddDbContext<AccountContext>(options => options.UseNpgsql(
    connectionString, 
    x => x.MigrationsAssembly("AccountServiceAPI")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountLogic, AccountLogic>();
builder.Services.AddHostedService<MessageBusListener>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Account Service");
        c.RoutePrefix = string.Empty;  // Set Swagger UI at apps root
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();