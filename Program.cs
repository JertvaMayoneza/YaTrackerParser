var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<YaTrackerParser.Auth.TokenManager>();
builder.Services.AddScoped<YaTrackerParser.Services.GetTicketsService>();
builder.Services.AddScoped<YaTrackerParser.Services.TicketProcessor>();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<YaTrackerParser.Services.TicketFilterService>();
builder.Services.AddScoped<YaTrackerParser.Services.FileWriterService>();


// Corrected base address for Yandex Tracker API
builder.Services.AddHttpClient("YaTrackerClient", client =>
{
    client.BaseAddress = new Uri("https://api.tracker.yandex.net/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
