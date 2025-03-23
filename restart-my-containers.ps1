param (
	[string]$container = ""
)
docker-compose stop $container
docker-compose build $container
docker-compose up -d $container

Write-Host "Containers done and dusted!" -ForegroundColor Magenta
docker ps

