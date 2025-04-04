# StudyConnect Back-End
This repository holds the backend of the StudyConnect Application.
For now only a demo API is available to interface with from the frontend.

## Setup Docker Environment
Rebuild and start the backend with the following docker Build commands from the root of the repository:
> docker-compose -f StudyConnect.API/compose.yaml up --build --force-recreate
Afterwards the swagger API will be available under http://localhost:8080/swagger/

Alternatively you can also use the start-and-open.sh script to build the container and open the API in the browser:
Linux / MacOS:
> ./start-and-open.sh
Windows
> run start-and-open.sh
