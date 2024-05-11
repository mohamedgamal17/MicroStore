#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MicroStore.Catalog.Api/MicroStore.Catalog.Api.csproj", "MicroStore.Catalog.Api/"]
COPY ["MicroStore.BuildingBlocks.AspNetCore/MicroStore.BuildingBlocks.AspNetCore.csproj", "MicroStore.BuildingBlocks.AspNetCore/"]
COPY ["MicroStore.BuildingBlocks.Utils/MicroStore.BuildingBlocks.Utils.csproj", "MicroStore.BuildingBlocks.Utils/"]
COPY ["MicroStore.Catalog.Application/MicroStore.Catalog.Application.csproj", "MicroStore.Catalog.Application/"]
COPY ["MicroStore.Catalog.Application.Abstractions/MicroStore.Catalog.Application.Abstractions.csproj", "MicroStore.Catalog.Application.Abstractions/"]
COPY ["MicroStore.Catalog.Domain/MicroStore.Catalog.Domain.csproj", "MicroStore.Catalog.Domain/"]
COPY ["MicroStore.Catalog.Entities.ElasticSearch/MicroStore.Catalog.Entities.ElasticSearch.csproj", "MicroStore.Catalog.Entities.ElasticSearch/"]
COPY ["MicroStore.Catalog.IntegrationEvents/MicroStore.Catalog.IntegrationEvents.csproj", "MicroStore.Catalog.IntegrationEvents/"]
COPY ["MicroStore.Catalog.Infrastructure/MicroStore.Catalog.Infrastructure.csproj", "MicroStore.Catalog.Infrastructure/"]
COPY ["MicroStore.Catalog.Application.Operations/MicroStore.Catalog.Application.Operations.csproj", "MicroStore.Catalog.Application.Operations/"]
RUN dotnet restore "./MicroStore.Catalog.Api/MicroStore.Catalog.Api.csproj"
COPY . .
WORKDIR "/src/MicroStore.Catalog.Api"
RUN dotnet build "./MicroStore.Catalog.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MicroStore.Catalog.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroStore.Catalog.Api.dll"]