# StudyConnect Back-End
This repository holds the backend of the StudyConnect Application.
For now only a demo API is available to interface with from the frontend.

## Setup Docker Environment
Start backend with the following docker Build commands from inside the StudyConnect.API folder:
> docker build -t sc-backend -f Dockerfile .
> docker run --name sc-backend -e ASPNETCORE_URLS="http://0.0.0.0:8080" -p 8080:8080 sc-backend

The [https://github.com/StudyConnect-ZHAW/Frontend|Frontend] should be built and started with the following:
> docker build -t studyconnect-frontend -f docker/Dockerfile.frontend .    
> docker run -e NEXT_PUBLIC_API_URL="http://localhost:8080/swagger/index.html" -p 3000:3000 studyconnect-frontend

