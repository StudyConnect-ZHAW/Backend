# StudyConnect Back-End

This repository holds the backend of the StudyConnect Application.
For now only a demo API is available to interface with from the frontend.

## Prerequisites

Before running the application, ensure the following are installed on your system:
- **.NET** (TODO: Version)
- **Docker (Engine and CLI):** Refer to the official website
- **Docker Compose**

## Setup

1. Clone the repository:
```
git clone https://github.com/StudyConnect-ZHAW/Backend.git
cd Backend
```
2. Create the environment file:
```
cp .example.env .env
```
Replace the placeholder values with the actual values inside `.env`.

## Running the Backend

### Option 1: Using Docker Compose

Rebuild and start the backend with the following docker Build commands from the root of the repository:
```
docker-compose -f ./compose.yaml up --build --force-recreate
```

Afterwards the swagger API will be available under http://localhost:8080/swagger/.

### Option 2: Using Provided Start Script

Alternatively, you can also use the included script to build the container and open the API in the browser:

**On Linux / MacOS:**
```
./start-and-open.sh
```

**On Windows (PowerShell):**
```
.\start-and-open.ps1
```
