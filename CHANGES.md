# SMT.API - Журнал изменений и рекомендации

## ✅ Выполненные улучшения

### 1. Безопасность

#### JWT-аутентификация
- ✅ Добавлена настройка JWT bearer-аутентификации в `Program.cs`
- ✅ Добавлен `UseAuthentication()` перед `UseAuthorization()`
- ✅ Настроена валидация токенов (issuer, audience, lifetime, signing key)
- ✅ Добавлена поддержка JWT в Swagger UI

#### Обработка исключений
- ✅ Создан глобальный middleware `ExceptionHandlingMiddleware`
- ✅ Централизованная обработка `UnauthorizedException`, `NotFoundExceptions`, `ValidationException`
- ✅ Консистентные ответы об ошибках в формате JSON

#### Валидация входных данных
- ✅ Добавлен FluentValidation
- ✅ Созданы валидаторы для:
  - `LoginUserCommand` (имя: 3-50 символов, пароль: 6+ символов)
  - `RegisterUserCommand` (имя: 3-50 символов, пароль: 8+ символов с заглавными, строчными и цифрами)
  - `UpdateTableCommand` (название таблицы, данные года)
  - `RefreshTokenCommand` (refresh токен)
- ✅ Добавлен `ValidationBehavior` для автоматической валидации через MediatR

#### Refresh Token
- ✅ Реализован endpoint `/api/user/refresh-token`
- ✅ Проверка срока действия refresh токена
- ✅ Генерация новой пары access/refresh токенов

### 2. Структура проекта

#### Исправление опечаток
- ✅ `GetYaerById` → `GetYearById`
- ✅ `TotalForQuartet` → `TotalForQuarter`
- ✅ Удалён мёртвый код в `AssemblyMappingProfile`

#### Реализация недостающего функционала
- ✅ Реализован `UpdateTableHandler.Handle()`
- ✅ Исправлен `UnauthorizedException` (ранее выбрасывал `NotImplementedException`)

### 3. Конфигурация

#### Удаление хардкода
- ✅ Удалены хардкод пароля БД и JWT секрета из `appsettings.Development.json`
- ✅ Обновлён `.env.example` с безопасными значениями по умолчанию
- ✅ Добавлены переменные окружения для чувствительных данных

#### Docker Compose
- ✅ Добавлен сервис PostgreSQL
- ✅ Настроена сеть между сервисами
- ✅ Добавлен health check для БД
- ✅ Добавлены volumes для сохранения данных
- ✅ Настроены переменные окружения для контейнеров

---

## ⚠️ Rate Limiting (удалено)

Пакет `AspNetCoreRateLimit` был удалён из-за несовместимости API. 

### Альтернативные решения для rate limiting:

#### Вариант 1: Встроенный .NET 8 Rate Limiter
```csharp
// В Program.cs
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
        httpContext => RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.Identity?.Name ?? "anonymous",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            }));
    
    // Отдельное правило для login
    options.AddPolicy("LoginLimit", httpContext =>
        httpContext.Request.Path == "/api/user/login" && httpContext.Request.Method == "POST"
        ? RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1)
            })
        : null);
});

app.UseRateLimiter();
```

#### Вариант 2: Middleware для login endpoint
Создать простой middleware в `/SMT.API/Middleware/RateLimitMiddleware.cs`:
```csharp
public class RateLimitMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ConcurrentDictionary<string, (int count, DateTime resetTime)> _loginAttempts = new();

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == "/api/user/login" && context.Request.Method == "POST")
        {
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var now = DateTime.UtcNow;
            
            if (!_loginAttempts.TryGetValue(ip, out var attempt) || attempt.resetTime < now)
            {
                attempt = (0, now.AddMinutes(1));
                _loginAttempts[ip] = attempt;
            }
            
            if (attempt.count >= 5)
            {
                context.Response.StatusCode = 429;
                await context.Response.WriteAsJsonAsync(new { message = "Слишком много попыток входа" });
                return;
            }
            
            attempt.count++;
            _loginAttempts[ip] = attempt;
        }
        
        await _next(context);
    }
}
```

---

## 📋 Оставшиеся предупреждения компиляции

### Предупреждения (не критично):
1. **AutoMapper 14.0.0** - уязвимость безопасности (GHSA-rvv3-g6hj-g44x). Рекомендуется обновить до 15.x
2. **CS8604** - возможный null для SecretKey в JwtProvider (решается проверкой конфигурации)
3. **CS0168** - неиспользуемая переменная `ex` в контроллерах (можно удалить или добавить логирование)
4. **ASP0000** - `BuildServiceProvider` в Program.cs (лучше использовать инициализацию БД через hosted service)

---

## 🚀 Запуск проекта

### Через Docker Compose:
```bash
cd /Users/admin/Documents/Диплом/SMT.API

# Создать .env файл
cp .env.example .env

# Запустить
docker-compose up --build
```

### Локально:
```bash
# Установить секреты
dotnet user-secrets set "JwtOptions:SecretKey" "ваш-64-символьный-ключ"
dotnet user-secrets set "ConnectionStrings__DefaultConnection" "Host=localhost;Database=SMTDb;Username=postgres;Password=ваш-пароль"

# Запустить
dotnet run --project SMT.API/SMT.API.csproj
```

---

## 📁 Новые файлы

```
SMT.API/
├── SMT.API/
│   ├── Middleware/
│   │   └── ExceptionHandlingMiddleware.cs    # Глобальная обработка исключений
│   └── Program.cs                             # Обновлён с JWT и middleware
├── SMT.Application/
│   ├── Common/
│   │   ├── Behaviors/
│   │   │   └── ValidationBehavior.cs         # Валидация через MediatR
│   │   └── Validation/
│   │       ├── LoginUserCommandValidator.cs
│   │       ├── RegisterUserCommandValidator.cs
│   │       ├── UpdateTableCommandValidator.cs
│   │       └── RefreshTokenCommandValidator.cs
│   └── UserCommand/
│       └── RefreshToken/
│           ├── RefreshTokenCommand.cs        # Команда обновления токена
│           └── RefreshTokenHandler.cs        # Обработчик
├── SMT.Domain/
│   └── Exceptions/
│       └── UnauthorizedException.cs          # Исправленное исключение
└── compose.yaml                               # Полный Docker Compose
```

---

## 🔐 Рекомендации по безопасности

1. **Смените пароли по умолчанию** в production
2. **Используйте HTTPS** в production (добавлен `UseHttpsRedirection()`)
3. **Регулярно обновляйте пакеты** (особенно AutoMapper)
4. **Добавьте логирование** в middleware исключений
5. **Настройте CORS** для production окружения
6. **Используйте external secret manager** (Azure Key Vault, AWS Secrets Manager, HashiCorp Vault)
