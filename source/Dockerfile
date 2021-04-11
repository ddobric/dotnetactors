FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /src
COPY ["DotNetActorsHost/DotNetActorsHost.csproj", "DotNetActorsHost/"]
COPY ["DotNetActors/DotNetActors.csproj", "DotNetActors/"]
COPY ["ActorLibrary/ActorLibrary.csproj", "ActorLibrary/"]
RUN dotnet restore "DotNetActorsHost/DotNetActorsHost.csproj"
COPY . .
WORKDIR "/src/DotNetActorsHost"
RUN dotnet build "DotNetActorsHost.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DotNetActorsHost.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DotNetActorsHost.dll"]