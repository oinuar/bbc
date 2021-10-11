FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["tech.haamu.Movie.sln", "."]
COPY ["tech.haamu.Movie/tech.haamu.Movie.csproj", "tech.haamu.Movie/"]
COPY ["tech.haamu.Movie.UnitTest/tech.haamu.Movie.UnitTest.csproj", "tech.haamu.Movie.UnitTest/"]
COPY ["tech.haamu.Movie.IntegrationTest/tech.haamu.Movie.IntegrationTest.csproj", "tech.haamu.Movie.IntegrationTest/"]
RUN dotnet restore "."
COPY . .
RUN dotnet build "." -c Release -o /app/build

FROM build AS qa
RUN dotnet test "tech.haamu.Movie.UnitTest/" && dotnet test "tech.haamu.Movie.IntegrationTest/"

FROM build AS publish
RUN dotnet publish "tech.haamu.Movie/" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "tech.haamu.Movie.dll" ]
