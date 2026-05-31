using Microsoft.EntityFrameworkCore;
using SMT.API.Common;
using SMT.API.Configuration;
using SMT.API.Middleware;
using SMT.Application;
using SMT.Infrastructure;
using SMT.Persistence;
using SMT.Persistence.SMTConfiguration;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddPersistence(configuration);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(configuration);

// Настройка JWT-аутентификации
builder.Services.AddJwtConfig(configuration);

builder.Services.AddAuthorization();

// Регистрация HttpContextAccessor для доступа к текущему пользователю
builder.Services.AddHttpContextAccessor();

// Регистарция RabbitMq
builder.Services.AddMassTransitServices(configuration);

// Настройка CORS
builder.Services.AddCorsConfiguration(configuration); 

// Настройка фильтров
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiResponseFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Инициализация БД
// var provider = builder.Services.BuildServiceProvider();
// var context = provider.GetRequiredService<SMTDBContext>();
// DbInitialize.Initialize(context);

var app = builder.Build();
// ✅ Инициализация БД через ОСНОВНОЙ контейнер
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SMTDBContext>();
    
    // Вариант А: Применить миграции (если используете Code First)
    await context.Database.MigrateAsync();
    
    // Вариант Б: Ваш кастомный DbInitialize (если нужен сидинг)
    // await DbInitialize.InitializeAsync(context);
}

app.UseCors("CorsPolicy");

// Middleware обработки исключений (должен быть первым)
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();