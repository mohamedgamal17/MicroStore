#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MicroStore.Client.PublicWeb/MicroStore.Client.PublicWeb.csproj", "MicroStore.Client.PublicWeb/"]
COPY ["MicroStore.AspNetCore.UI/MicroStore.AspNetCore.UI.csproj", "MicroStore.AspNetCore.UI/"]
COPY ["MicroStore.BuildingBlocks.AspNetCore/MicroStore.BuildingBlocks.AspNetCore.csproj", "MicroStore.BuildingBlocks.AspNetCore/"]
COPY ["MicroStore.BuildingBlocks.Utils/MicroStore.BuildingBlocks.Utils.csproj", "MicroStore.BuildingBlocks.Utils/"]
COPY ["MicroStore.ShoppingGateway.ClinetSdk/MicroStore.ShoppingGateway.ClinetSdk.csproj", "MicroStore.ShoppingGateway.ClinetSdk/"]
RUN dotnet restore "./MicroStore.Client.PublicWeb/MicroStore.Client.PublicWeb.csproj"
COPY . .
WORKDIR "/src/MicroStore.Client.PublicWeb"
RUN apt-get update -yq && apt-get upgrade -yq && apt-get install -yq curl git nano
RUN curl -sL https://deb.nodesource.com/setup_20.x | bash - && apt-get install -yq nodejs build-essential
RUN npm install
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet tool install --global Volo.Abp.Cli --version 6.0.0
RUN abp install-libs
RUN dotnet build "./MicroStore.Client.PublicWeb.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MicroStore.Client.PublicWeb.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroStore.Client.PublicWeb.dll"]