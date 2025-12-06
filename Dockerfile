# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files first for better layer caching
COPY ["DRB-Task Solution.sln", "./"]
COPY ["DRB-Task.API/DRB-Task.API.csproj", "DRB-Task.API/"]
COPY ["Core/Domain/Domain.csproj", "Core/Domain/"]
COPY ["Core/Service.Abstraction/Service.Abstraction.csproj", "Core/Service.Abstraction/"]
COPY ["Core/Service.Implementation/Service.Implementation.csproj", "Core/Service.Implementation/"]
COPY ["Infrastructure/Persistence/Persistence.csproj", "Infrastructure/Persistence/"]
COPY ["Infrastructure/Presentation/Presentation.csproj", "Infrastructure/Presentation/"]
COPY ["Shared/Shared.csproj", "Shared/"]

# Restore dependencies
RUN dotnet restore "DRB-Task.API/DRB-Task.API.csproj"

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR "/src/DRB-Task.API"
RUN dotnet build "DRB-Task.API.csproj" -c Release -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish "DRB-Task.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published application
COPY --from=publish /app/publish .

# Expose port (Railway will inject PORT env variable)
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}

# Entry point
ENTRYPOINT ["dotnet", "DRB-Task.API.dll"]
