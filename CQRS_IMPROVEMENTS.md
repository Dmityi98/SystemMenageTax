# Улучшения CQRS в SMT.API

## 📋 Обзор изменений

Все команды и обработчики были значительно улучшены для соответствия лучшим практикам CQRS, безопасности и чистоты кода.

---

## ✅ Выполненные улучшения

### 1. User Commands

#### RegisterUserCommand
**Было:**
```csharp
public class RegisterUserCommand : IRequest<RegisterUserDto>
{
    public string Name { get; set; }
    public string Password { get; set; }
}
```

**Стало:**
```csharp
public record RegisterUserCommand(
    string Name,
    string Password
) : IRequest<RegisterUserDto>;
```

**Улучшения:**
- ✅ Использован `record` вместо `class` (иммутабельность)
- ✅ Перемещён в правильный namespace: `SMT.Application.UserCommand.RegisterUser`

---

#### RegisterUserDto
**Было:**
```csharp
public class RegisterUserDto(string name, string password) : IMapWith<User>
{
    public string Name { get; set; } = name;
    public string Password { get; set; } = password; // ❌ Возврат пароля!
}
```

**Стало:**
```csharp
public record RegisterUserDto(string Name, Guid Id) : IMapWith<User>;
```

**Улучшения:**
- ✅ **Удалён пароль из ответа** (безопасность!)
- ✅ Добавлен `Id` пользователя
- ✅ Использован `record`

---

#### RegisterUserHandler
**Было:**
```csharp
// Неправильное исключение для существующего пользователя
if (user != null)
{
    throw new NotFoundExceptions(nameof(User), request.Name);
}
```

**Стало:**
```csharp
var existingUser = await context.Users
    .AsNoTracking()
    .FirstOrDefaultAsync(u => u.Name == request.Name, cancellationToken);

if (existingUser != null)
{
    throw new ConflictException($"Пользователь с именем '{request.Name}' уже существует");
}
```

**Улучшения:**
- ✅ Использовано `AsNoTracking()` для чтения
- ✅ Правильное исключение `ConflictException` (HTTP 409)
- ✅ Добавлен новый класс исключения `ConflictException`
- ✅ Улучшено сообщение об ошибке

---

#### LoginUserCommand
**Было:**
```csharp
public class LoginUserCommand : IRequest<LoginUserDto>
{
    public string Name { get; set; }
    public string Password { get; set; }
}
```

**Стало:**
```csharp
public record LoginUserCommand(
    string Name,
    string Password
) : IRequest<LoginUserDto>;
```

**Улучшения:**
- ✅ Использован `record`
- ✅ Добавлены XML-документы

---

#### LoginUserDto
**Было:**
```csharp
public class LoginUserDto(string userName, string token, string refreshToken) : IMapWith<LoginUserDto>
{
    // ❌ Неправильный IMapWith<LoginUserDto>
}
```

**Стало:**
```csharp
public record LoginUserDto(
    string UserName,
    string AccessToken,
    string RefreshToken
);
```

**Улучшения:**
- ✅ Удалён неправильный `IMapWith`
- ✅ Упрощён до чистого record

---

#### LoginUserHandler
**Улучшения:**
- ✅ Добавлены XML-документы
- ✅ Улучшено форматирование кода
- ✅ Добавлены комментарии к секциям кода

---

#### RefreshTokenCommand
**Было:**
```csharp
public class RefreshTokenCommand : IRequest<RefreshTokenDto>
{
    public string RefreshToken { get; set; } = string.Empty;
}
```

**Стало:**
```csharp
public record RefreshTokenCommand(
    string RefreshToken
) : IRequest<RefreshTokenDto>;
```

**Улучшения:**
- ✅ Использован `record`
- ✅ Добавлены XML-документы

---

#### RefreshTokenHandler
**Улучшения:**
- ✅ Добавлены XML-документы
- ✅ Улучшены комментарии
- ✅ Использован `new()` вместо `new List<Claim>`

---

### 2. Year Commands

#### CreateTableCommand
**Было:**
```csharp
public class CreateTableCommand : IRequest<YearDTO>
{
    [Required] public Guid Id { get; set; }  // ❌ Неправильное имя
    [Required] public string NameTable{ get; set; }
}
```

**Стало:**
```csharp
public record CreateTableCommand(
    Guid UserId,
    string NameTable
) : IRequest<YearDTO>;
```

**Улучшения:**
- ✅ Исправлено имя: `Id` → `UserId`
- ✅ Использован `record`
- ✅ Удалены атрибуты `[Required]` (теперь в FluentValidation)

---

#### CreateTableHandler
**Было:**
```csharp
public async Task<YearDTO> Handle(CreateTableCommand request, CancellationToken cancellationToken)
{
    var user = await context.Users.FirstOrDefaultAsync(user => user.Id == request.Id);
    var year = await Initialize(request.Id, request.NameTable, cancellationToken);
    year.UserId = user.Id;  // ❌ Странная логика
    return mapper.Map<YearDTO>(year);
}
```

**Стало:**
```csharp
public async Task<YearDTO> Handle(CreateTableCommand request, CancellationToken cancellationToken)
{
    // Проверка существования пользователя
    var userExists = await context.Users
        .AsNoTracking()
        .AnyAsync(u => u.Id == request.UserId, cancellationToken);

    if (!userExists)
    {
        throw new NotFoundExceptions(nameof(User), request.UserId);
    }

    // Создание годовой таблицы
    var year = CreateYearWithQuarters(request.UserId, request.NameTable);
    context.Years.Add(year);
    await context.SaveChangesAsync(cancellationToken);

    return mapper.Map<YearDTO>(year);
}
```

**Улучшения:**
- ✅ Проверка существования пользователя
- ✅ Использовано `AsNoTracking()`
- ✅ Вынесена логика создания в отдельный метод
- ✅ Улучшена читаемость

---

#### UpdateTableCommand
**Было:**
```csharp
public class UpdateTableCommand : IRequest<bool>
{
    [Required] public string? NameTable { get; set; }
    [Required] public YearDTO yearDto { get; set; }  // ❌ lowercase
}
```

**Стало:**
```csharp
public record UpdateTableCommand(
    Guid UserId,
    Guid YearId,
    string NameTable,
    YearDTO YearDto
) : IRequest<bool>;
```

**Улучшения:**
- ✅ Добавлен `UserId` для проверки прав
- ✅ Добавлен `YearId` для явности
- ✅ Исправлено имя: `yearDto` → `YearDto`
- ✅ Использован `record`

---

#### UpdateTableHandler
**Было:**
```csharp
// ❌ Нет проверки прав доступа!
var year = await context.Years
    .Include(y => y.Quarters)
    .ThenInclude(q => q.Columns)
    .FirstOrDefaultAsync(y => y.Id == request.yearDto.Id, cancellationToken);
```

**Стало:**
```csharp
// Проверка существования таблицы и прав доступа
var year = await context.Years
    .Include(y => y.Quarters)
    .ThenInclude(q => q.Columns)
    .FirstOrDefaultAsync(y => y.Id == request.YearId, cancellationToken);

if (year == null)
{
    throw new NotFoundExceptions(nameof(Year), request.YearId);
}

// Проверка: принадлежит ли таблица пользователю
if (year.UserId != request.UserId)
{
    throw new UnauthorizedException("У вас нет прав для редактирования этой таблицы");
}
```

**Улучшения:**
- ✅ **Добавлена проверка прав доступа** (безопасность!)
- ✅ Улучшены имена переменных
- ✅ Добавлены XML-документы

---

#### GetYearByIdCommand
**Было:**
```csharp
public class GetYearByIdCommand : IRequest<YearDTO>
{
    public Guid Id { get; set; }  // ❌ Нет UserId
}
```

**Стало:**
```csharp
public record GetYearByIdCommand(
    Guid Id,
    Guid UserId
) : IRequest<YearDTO>;
```

**Улучшения:**
- ✅ Добавлен `UserId` для проверки прав
- ✅ Использован `record`

---

#### GetYearByIdHandler
**Было:**
```csharp
// ❌ Нет проверки прав доступа!
var entity = await context.Years
    .Include(y => y.Quarters)
    .FirstOrDefaultAsync(y => y.Id == request.Id, cancellationToken);
```

**Стало:**
```csharp
var entity = await context.Years
    .Include(y => y.Quarters)
    .ThenInclude(q => q.Columns)
    .FirstOrDefaultAsync(y => y.Id == request.Id, cancellationToken);

if (entity == null)
{
    throw new NotFoundExceptions(nameof(Year), request.Id);
}

// Проверка: принадлежит ли таблица пользователю
if (entity.UserId != request.UserId)
{
    throw new UnauthorizedException("У вас нет прав для просмотра этой таблицы");
}
```

**Улучшения:**
- ✅ **Добавлена проверка прав доступа** (безопасность!)
- ✅ Добавлен `ThenInclude` для загрузки колонок
- ✅ Добавлены XML-документы

---

### 3. YearsController

**Было:**
```csharp
[HttpGet("{id}")]
public async Task<IActionResult> GetYearById(Guid id)
{
    try
    {
        var command = new GetYearByIdCommand() { Id = id };
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    catch (Exception ex)
    {
        return StatusCode(500, "Internal server error");
    }
}
```

**Стало:**
```csharp
[HttpGet("{id}")]
public async Task<IActionResult> GetYearById(Guid id)
{
    var userId = GetCurrentUserUserId();
    var command = new GetYearByIdCommand(id, userId);
    var result = await _mediator.Send(command);
    return Ok(result);
}

private Guid GetCurrentUserUserId()
{
    var claim = User.FindFirst(ClaimTypes.NameIdentifier) 
                ?? throw new UnauthorizedAccessException("Пользователь не аутентифицирован");
    return Guid.Parse(claim.Value);
}
```

**Улучшения:**
- ✅ **Извлечение UserId из JWT токена**
- ✅ Удалены try-catch (теперь обрабатываются middleware)
- ✅ Добавлен вспомогательный метод `GetCurrentUserUserId()`
- ✅ Добавлены XML-документы для endpoints

---

### 4. Валидаторы FluentValidation

Созданы новые валидаторы:

#### CreateTableCommandValidator
```csharp
public class CreateTableCommandValidator : AbstractValidator<CreateTableCommand>
{
    public CreateTableCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.NameTable)
            .NotEmpty()
            .MaximumLength(100);
    }
}
```

#### GetYearByIdCommandValidator
```csharp
public class GetYearByIdCommandValidator : AbstractValidator<GetYearByIdCommand>
{
    public GetYearByIdCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
```

#### UpdateTableCommandValidator (обновлён)
```csharp
public class UpdateTableCommandValidator : AbstractValidator<UpdateTableCommand>
{
    public UpdateTableCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.YearId).NotEmpty();
        RuleFor(x => x.NameTable)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(x => x.YearDto).NotNull();
    }
}
```

---

## 🔐 Безопасность

### Критические улучшения безопасности

| Проблема | Решение |
|----------|---------|
| ❌ Нет проверки прав доступа | ✅ Добавлена проверка `UserId` во всех handlers |
| ❌ Возврат пароля в RegisterUserDto | ✅ Удалён пароль из ответа |
| ❌ Нет проверки принадлежности таблицы | ✅ Проверка `year.UserId != request.UserId` |
| ❌ Неправильные исключения | ✅ `ConflictException` для 409, `UnauthorizedException` для 401 |

---

## 📁 Новые файлы

```
SMT.Application/
├── Common/Validation/
│   ├── CreateTableCommandValidator.cs    # Новый
│   └── GetYearByIdCommandValidator.cs    # Новый
├── UserCommand/
│   └── RegisterUser/
│       └── RegisterUserHandler.cs        # Добавлен ConflictException
└── Years/
    ├── CreateTable/
    │   └── CreateTableHandler.cs         # Улучшен
    ├── UpdateTable/
    │   └── UpdateTableHandler.cs         # Добавлена проверка прав
    └── GetYearById/
        └── GetYearByIdHandler.cs         # Добавлена проверка прав
```

---

## 🎯 Итоговые преимущества

| Категория | Улучшения |
|-----------|-----------|
| **Безопасность** | Проверка прав доступа, скрытие пароля, правильные исключения |
| **Чистота кода** | Record types, XML-документы, улучшенные имена |
| **Сопровождаемость** | Разделение ответственности, валидация через FluentValidation |
| **Тестируемость** | Иммутабельные команды, явные зависимости |

---

## ⚠️ Breaking Changes

### Изменения в контрактах

1. **GetYearByIdCommand**: добавлен параметр `UserId`
2. **UpdateTableCommand**: добавлены параметры `UserId`, `YearId`
3. **CreateTableCommand**: `Id` переименован в `UserId`
4. **RegisterUserDto**: удалён параметр `Password`, добавлен `Id`

### Требуемые изменения на клиенте

```typescript
// Было:
GET /api/years/{id}

// Стало:
GET /api/years/{id}
Authorization: Bearer {JWT с ClaimTypes.NameIdentifier}
```

---

## 🚀 Запуск

```bash
cd /Users/admin/Documents/Диплом/SMT.API
dotnet build
dotnet run --project SMT.API/SMT.API.csproj
```

Все улучшения протестированы и собраны без ошибок.
