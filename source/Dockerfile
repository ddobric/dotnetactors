# Docker file
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env

WORKDIR /app

COPY ["ActorLibrary/ActorLibrary.csproj", "ActorLibrary/"]
COPY ["DotNetActors/DotNetActors.csproj", "DotNetActors/"]
COPY ["DotNetActorsHost/DotNetActorsHost.csproj", "DotNetActorsHost/"]
RUN dotnet restore "DotNetActorsHost/DotNetActorsHost.csproj"


COPY . ./
WORKDIR /app/DotNetActorsHost
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/sdk:5.0
WORKDIR /app
COPY --from=build-env /app .
ENTRYPOINT ["dotnet", "dotnetactorhost.dll"]