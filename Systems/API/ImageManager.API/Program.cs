using ImageManager.API.Configuration;
using ImageManager.API;
using ImageManager.Services.Settings;
using ImageManager.Context;

var mainSettings = ImageManager.Common.Settings.Settings.Load<MainSettings>("Main");
var logSettings = ImageManager.Common.Settings.Settings.Load<LogSettings>("Log");
var swaggerSettings = ImageManager.Common.Settings.Settings.Load<SwaggerSettings>("Swagger");

var builder = WebApplication.CreateBuilder(args);

builder.AddAppLogger(mainSettings, logSettings);

var services = builder.Services;

services.AddAppController();

services.AddHttpContextAccessor();

services.AddAppDbContext();

services.AddAppHealthChecks();

services.AddAppSwagger(swaggerSettings);

services.RegisterServicesAndModels();

services.AddAppCors();


var app = builder.Build();

app.UseAppCors();

app.UseHttpsRedirection();

app.UseAppController();
app.MapControllers();

app.UseAppHealthChecks();

app.UseAppSwagger();

DbInitializer.Execute(app.Services);

app.Run();