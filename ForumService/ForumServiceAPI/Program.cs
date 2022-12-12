using ForumServiceDAL;
using ForumServiceLogic;
using ForumServiceMessageBus;
using ForumServiceModels.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
string? runningEnvironment = Environment.GetEnvironmentVariable("HOSTED_ENVIRONMENT");

// Switch database depending on where we're running.
// While running locally or debugging, an in-memory database is used.
// When running (locally) in docker, a dockerized postgres database is used.
// When running in kubernetes, a cloud database is used.
switch (runningEnvironment)
{
    case ("docker"):
        string connectionString = builder.Configuration.GetConnectionString("PostgresConnectionString");
        builder.Services.AddDbContext<ForumContext>(options => options.UseNpgsql(
            connectionString, 
            x => x.MigrationsAssembly("AccountServiceAPI")));
        break;
    case ("kubernetes"):
        builder.Services.AddDbContext<ForumContext>(options => options.UseInMemoryDatabase("AccountService"));
        break;
    default:
        builder.Services.AddDbContext<ForumContext>(options => options.UseInMemoryDatabase("AccountService"));
        break;
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IForumRepository, ForumRepository>();
builder.Services.AddScoped<IForumLogic, ForumLogic>();
builder.Services.AddHostedService<MessageBusListener>();

var app = builder.Build();

app.Logger.LogInformation("Running environment is: {RunningEnvironment}", runningEnvironment);

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