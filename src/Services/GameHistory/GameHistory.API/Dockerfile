#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/GameHistory/GameHistory.API/GameHistory.API.csproj", "src/Services/GameHistory/GameHistory.API/"]
COPY ["src/Contracts/SeaBattle.Contracts/SeaBattle.Contracts.csproj", "src/Contracts/SeaBattle.Contracts/"]
COPY ["src/Services/GameHistory/GameHistory.BLL/GameHistory.BLL.csproj", "src/Services/GameHistory/GameHistory.BLL/"]
COPY ["src/Services/GameHistory/GameHistory.DAL/GameHistory.DAL.csproj", "src/Services/GameHistory/GameHistory.DAL/"]
RUN dotnet restore "src/Services/GameHistory/GameHistory.API/GameHistory.API.csproj"
COPY . .
WORKDIR "/src/src/Services/GameHistory/GameHistory.API"
RUN dotnet build "GameHistory.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameHistory.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameHistory.API.dll"]