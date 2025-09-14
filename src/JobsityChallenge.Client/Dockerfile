# Example Dockerfile for Blazor WebAssembly Client
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY src/JobsityChallenge.Client/JobsityChallenge.Client.csproj JobsityChallenge.Client/
RUN dotnet restore JobsityChallenge.Client/JobsityChallenge.Client.csproj

# Copy all project files and publish the app
COPY src/JobsityChallenge.Client/ JobsityChallenge.Client/
WORKDIR /src/JobsityChallenge.Client
RUN dotnet publish -c Release -o /app/publish

# Use NGINX to serve the static files
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=build /app/publish/wwwroot .
EXPOSE 80
