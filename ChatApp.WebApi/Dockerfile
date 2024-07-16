FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ChatApp.WebApi/ChatApp.WebApi.csproj", "ChatApp.WebApi/"]
COPY ["ChatApp.Application/ChatApp.Application.csproj", "ChatApp.Application/"]
COPY ["ChatApp.Domain/ChatApp.Domain.csproj", "ChatApp.Domain/"]
COPY ["ChatApp.Infrastructure/ChatApp.Infrastructure.csproj", "ChatApp.Infrastructure/"]
COPY ["ChatApp.Persistence/ChatApp.Persistence.csproj", "ChatApp.Persistence/"]
RUN dotnet restore "./ChatApp.WebApi/./ChatApp.WebApi.csproj"
COPY . .
WORKDIR "/src/ChatApp.WebApi"
RUN dotnet build "./ChatApp.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ChatApp.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChatApp.WebApi.dll"]