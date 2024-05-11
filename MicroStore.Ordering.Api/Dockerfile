#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MicroStore.Ordering.Api/MicroStore.Ordering.Api.csproj", "MicroStore.Ordering.Api/"]
COPY ["MicroStore.BuildingBlocks.AspNetCore/MicroStore.BuildingBlocks.AspNetCore.csproj", "MicroStore.BuildingBlocks.AspNetCore/"]
COPY ["MicroStore.BuildingBlocks.Utils/MicroStore.BuildingBlocks.Utils.csproj", "MicroStore.BuildingBlocks.Utils/"]
COPY ["MicroStore.Ordering.Infrastructure/MicroStore.Ordering.Infrastructure.csproj", "MicroStore.Ordering.Infrastructure/"]
COPY ["MicroStore.Ordering.Application/MicroStore.Ordering.Application.csproj", "MicroStore.Ordering.Application/"]
COPY ["MicroStore.Inventory.IntegrationEvents/MicroStore.Inventory.IntegrationEvents.csproj", "MicroStore.Inventory.IntegrationEvents/"]
COPY ["MicroStore.Ordering.IntegrationEvents/MicroStore.Ordering.IntegrationEvents.csproj", "MicroStore.Ordering.IntegrationEvents/"]
COPY ["MicroStore.Payment.IntegrationEvents/MicroStore.Payment.IntegrationEvents.csproj", "MicroStore.Payment.IntegrationEvents/"]
RUN dotnet restore "./MicroStore.Ordering.Api/MicroStore.Ordering.Api.csproj"
COPY . .
WORKDIR "/src/MicroStore.Ordering.Api"
RUN dotnet build "./MicroStore.Ordering.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MicroStore.Ordering.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroStore.Ordering.Api.dll"]