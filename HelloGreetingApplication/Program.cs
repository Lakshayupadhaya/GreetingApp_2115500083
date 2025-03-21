using BusinessLayer.Email;
using BusinessLayer.Interface;
using BusinessLayer.RabbitMQ;
using BusinessLayer.Redis;
using BusinessLayer.Service;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RepositoryLayer.Context;
using RepositoryLayer.Helper;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();




//Logger
builder.Logging.ClearProviders();//clearing all the pre defined outputs of logger
builder.Logging.AddConsole();//logs to the console
builder.Logging.AddDebug();//loogs to the debug console window

builder.Services.AddControllers();
//Adding services of business layer
builder.Services.AddScoped<IGreetingBL, GreetingBL>();
//Adding services of repository layer
builder.Services.AddScoped<IGreetingRL, GreetingRL>();
builder.Services.AddScoped<IUserBL, UserBL>();
builder.Services.AddScoped<IUserRL, UserRL>();
builder.Services.AddScoped<Jwt>();
builder.Services.AddScoped<EmailHelper>();

var redis = ConnectionMultiplexer.Connect(new ConfigurationOptions
{
    EndPoints = { "localhost:6379" },
    AbortOnConnectFail = false,
    ConnectTimeout = 15000,   // Increased connection timeout
    SyncTimeout = 10000,      // Increased synchronous timeout
    AsyncTimeout = 10000,
    KeepAlive = 180           // Keep connection alive
});
builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();

//  Configure RabbitMQ
builder.Services.AddSingleton<IConnectionFactory>(sp => new ConnectionFactory()
{
    HostName = "localhost",
    DispatchConsumersAsync = true // Allow async processing
});

//  Register RabbitMQ Connection
builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = sp.GetRequiredService<IConnectionFactory>();
    return factory.CreateConnection(); // Create and return the RabbitMQ connection
});

//  Register RabbitMQ Channel
builder.Services.AddSingleton<IModel>(sp =>
{
    var connection = sp.GetRequiredService<IConnection>();
    return connection.CreateModel(); // Create and return the RabbitMQ channel
});

//  Register Producer and Consumer
builder.Services.AddSingleton<Producer>();
builder.Services.AddHostedService<Consumer>();


var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
builder.Services.AddDbContext<
    GreetingAppContext>(options =>
    options.UseSqlServer(connectionString));

//Add swagger to the container
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();//using Swagger
    app.UseSwaggerUI();// responsible for colorfullness
}

// Add Global Exception Middleware

app.UseMiddleware<MiddleWare.MiddleWare.GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
