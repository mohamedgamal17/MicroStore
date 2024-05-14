#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MicroStore.IdentityProvider.Host/MicroStore.IdentityProvider.Host.csproj", "MicroStore.IdentityProvider.Host/"]
COPY ["MicroStore.AspNetCore.UI/MicroStore.AspNetCore.UI.csproj", "MicroStore.AspNetCore.UI/"]
COPY ["MicroStore.BuildingBlocks.AspNetCore/MicroStore.BuildingBlocks.AspNetCore.csproj", "MicroStore.BuildingBlocks.AspNetCore/"]
COPY ["MicroStore.BuildingBlocks.Utils/MicroStore.BuildingBlocks.Utils.csproj", "MicroStore.BuildingBlocks.Utils/"]
COPY ["MicroStore.IdentityProvider.Identity.Web/MicroStore.IdentityProvider.Identity.Web.csproj", "MicroStore.IdentityProvider.Identity.Web/"]
COPY ["MicroStore.IdentityProvider.Identity.Infrastructure/MicroStore.IdentityProvider.Identity.Infrastructure.csproj", "MicroStore.IdentityProvider.Identity.Infrastructure/"]
COPY ["MicroStore.IdentityProvider.Identity.Application/MicroStore.IdentityProvider.Identity.Application.csproj", "MicroStore.IdentityProvider.Identity.Application/"]
COPY ["MicroStore.IdentityProvider.Identity.Domain.Shared/MicroStore.IdentityProvider.Identity.Domain.Shared.csproj", "MicroStore.IdentityProvider.Identity.Domain.Shared/"]
COPY ["MicroStore.IdentityProvider.IdentityServer.Infrastructure/MicroStore.IdentityProvider.IdentityServer.Infrastructure.csproj", "MicroStore.IdentityProvider.IdentityServer.Infrastructure/"]
COPY ["MicroStore.IdentityProvider.IdentityServer.Application/MicroStore.IdentityProvider.IdentityServer.Application.csproj", "MicroStore.IdentityProvider.IdentityServer.Application/"]
COPY ["MicroStore.IdentityProvider.IdentityServer.Web/MicroStore.IdentityProvider.IdentityServer.Web.csproj", "MicroStore.IdentityProvider.IdentityServer.Web/"]
RUN dotnet restore "./MicroStore.IdentityProvider.Host/MicroStore.IdentityProvider.Host.csproj"
COPY . .

WORKDIR "/src/MicroStore.IdentityProvider.Host"
RUN apt-get update -yq && apt-get upgrade -yq && apt-get install -yq curl git nano
RUN curl -sL https://deb.nodesource.com/setup_20.x | bash - && apt-get install -yq nodejs build-essential
RUN npm install
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet tool install --global Volo.Abp.Cli --version 6.0.0
RUN abp install-libs
RUN dotnet build "./MicroStore.IdentityProvider.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build




FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/MicroStore.IdentityProvider.Host"
RUN dotnet publish "./MicroStore.IdentityProvider.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroStore.IdentityProvider.Host.dll"]