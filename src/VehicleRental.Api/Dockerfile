#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#USER app
WORKDIR /app
EXPOSE 8080
#EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src
COPY ["src/VehicleRental.Api/VehicleRental.Api.csproj", "src/VehicleRental.Api/"]
COPY ["src/VehicleRental.Application/VehicleRental.Application.csproj", "src/VehicleRental.Application/"]
COPY ["src/VehicleRental.Domain/VehicleRental.Domain.csproj", "src/VehicleRental.Domain/"]
COPY ["src/VehicleRental.Infrastructure/VehicleRental.Infrastructure.csproj", "src/VehicleRental.Infrastructure/"]
RUN dotnet restore "./src/VehicleRental.Api/VehicleRental.Api.csproj"
COPY . .
WORKDIR "/src/src/VehicleRental.Api"
RUN dotnet build "./VehicleRental.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "./VehicleRental.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VehicleRental.Api.dll"]