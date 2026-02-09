.PHONY: up down clean logs lint fmt test

up:
	docker-compose up --build

down:
	docker-compose down

clean:
	docker-compose down -v
	dotnet clean

logs:
	docker-compose logs -f api

lint:
	dotnet format --verify-no-changes

fmt:
	dotnet format

test:
	dotnet test