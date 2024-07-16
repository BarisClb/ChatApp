FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY ./ChatApp.WebHub/Configurations/sslcertificate.pfx /https/sslcertificate.pfx

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ChatApp.WebHub/ChatApp.WebHub.csproj", "ChatApp.WebHub/"]
RUN dotnet restore "./ChatApp.WebHub/ChatApp.WebHub.csproj"
COPY . .
WORKDIR "/src/ChatApp.WebHub"
RUN dotnet build "./ChatApp.WebHub.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ChatApp.WebHub.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=https://+:8081;http://+:8080
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/sslcertificate.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=Password123
ENTRYPOINT ["dotnet", "ChatApp.WebHub.dll"]