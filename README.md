Limbus Company is made by project moon.
https://limbuscompany.wiki.gg/wiki/Limbus_Company_Wiki is also where a big amount of my information came from. Thanks limbuscompany.wiki.gg for compiling all the identity data.
This is a Blazor app and API that connect to a SQL Server Docker Container database. To run this make sure to launch both the LimbusIdentity.Api and LimbusIdentity.App at the same time (make them both start up projects).
The Blazor Home page will give you instructions on how to use the actual Blazor app. You could also make requests with Swagger but the Blazor App is more user friendly.
You can search through the list of identities, passives, or skills and even add new ones if you wish.

Please don't use this for commercial purposes.

For the Docker image use this command if you want the database to have things in it. (Note put an actual password instead of just YOURSQLSERVERPASSWORD)
docker run --env=SQLCMDPASSWORD='YOURSQLSERVERPASSWORD' --env=MSSQL_SA_PASSWORD='YOURSQLSERVERPASSWORD' --env=SQLCMDUSER=sa -p 1433:1433 --restart=no --label='com.microsoft.product=Microsoft SQL Server' --label='com.microsoft.version=16.0.4115.5' --label='vendor=Microsoft' --runtime=runc -d noahtheperson/identitydb:latest

Note the above database does not have every single identity. Instead it just has the base Identities and one identity of each faction.

If you want a fresh empty database use this instead. (Note put an actual password instead of just YOURSQLSERVERPASSWORD)
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YOURSQLSERVERPASSWORD" -e "MSSQL_PID=Evaluation" -p 1433:1433  --name sqlpreview --hostname sqlpreview -d mcr.microsoft.com/mssql/server:2022-preview-ubuntu-22.04

then migrate the database to add the tables to the docker container (You can skip this step if you used the top docker run command).

This is the connection string format. Just put it in your apppsettings.json and appsettings.Development.json. (Note put an actual password instead of just YOURSQLSERVERPASSWORD)
Data Source=localhost; Initial Catalog=identitydb; User Id=sa; Password=YOURSQLSERVERPASSWORD; TrustServerCertificate=True;

For the appsettings.json file you should structure the api appsettings.json to look like this to use serilog (jsut remove AllowedHosts for the development version).
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft.AspNetCore": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Console"
      }
    ],
    "Properties": {
      "Application": "EnemyApi"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "IdentityDbConnectionString": "Data Source=localhost; Initial Catalog=identitydb; User Id=sa; Password=YOURSQLSEVERPASSWORD; TrustServerCertificate=True;"
  }
}

NOTE. Please don't try and make a pull request to the main github repository. If you want to change any of the code Fork this repository and push to your Forked repository.
