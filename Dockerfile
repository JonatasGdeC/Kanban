FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Kanban.sln ./
COPY src/Backend ./src/Backend
COPY src/Shared ./src/Shared

RUN dotnet restore src/Backend/Kanban.Api/Kanban.Api.csproj

RUN dotnet publish src/Backend/Kanban.Api/Kanban.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Kanban.Api.dll"]