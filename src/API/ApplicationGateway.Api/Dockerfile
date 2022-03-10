#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/API/ApplicationGateway.Api/ApplicationGateway.Api.csproj", "src/API/ApplicationGateway.Api/"]
COPY ["src/Infrastructure/ApplicationGateway.Identity/ApplicationGateway.Identity.csproj", "src/Infrastructure/ApplicationGateway.Identity/"]
COPY ["src/Core/ApplicationGateway.Application/ApplicationGateway.Application.csproj", "src/Core/ApplicationGateway.Application/"]
COPY ["src/Core/ApplicationGateway.Domain/ApplicationGateway.Domain.csproj", "src/Core/ApplicationGateway.Domain/"]
COPY ["src/Infrastructure/ApplicationGateway.Persistence/ApplicationGateway.Persistence.csproj", "src/Infrastructure/ApplicationGateway.Persistence/"]
COPY ["src/Infrastructure/ApplicationGateway.Infrastructure/ApplicationGateway.Infrastructure.csproj", "src/Infrastructure/ApplicationGateway.Infrastructure/"]
RUN dotnet restore "src/API/ApplicationGateway.Api/ApplicationGateway.Api.csproj"
COPY . .
WORKDIR "/src/src/API/ApplicationGateway.Api"
RUN dotnet build "ApplicationGateway.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApplicationGateway.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApplicationGateway.Api.dll"]