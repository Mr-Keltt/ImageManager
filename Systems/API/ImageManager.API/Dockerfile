#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Systems/API/ImageManager.API/ImageManager.API.csproj", "Systems/API/ImageManager.API/"]
COPY ["Application/Models/ImageManager.Models/ImageManager.Models.csproj", "Application/Models/ImageManager.Models/"]
COPY ["Application/Services/ImageManager.Services.Logger/ImageManager.Services.Logger.csproj", "Application/Services/ImageManager.Services.Logger/"]
COPY ["Application/Services/ImageManager.Services.Settings/ImageManager.Services.Settings.csproj", "Application/Services/ImageManager.Services.Settings/"]
COPY ["Shared/ImageManager.Common/ImageManager.Common.csproj", "Shared/ImageManager.Common/"]
COPY ["Infrastructure/Data/ImageManager.Context/ImageManager.Context.csproj", "Infrastructure/Data/ImageManager.Context/"]
COPY ["Infrastructure/Repositories/ImageManager.Entities/ImageManager.Entities.csproj", "Infrastructure/Repositories/ImageManager.Entities/"]
COPY ["Infrastructure/Data/Migrations/ImageManager.Context.Migrations.PgSql/ImageManager.Context.Migrations.PgSql.csproj", "Infrastructure/Data/Migrations/ImageManager.Context.Migrations.PgSql/"]
RUN dotnet restore "./Systems/API/ImageManager.API/ImageManager.API.csproj"
COPY . .
WORKDIR "/src/Systems/API/ImageManager.API"
RUN dotnet build "./ImageManager.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ImageManager.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ImageManager.API.dll"]