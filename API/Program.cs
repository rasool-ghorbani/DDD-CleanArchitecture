using Persistence;
using Application;
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.ConfigureAppConfiguration(
                (x, config) =>
                {
                    config.AddJsonFile("appsettings.json").AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json");
                });

    // Add services to the container.

    builder.Services.AddPersistence(builder.Configuration);
  
    builder.Services.AddApplication();

    builder.Services.AddControllers();

    builder.Services.AddOpenApi();

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.UseSwagger();

    app.UseSwaggerUI();

    app.Run();
}
catch (Exception)
{

    throw;
}