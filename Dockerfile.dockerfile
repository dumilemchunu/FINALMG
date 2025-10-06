# =========================
# BUILD STAGE
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and publish
COPY . ./
RUN dotnet publish -c Release -o out

# =========================
# RUNTIME STAGE
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Expose port 8080 for Render
EXPOSE 8080

# Start the app
ENTRYPOINT ["dotnet", "TruckSheduler.dll"]
