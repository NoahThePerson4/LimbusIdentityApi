Limbus Company is made by project moon.
https://limbuscompany.wiki.gg/wiki/Limbus_Company_Wiki is also where a big amount of my information came from. Thanks limbuscompany.wiki.gg for compiling all the identity data.

For the Docker image use this command if you want the database to have things in it. (Note put an actual password instead of just YOURSQLSERVERPASSWORD)
docker run --env=SQLCMDPASSWORD='YOURSQLSERVERPASSWORD' --env=MSSQL_SA_PASSWORD='YOURSQLSERVERPASSWORD' --env=SQLCMDUSER=sa -p 1433:1433 --restart=no --label='com.microsoft.product=Microsoft SQL Server' --label='com.microsoft.version=16.0.4115.5' --label='vendor=Microsoft' --runtime=runc -d noahtheperson/identitydb:latest

If you want a fresh empty database use this insted. (Note put an actual password instead of just YOURSQLSERVERPASSWORD)
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YOURSQLSERVERPASSWORD" -e "MSSQL_PID=Evaluation" -p 1433:1433  --name sqlpreview --hostname sqlpreview -d mcr.microsoft.com/mssql/server:2022-preview-ubuntu-22.04

then migrate the database to add the tables to the docker container (You can skip this step if you used the top docker run command).

This is the connection string format. Just put it in your apppsettings.json and appsettings.Development.json. (Note put an actual password instead of just YOURSQLSERVERPASSWORD)
Data Source=localhost; Initial Catalog=identitydb; User Id=sa; Password=YOURSQLSERVERPASSWORD; TrustServerCertificate=True;
