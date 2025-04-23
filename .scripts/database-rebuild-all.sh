#!/bin/bash
#To make the .sh file executable
#sudo chmod +x ./database-rebuild-all.sh

# To execute:
# ./database-rebuild-all.sh seed

#Which database server
DBContext="SqlServerDbContext"
#DBContext="MySqlDbContext"
#DBContext="PostgresDbContext"

#drop the database
#dotnet ef database drop -f -c SqlServerDbContext -p ../DbContext -s ../DbContext
#dotnet ef database drop -f -c MySqlDbContext -p ../DbContext -s ../DbContext
#dotnet ef database drop -f -c PostgresDbContext -p ../DbContext -s ../DbContext
dotnet ef database drop -f -c $DBContext -p ../DbContext -s ../DbContext

#remove any migration
#rm -rf ../DbContext/Migrations/SqlServerDbContext
#rm -rf ../DbContext/Migrations/MySqlDbContext
#rm -rf ../DbContext/Migrations/PostgresDbContext
rm -rf ../DbContext/Migrations/$DBContext

#make a full new migration
#dotnet ef migrations add miInitial -c SqlServerDbContext -p ../DbContext -s ../DbContext -o ../DbContext/Migrations/SqlServerDbContext
#dotnet ef migrations add miInitial -c MySqlDbContext -p ../DbContext -s ../DbContext -o ../DbContext/Migrations/MySqlDbContext
#dotnet ef migrations add miInitial -c PostgresDbContext -p ../DbContext -s ../DbContext -o ../DbContext/Migrations/PostgresDbContext
dotnet ef migrations add miInitial -c $DBContext -p ../DbContext -s ../DbContext -o ../DbContext/Migrations/$DBContext


#update the database from the migration
#dotnet ef database update -c SqlServerDbContext -p ../DbContext -s ../DbContext
#dotnet ef database update -c MySqlDbContext -p ../DbContext -s ../DbContext
#dotnet ef database update -c PostgresDbContext -p ../DbContext -s ../DbContext
dotnet ef database update -c $DBContext -p ../DbContext -s ../DbContext

# Check for 'seed' argument and seed the database if present
if [[ $1 == "seed" ]]; then
    #seed the database
    cd ../AppSeeder
    dotnet run
fi
