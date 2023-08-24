FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
WORKDIR /src
COPY Catalog.API/*.csproj Catalog.API/
COPY Catalog.Core/*.csproj Catalog.Core/
COPY Catalog.DataAccess/*.csproj Catalog.DataAccess/
COPY Catalog.Infrastructure/*.csproj Catalog.Infrastructure/
COPY Catalog.IntegrationTests/*.csproj Catalog.IntegrationTests/
COPY Catalog.UnitTests/*.csproj Catalog.UnitTests/
COPY *.sln .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release

FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime
WORKDIR /publish
COPY --from=build src/Catalog.API/bin/Release/net7.0/publish .
EXPOSE 80
ENTRYPOINT [ "dotnet", "Catalog.API.dll" ]