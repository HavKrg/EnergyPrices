# Set the base image for the build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copy .sln and .csproj files for all projects
COPY *.sln .
COPY src/Application/*.csproj src/Application/
COPY src/Core/*.csproj src/Core/
COPY src/Infrastructure/*.csproj src/Infrastructure/
COPY src/WebApi/*.csproj src/WebApi/

# Restore packages for all projects
RUN dotnet restore

# Copy other source files and build the projects
COPY src/ .
RUN dotnet publish src/WebApi/WebApi.csproj -c Release -o out

# Set the base image for the runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expose the default ASP.NET Core WebApi port (typically 80 for HTTP and 443 for HTTPS)
EXPOSE 80

# Set the entry point for the application
ENTRYPOINT ["dotnet", "WebApi.dll"]