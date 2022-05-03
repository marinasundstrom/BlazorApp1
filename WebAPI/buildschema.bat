rmdir /S /Q "Data/Migrations"

dotnet ef migrations add CreateIdentitySchema -c ApplicationDbContext -o Data/Migrations
