#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MicroStore.Bff.Shopping/MicroStore.Bff.Shopping.csproj", "MicroStore.Bff.Shopping/"]
COPY ["MicroStore.BuildingBlocks.AspNetCore/MicroStore.BuildingBlocks.AspNetCore.csproj", "MicroStore.BuildingBlocks.AspNetCore/"]
COPY ["MicroStore.BuildingBlocks.Utils/MicroStore.BuildingBlocks.Utils.csproj", "MicroStore.BuildingBlocks.Utils/"]
RUN dotnet restore "./MicroStore.Bff.Shopping/MicroStore.Bff.Shopping.csproj"
COPY . .
WORKDIR "/src/MicroStore.Bff.Shopping"
RUN dotnet build "./MicroStore.Bff.Shopping.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MicroStore.Bff.Shopping.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroStore.Bff.Shopping.dll"]