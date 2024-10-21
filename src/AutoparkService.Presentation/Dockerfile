FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/AutoparkService.Presentation/AutoparkService.Presentation.csproj", "src/AutoparkService.Presentation/"]
COPY ["src/AutoparkService.Infrastructure/AutoparkService.Infrastructure.csproj", "src/AutoparkService.Infrastructure/"]
COPY ["src/AutoparkService.Application/AutoparkService.Application.csproj", "src/AutoparkService.Application/"]
COPY ["src/AutoparkService.Domain/AutoparkService.Domain.csproj", "src/AutoparkService.Domain/"]
RUN dotnet restore "src/AutoparkService.Presentation/AutoparkService.Presentation.csproj"
COPY . .
WORKDIR "/src/src/AutoparkService.Presentation"
RUN dotnet build "AutoparkService.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AutoparkService.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AutoparkService.Presentation.dll"]
