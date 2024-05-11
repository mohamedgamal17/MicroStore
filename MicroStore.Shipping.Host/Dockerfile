#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MicroStore.Shipping.Host/MicroStore.Shipping.Host.csproj", "MicroStore.Shipping.Host/"]
COPY ["MicroStore.BuildingBlocks.AspNetCore/MicroStore.BuildingBlocks.AspNetCore.csproj", "MicroStore.BuildingBlocks.AspNetCore/"]
COPY ["MicroStore.BuildingBlocks.Utils/MicroStore.BuildingBlocks.Utils.csproj", "MicroStore.BuildingBlocks.Utils/"]
COPY ["MicroStore.Shipping.Application/MicroStore.Shipping.Application.csproj", "MicroStore.Shipping.Application/"]
COPY ["MicroStore.Ordering.IntegrationEvents/MicroStore.Ordering.IntegrationEvents.csproj", "MicroStore.Ordering.IntegrationEvents/"]
COPY ["MicroStore.Shipping.Application.Abstraction/MicroStore.Shipping.Application.Abstraction.csproj", "MicroStore.Shipping.Application.Abstraction/"]
COPY ["MicroStore.Shipping.Domain/MicroStore.Shipping.Domain.csproj", "MicroStore.Shipping.Domain/"]
COPY ["MicroStore.Shipping.IntegrationEvents/MicroStore.Shipping.IntegrationEvents.csproj", "MicroStore.Shipping.IntegrationEvents/"]
COPY ["MicroStore.Shipping.Infrastructure/MicroStore.Shipping.Infrastructure.csproj", "MicroStore.Shipping.Infrastructure/"]
COPY ["MicroStore.Shipping.Plugin.ShipEngineGateway/MicroStore.Shipping.Plugin.ShipEngineGateway.csproj", "MicroStore.Shipping.Plugin.ShipEngineGateway/"]
RUN dotnet restore "./MicroStore.Shipping.Host/MicroStore.Shipping.Host.csproj"
COPY . .
WORKDIR "/src/MicroStore.Shipping.Host"
RUN dotnet build "./MicroStore.Shipping.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MicroStore.Shipping.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroStore.Shipping.Host.dll"]