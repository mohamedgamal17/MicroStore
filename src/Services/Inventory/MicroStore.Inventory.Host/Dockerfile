#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MicroStore.Inventory.Host/MicroStore.Inventory.Host.csproj", "MicroStore.Inventory.Host/"]
COPY ["MicroStore.BuildingBlocks.AspNetCore/MicroStore.BuildingBlocks.AspNetCore.csproj", "MicroStore.BuildingBlocks.AspNetCore/"]
COPY ["MicroStore.BuildingBlocks.Utils/MicroStore.BuildingBlocks.Utils.csproj", "MicroStore.BuildingBlocks.Utils/"]
COPY ["MicroStore.Inventory.Application/MicroStore.Inventory.Application.csproj", "MicroStore.Inventory.Application/"]
COPY ["MicroStore.Catalog.IntegrationEvents/MicroStore.Catalog.IntegrationEvents.csproj", "MicroStore.Catalog.IntegrationEvents/"]
COPY ["MicroStore.Inventory.Domain/MicroStore.Inventory.Domain.csproj", "MicroStore.Inventory.Domain/"]
COPY ["MicroStore.Inventory.IntegrationEvents/MicroStore.Inventory.IntegrationEvents.csproj", "MicroStore.Inventory.IntegrationEvents/"]
COPY ["MicroStore.Inventory.Infrastructure/MicroStore.Inventory.Infrastructure.csproj", "MicroStore.Inventory.Infrastructure/"]
RUN dotnet restore "./MicroStore.Inventory.Host/MicroStore.Inventory.Host.csproj"
COPY . .
WORKDIR "/src/MicroStore.Inventory.Host"
RUN dotnet build "./MicroStore.Inventory.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MicroStore.Inventory.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroStore.Inventory.Host.dll"]