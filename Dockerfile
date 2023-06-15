# Set the base image for the build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Set the working directory in the container
WORKDIR /src

# Copy the .sln file
COPY *.sln ./

# Copy the .csproj files and restore the solution's dependencies
COPY src/Application/*.csproj ./src/Application/
COPY src/Core/*.csproj ./src/Core/
COPY src/Infrastructure/*.csproj ./src/Infrastructure/
COPY src/WebApi/*.csproj ./src/WebApi/
RUN dotnet restore

# Copy the remaining project files
COPY src/Application/. ./src/Application/
COPY src/Core/. ./src/Core/
COPY src/Infrastructure/. ./src/Infrastructure/
COPY src/WebApi/. ./src/WebApi/

# Build and publish the WebApi project
WORKDIR /src/src/WebApi
RUN dotnet publish -c Release -o /app

# Set the base image for the runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime

# Set the working directory in the container
WORKDIR /app

# Copy the output from the build stage to the runtime stage
COPY --from=build /app .

# Expose the Kestrel web server port
EXPOSE 80

# Set the entrypoint to start the application
ENTRYPOINT ["dotnet", "WebApi.dll"]