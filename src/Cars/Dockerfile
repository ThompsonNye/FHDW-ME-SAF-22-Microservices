FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY Shared/Shared.csproj Shared/
RUN dotnet restore Shared/Shared.csproj
COPY Cars/Cars.csproj Cars/
RUN dotnet restore Cars/Cars.csproj
COPY Shared/ Shared/
COPY Cars/ Cars/
WORKDIR /src/Cars
RUN dotnet build "Cars.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cars.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cars.dll"]
