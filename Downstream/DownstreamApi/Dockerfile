#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Downstream/DownstreamApi/DownstreamApi.csproj", "Downstream/DownstreamApi/"]
RUN dotnet restore "Downstream/DownstreamApi/DownstreamApi.csproj"
COPY . .
WORKDIR "/src/Downstream/DownstreamApi"
RUN dotnet build "DownstreamApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DownstreamApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DownstreamApi.dll"]