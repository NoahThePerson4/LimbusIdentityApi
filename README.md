Limbus Company is made by project moon.
https://limbuscompany.wiki.gg/wiki/Limbus_Company_Wiki is also were a big amount of my information came from. Thanks limbuscompany.wiki.gg for compiling all the identity data.

For the Docker Image you must build it with docker run -p 1433:1433  --name LimbusTestNew2 --hostname sqlpreview -d noahtheperson/limbusidentitiescontainer:latest 
after pulling it with docker pull noahtheperson/limbusidentitiescontainer

The Connection String should be Data Source=localhost; Initial Catalog=IdentityDB; User Id=sa; Password=MySaPassword123; TrustServerCertificate=True; (This should already be in the repository).
