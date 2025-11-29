# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar solo el archivo .csproj primero para aprovechar layers de cache
COPY *.csproj ./
RUN dotnet restore

# Copiar el resto del proyecto
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar archivos publicados desde el build stage
COPY --from=build /app/publish .

# Puerto por defecto para ASP.NET
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Comando de inicio
ENTRYPOINT ["dotnet", "SpotifyWrapped.dll"]