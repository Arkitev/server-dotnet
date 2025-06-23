FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY server-dotnet/server-dotnet.sln ./
COPY server-dotnet/*.csproj ./server-dotnet/
COPY server-dotnet-dal/*.csproj ./server-dotnet-dal/

RUN dotnet restore ./server-dotnet.sln

COPY server-dotnet ./server-dotnet/
COPY server-dotnet-dal ./server-dotnet-dal/

WORKDIR /app/server-dotnet
RUN dotnet publish -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /publish .

ENTRYPOINT ["dotnet", "server-dotnet.dll"]