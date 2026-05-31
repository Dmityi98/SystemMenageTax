FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
# EXPOSE 8081 - закомментировано, так как HTTPS внутри сети не нужен

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем проекты (пути выглядят корректно для сборки от корня репозитория)
COPY ["SMT.API/SMT.API/SMT.API.csproj", "SMT.API/SMT.API/"]
COPY ["SMT.API/SMT.Application/SMT.Application.csproj", "SMT.API/SMT.Application/"]
COPY ["SMT.API/SMT.Domain/SMT.Domain.csproj", "SMT.API/SMT.Domain/"]
COPY ["SMT.API/SMT.Infrastructure/SMT.Infrastructure.csproj", "SMT.API/SMT.Infrastructure/"]
COPY ["SMT.API/SMT.Persistence/SMT.Persistence.csproj", "SMT.API/SMT.Persistence/"]
COPY ["SMT.Events/SMT.Events/SMT.Events.csproj", "SMT.Events/SMT.Events/"]

RUN dotnet restore "SMT.API/SMT.API/SMT.API.csproj"

COPY . .

WORKDIR "/src/SMT.API/SMT.API"
RUN dotnet build "SMT.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SMT.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

# ИСПРАВЛЕНО: --chown вместо --crown и добавлено --from=publish
COPY --chown=$APP_UID:0 --from=publish /app/publish .

USER $APP_UID
ENTRYPOINT ["dotnet", "SMT.API.dll"]