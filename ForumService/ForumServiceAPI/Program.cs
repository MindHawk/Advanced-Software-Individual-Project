using ForumServiceDAL;
using ForumServiceLogic;
using ForumServiceMessageBus;
using ForumServiceModels.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
bool inMemoryDatabase = Environment.GetEnvironmentVariable("USE_IN_MEMORY_DATABASE") == "false";

// Add services to the container.
if(inMemoryDatabase)
{
    string connectionString = builder.Configuration.GetConnectionString("PostgresConnectionString");
    builder.Services.AddDbContext<ForumContext>(options => options.UseNpgsql(
        connectionString, 
        x => x.MigrationsAssembly("ForumServiceAPI")));
}
else
{
    builder.Services.AddDbContext<ForumContext>(options => options.UseInMemoryDatabase("AccountService"));
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IForumRepository, ForumRepository>();
builder.Services.AddScoped<IForumLogic, ForumLogic>();
builder.Services.AddHostedService<MessageBusListener>();

var app = builder.Build();

app.Logger.LogInformation("Using in memory database is: " + inMemoryDatabase);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Forum Service");
        c.RoutePrefix = string.Empty;  // Set Swagger UI at apps root
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();