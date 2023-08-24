# Add EF Core Design Package
dotnet add package Microsoft.EntityFrameworkCore.Design

# Install EF Core Tools
dotnet tool install --global dotnet-ef

# Add Migration
dotnet ef migrations add InitialMigration --project .\Catalog.DataAccess --startup-project .\Catalog.API\

# Update Database
dotnet ef database update --project .\Catalog.DataAccess --startup-project .\Catalog.API\

# Run project
dotnet run --project ./Catalog.API

# Build Catalog servie image
docker build --tag catalog-service .

# Run Catalog service container
docker run -d --name catalog-service -e "ASPNETCORE_ENVIRONMENT=Production" --network catalog-database-net -p 5101:80 catalog-service

# Pull SQL Server image
docker pull mcr.microsoft.com/mssql/server:2022-latest

# Create SQL Server volume
docker volume create catalog-database-data

# Create SQL Server network
docker network create catalog-database-net

# Run SQL Server container
docker run --rm -d -v catalog-database-data:/var/opt/mssql --network catalog-database-net -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=pa55word!" -p 5434:1433 --name catalog-database --hostname catalog-database mcr.microsoft.com/mssql/server:2022-latest

# Run interactive inside SQL Server container
docker exec -ti catalog-database-engine bash