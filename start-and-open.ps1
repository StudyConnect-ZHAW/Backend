docker-compose -f ./compose.yaml up --build -d
Start-Sleep -Seconds 5
Start-Process "http://localhost:8080/swagger/"