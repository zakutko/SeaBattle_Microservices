#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/Game/Game.API/Game.API.csproj", "src/Services/Game/Game.API/"]
COPY ["src/Contracts/SeaBattle.Contracts/SeaBattle.Contracts.csproj", "src/Contracts/SeaBattle.Contracts/"]
COPY ["src/Services/Game/Game.BLL/Game.BLL.csproj", "src/Services/Game/Game.BLL/"]
COPY ["src/Services/Game/Game.DAL/Game.DAL.csproj", "src/Services/Game/Game.DAL/"]
RUN dotnet restore "src/Services/Game/Game.API/Game.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Game/Game.API"
RUN dotnet build "Game.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Game.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Game.API.dll"]