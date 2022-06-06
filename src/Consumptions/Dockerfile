FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY Shared/Shared.csproj Shared/
RUN dotnet restore Shared/Shared.csproj
COPY Consumptions/Consumptions.csproj Consumptions/
RUN dotnet restore Consumptions/Consumptions.csproj
COPY Shared/ Shared/
COPY Consumptions/ Consumptions/
WORKDIR /src/Consumptions
RUN dotnet publish "Consumptions.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Consumptions.dll"]
