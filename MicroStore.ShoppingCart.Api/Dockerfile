#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MicroStore.ShoppingCart.Api/MicroStore.ShoppingCart.Api.csproj", "MicroStore.ShoppingCart.Api/"]
COPY ["MicroStore.BuildingBlocks.AspNetCore/MicroStore.BuildingBlocks.AspNetCore.csproj", "MicroStore.BuildingBlocks.AspNetCore/"]
COPY ["MicroStore.BuildingBlocks.Utils/MicroStore.BuildingBlocks.Utils.csproj", "MicroStore.BuildingBlocks.Utils/"]
RUN dotnet restore "./MicroStore.ShoppingCart.Api/MicroStore.ShoppingCart.Api.csproj"
COPY . .
WORKDIR "/src/MicroStore.ShoppingCart.Api"
RUN dotnet build "./MicroStore.ShoppingCart.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MicroStore.ShoppingCart.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroStore.ShoppingCart.Api.dll"]