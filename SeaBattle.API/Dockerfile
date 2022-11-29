#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SeaBattle.API/SeaBattle.API.csproj", "SeaBattle.API/"]
COPY ["src/Contracts/SeaBattle.Contracts/SeaBattle.Contracts.csproj", "src/Contracts/SeaBattle.Contracts/"]
RUN dotnet restore "SeaBattle.API/SeaBattle.API.csproj"
COPY . .
WORKDIR "/src/SeaBattle.API"
RUN dotnet build "SeaBattle.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SeaBattle.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SeaBattle.API.dll"]