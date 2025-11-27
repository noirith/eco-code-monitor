using ErroLoggerAPI.Models;
using ErroLoggerAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurações MongoDB
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Registrar serviços NOVOS
builder.Services.AddSingleton<CalculadoraAmbientalService>();
builder.Services.AddSingleton<MetricaAmbientalService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "EcoCode Monitor API",
        Version = "v1",
        Description = "API para monitoramento de impacto ambiental de aplicações",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Seu Nome",
            Email = "seu@email.com"
        }
    });
});

// Configurar CORS (importante para o Electron Dashboard)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();