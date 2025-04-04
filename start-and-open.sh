#!/bin/bash
# Attempt to detect OS and open URL

docker-compose -f StudyConnect.API/compose.yaml up --build -d

# Wait for the container to be ready
sleep 5

# OS Detection and URL Opening
if [[ "$OSTYPE" == "darwin"* ]]; then
    open http://localhost:8080/swagger/  # macOS
elif [[ "$OSTYPE" == "linux-gnu"* ]]; then
    if command -v xdg-open &> /dev/null; then
        xdg-open http://localhost:8080/swagger/ # Most Linux
    elif command -v gnome-open &> /dev/null; then
        gnome-open http://localhost:8080/swagger/ # Gnome Desktop
    elif command -v kde-open &> /dev/null; then
        kde-open http://localhost:8080/swagger/ # KDE Desktop
    elif command -v firefox &> /dev/null; then
        firefox http://localhost:8080/swagger/ # if firefox is installed
    elif command -v chrome &> /dev/null; then
        chrome http://localhost:8080/swagger/ # if chrome is installed
    else
        echo "Could not find a suitable command to open the URL on Linux."
    fi
elif [[ "$OSTYPE" == "cygwin" ]] || [[ "$OSTYPE" == "msys" ]] || [[ "$OSTYPE" == "win32" ]]; then
        start http://localhost:8080/swagger/ # Windows
else
    echo "Unsupported OS."
fi