#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MicroStore.Payment.Api.Host/MicroStore.Payment.Api.Host.csproj", "MicroStore.Payment.Api.Host/"]
COPY ["MicroStore.BuildingBlocks.AspNetCore/MicroStore.BuildingBlocks.AspNetCore.csproj", "MicroStore.BuildingBlocks.AspNetCore/"]
COPY ["MicroStore.BuildingBlocks.Utils/MicroStore.BuildingBlocks.Utils.csproj", "MicroStore.BuildingBlocks.Utils/"]
COPY ["MicroStore.Payment.Application/MicroStore.Payment.Application.csproj", "MicroStore.Payment.Application/"]
COPY ["MicroStore.Payment.Domain.Shared/MicroStore.Payment.Domain.Shared.csproj", "MicroStore.Payment.Domain.Shared/"]
COPY ["MicroStore.Payment.IntegrationEvents/MicroStore.Payment.IntegrationEvents.csproj", "MicroStore.Payment.IntegrationEvents/"]
COPY ["MicroStore.Payment.Plugin.StripeGateway/MicroStore.Payment.Plugin.StripeGateway.csproj", "MicroStore.Payment.Plugin.StripeGateway/"]
RUN dotnet restore "./MicroStore.Payment.Api.Host/MicroStore.Payment.Api.Host.csproj"
COPY . .
WORKDIR "/src/MicroStore.Payment.Api.Host"
RUN dotnet build "./MicroStore.Payment.Api.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MicroStore.Payment.Api.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroStore.Payment.Api.Host.dll"]