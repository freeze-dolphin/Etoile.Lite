FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Etoile.Lite/Etoile.Lite.csproj", "Etoile.Lite/"]
RUN dotnet restore "Etoile.Lite/Etoile.Lite.csproj"
COPY . .
WORKDIR "/src/Etoile.Lite"
RUN dotnet build "./Etoile.Lite.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Etoile.Lite.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Etoile.Lite.dll"]
