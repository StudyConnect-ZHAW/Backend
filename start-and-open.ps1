# Check if the Docker network exists
$networkExists = docker network ls --format "{{.Name}}" | Where-Object { $_ -eq "studyconnect-network" }

if (-not $networkExists) {
    docker network create studyconnect-network
}

# Start the Docker containers
docker-compose -f ./compose.yaml up --build -d

# Wait for the application to respond on /health
$maxRetries = 30
$retryCount = 0
$healthy = $false

while (-not $healthy -and $retryCount -lt $maxRetries) {
    try {
        $response = curl http://localhost:8080/health -UseBasicParsing
        if ($response.StatusCode -eq 200 -and $response.Content -match "Healthy") {
            $healthy = $true
            Write-Host "Application is healthy! Opening Swagger UI..."
        } else {
            Write-Host "Health check failed. Status code: $($response.StatusCode)"
        }
    } catch {
        Write-Host "Waiting for health check to pass... ($_)" 
    }
}

# Open the Swagger UI in the default web browser
Start-Process "http://localhost:8080/swagger/"