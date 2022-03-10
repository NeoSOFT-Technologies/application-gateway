#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Infrastructure/ApplicationGateway.Worker/ApplicationGateway.Worker.csproj", "src/Infrastructure/ApplicationGateway.Worker/"]
COPY ["src/Core/ApplicationGateway.Application/ApplicationGateway.Application.csproj", "src/Core/ApplicationGateway.Application/"]
COPY ["src/Core/ApplicationGateway.Domain/ApplicationGateway.Domain.csproj", "src/Core/ApplicationGateway.Domain/"]
RUN dotnet restore "src/Infrastructure/ApplicationGateway.Worker/ApplicationGateway.Worker.csproj"
COPY . .
WORKDIR "/src/src/Infrastructure/ApplicationGateway.Worker"
RUN dotnet build "ApplicationGateway.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApplicationGateway.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApplicationGateway.Worker.dll"]