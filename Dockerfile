FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY fcg-shared-events/ fcg-shared-events/
COPY fcg-users-api/ fcg-users-api/
WORKDIR /src/fcg-users-api
RUN dotnet restore src/UsersAPI/UsersAPI.csproj
RUN dotnet publish src/UsersAPI/UsersAPI.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "UsersAPI.dll"]