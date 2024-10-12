# Use ASP.NET Core Runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use .NET Core SDK to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy .csproj files for restoring dependencies
COPY ["SWP.ProductManagement.API/SWP.ProductManagement.API.csproj", "SWP.ProductManagement.API/"]
COPY ["SWP.ProductManagement.Service/SWP.ProductManagement.Service.csproj", "SWP.ProductManagement.Service/"]
COPY ["SWP.ProductManagement.Repository/SWP.ProductManagement.Repository.csproj", "SWP.ProductManagement.Repository/"]

# Restore dependencies
RUN dotnet restore "./SWP.ProductManagement.API/SWP.ProductManagement.API.csproj"

# Copy everything and build the app
COPY . .
WORKDIR "/src/SWP.ProductManagement.API"
RUN dotnet build "./SWP.ProductManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the app to the /app/publish folder
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SWP.ProductManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Run the app using the base image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SWP.ProductManagement.API.dll"]
