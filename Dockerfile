# Unter https://aka.ms/customizecontainer erfahren Sie, wie Sie Ihren Debugcontainer anpassen und wie Visual Studio dieses Dockerfile verwendet, um Ihre Images für ein schnelleres Debuggen zu erstellen.

# Diese Stufe wird verwendet, wenn sie von VS im Schnellmodus ausgeführt wird (Standardeinstellung für Debugkonfiguration).
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Diese Stufe wird zum Erstellen des Dienstprojekts verwendet.
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

# Diese Stufe wird verwendet, um das Dienstprojekt zu veröffentlichen, das in die letzte Phase kopiert werden soll.
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "StudyConnect.API/StudyConnect.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Diese Stufe wird in der Produktion oder bei Ausführung von VS im regulären Modus verwendet (Standard, wenn die Debugkonfiguration nicht verwendet wird).
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN addgroup --gid 1000 appuser \
    && adduser --uid 1000 --ingroup appuser --home /app appuser \
    && chown -R appuser:appuser /app

USER appuser

ENTRYPOINT ["dotnet", "StudyConnect.API.dll"]