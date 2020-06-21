#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Catalogue.Api/Catalogue.Api.csproj", "Catalogue.Api/"]
RUN dotnet restore "Catalogue.Api/Catalogue.Api.csproj"
COPY . .
WORKDIR "/src/Catalogue.Api"
RUN dotnet build "Catalogue.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Catalogue.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalogue.Api.dll"]