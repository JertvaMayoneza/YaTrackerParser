using System.Reflection;
using RabbitMQ.Client;
using YaTrackerParser.Auth;
using YaTrackerParser.Contracts.Interfaces;
using YaTrackerParser.Services;
using YaTrackerParser.Data.Context;
using Microsoft.EntityFrameworkCore;
using YaTrackerParser.Data.Context.Entites;
using YaTrackerParser.Data.Repository;

var builder = WebApplication.CreateBuilder(args);

TokenManager.Initialize(builder.Configuration);

builder.Logging.AddConsole();
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
builder.Logging.AddFilter("Microsoft", LogLevel.Warning);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .EnableSensitiveDataLogging(false)
           .EnableDetailedErrors(false));

//Подключение к кролику из Docker контейнера
//builder.Services.AddSingleton<IConnectionFactory>(new ConnectionFactory
//{
//    HostName = "rabbitmq",
//    UserName = "guest",
//    Password = "guest"
//}
//);

//Подключение к кролику в Docker контейнер из локального запущенного приложения
builder.Services.AddSingleton<IConnectionFactory>(new ConnectionFactory
{
    HostName = "localhost",
    UserName = "guest",
    Password = "guest"
}
);

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<TicketConsumer>();
builder.Services.AddHostedService<TicketConsumer>();

builder.Services.AddScoped<IDatabaseWriterService, DatabaseWriterService>();
builder.Services.AddScoped<ITicketProcessor, TicketProcessor>();
builder.Services.AddScoped<IGetTicketsService, GetTicketsService>();
builder.Services.AddScoped<ITicketFilterService, TicketFilterService>();
builder.Services.AddScoped<IFileWriterService, FileWriterService>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddControllers();

builder.Services.AddScoped<GetTicketsService>(provider =>
    new GetTicketsService(
        provider.GetRequiredService<IHttpClientFactory>(),
        provider.GetRequiredService<IConfiguration>()
    ));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "TicketManager",
        Version = "v1.0",
        Description = "API для работы с тикетами Яндекс Трекера"
    });

    //c.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    //{
    //    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
    //    Description = "Введите ваш API ключ",
    //    Name = "X-Api-Key",
    //    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    //});

    //c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    //{
    //    {
    //        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    //        {
    //            Reference = new Microsoft.OpenApi.Models.OpenApiReference
    //            {
    //                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
    //                Id = "ApiKey"
    //            }
    //        },
    //        new string[] {}
    //    }
    //});

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddHttpClient("YaTrackerClient", client =>
{
    client.BaseAddress = new Uri("https://api.tracker.yandex.net/");
});

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//    app.UseSwagger();
//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TicketManager V1");
//    });
//}

app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TicketManager V1");
});

//app.UseMiddleware<ApiKeyMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
