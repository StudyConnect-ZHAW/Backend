# Under https://aka.ms/customizecontainer you can learn how to customize your debug container and how Visual Studio uses this Dockerfile to create your images for faster debugging.

# This Dockerfile is used to build the image for the ASP.NET Core application.
# It consists of multiple stages to optimize the build process and reduce the final image size.
# The first stage is the base image, which is used to run the application.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the application in a container.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["StudyConnect.API/StudyConnect.API.csproj", "StudyConnect.API/"]
COPY ["StudyConnect.Core/StudyConnect.Core.csproj", "StudyConnect.Core/"]
COPY ["StudyConnect.Data/StudyConnect.Data.csproj", "StudyConnect.Data/"]
RUN dotnet restore "StudyConnect.API/StudyConnect.API.csproj"

COPY StudyConnect.API/ ./StudyConnect.API/
COPY StudyConnect.Core/ ./StudyConnect.Core/
COPY StudyConnect.Data/ ./StudyConnect.Data/

RUN dotnet build "StudyConnect.API/StudyConnect.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project that should be copied to the final stage.
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "StudyConnect.API/StudyConnect.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running VS in regular mode (default when not using the debug configuration).
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN addgroup --gid 1000 appuser \
    && adduser --uid 1000 --ingroup appuser --home /app appuser \
    && chown -R appuser:appuser /app

USER appuser

ENTRYPOINT ["dotnet", "StudyConnect.API.dll"]