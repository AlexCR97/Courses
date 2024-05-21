```
// Install Ef Core cli tool 
dotnet tool install --global dotnet-ef

// Base command
dotnet ef

// Add a migration
dotnet ef migrations add 0001_Init --namespace EntityFramework.Migrations --startup-project ../AntuDevOps.PointOfSale.Api/AntuDevOps.PointOfSale.Api.csproj

// Update the database
// Run from where your DbContext is
dotnet ef database update --startup-project ../AntuDevOps.PointOfSale.Api/AntuDevOps.PointOfSale.Api.csproj
```
