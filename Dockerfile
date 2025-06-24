# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# 1. Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Jat.Web/Jat.Web.csproj", "Jat.Web/"]
COPY ["Jat.Tests/Jat.Tests.csproj", "Jat.Tests/"]
RUN dotnet restore "./Jat.Web/Jat.Web.csproj"
COPY . .
WORKDIR "/src/Jat.Web"
RUN dotnet build "./Jat.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet publish "./Jat.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 2. Test stage
FROM build AS test
WORKDIR /src/Jat.Tests
RUN dotnet test Jat.Tests.csproj --no-restore --no-build --logger:trx

# 3. Final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
EXPOSE 8081
ENTRYPOINT ["dotnet", "Jat.Web.dll"]