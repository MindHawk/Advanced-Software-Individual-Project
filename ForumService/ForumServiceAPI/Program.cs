using System.Text;
using ForumServiceAPI.Attributes;
using ForumServiceDAL;
using ForumServiceLogic;
using ForumServiceMessageBus;
using ForumServiceMessageBusProducer;
using ForumServiceModels.Interfaces;
using Google.Apis.Auth.AspNetCore3;
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
            x => x.MigrationsAssembly("ForumServiceAPI")));
        break;
    case ("kubernetes"):
        connectionString = builder.Configuration.GetConnectionString("CloudDBConnectionString") ?? string.Empty;
        builder.Services.AddDbContext<ForumContext>(options => options.UseSqlServer(
            connectionString, 
            x => x.MigrationsAssembly("ForumServiceAPI")));
        break;
    default:
        builder.Services.AddDbContext<ForumContext>(options => options.UseInMemoryDatabase("ForumService"));
        break;
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(b =>
    {
        b.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddScoped<IForumRepository, ForumRepository>();
builder.Services.AddScoped<IForumLogic, ForumLogic>();
builder.Services.AddScoped<AuthorizeGoogleTokenAttribute>();
builder.Services.AddScoped<ForumMessageBusProducer>();
builder.Services.AddHostedService<ForumMessageBusConsumer>();

var app = builder.Build();

app.Logger.LogInformation("Running environment is: {RunningEnvironment}", runningEnvironment);

switch (runningEnvironment)
{
    case ("docker"):
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<ForumContext>();
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
        break;
}

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

app.UseCors();

app.MapControllers();

app.Run();