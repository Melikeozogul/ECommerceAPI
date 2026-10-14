# Build aşaması
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ECommerceAPI.sln ./
COPY ECommerceAPI.API/ECommerceAPI.API.csproj ECommerceAPI.API/
COPY ECommerceAPI.Application/ECommerceAPI.Application.csproj ECommerceAPI.Application/
COPY ECommerceAPI.Domain/ECommerceAPI.Domain.csproj ECommerceAPI.Domain/
COPY ECommerceAPI.Infrastructure/ECommerceAPI.Infrastructure.csproj ECommerceAPI.Infrastructure/

RUN dotnet restore ECommerceAPI.sln

COPY . .
RUN dotnet publish ECommerceAPI.API/ECommerceAPI.API.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# Çalıştırma aşaması
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "ECommerceAPI.API.dll"]
