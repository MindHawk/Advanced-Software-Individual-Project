using ForumServiceDAL;
using ForumServiceLogic;
using ForumServiceMessageBus;
using ForumServiceModels.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connectionString = builder.Configuration.GetConnectionString("PostgresConnectionString");
builder.Services.AddDbContext<ForumContext>(options => options.UseNpgsql(
    connectionString, 
    x => x.MigrationsAssembly("HotelService")));

builder.Services.AddScoped<IForumRepository, ForumRepository>();
builder.Services.AddScoped<IForumLogic, ForumLogic>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<PLACEHOLDER_MessageBusEvent>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My service");
        c.RoutePrefix = string.Empty;  // Set Swagger UI at apps root
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();