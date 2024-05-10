# Use official .NET Core SDK image as the base image for building
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app



# Copy the csproj file into the container
FROM base AS build
COPY ["Transfer.csproj", ""]
RUN dotnet restore "./Transfer.csproj"
COPY . .
RUN dotnet build "Transfer.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Transfer.csproj" -c Release -o /app/publish

# Final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final

EXPOSE 80
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Transfer.dll"]
