FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim AS build
COPY ChurchManager.sln .
RUN dotnet restore 
COPY . .

FROM build AS publish
RUN dotnet publish --no-restore ./src/API/ChurchManager.Api/ChurchManager.Api.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# default environment variables - this will be set when we run the container again
ENV ASPNETCORE_ENVIRONMENT=Production 

ENTRYPOINT ["dotnet", "ChurchManager.Api.dll"]