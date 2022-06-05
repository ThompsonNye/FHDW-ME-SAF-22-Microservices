FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY ["Gateway.csproj", "Gateway/"]
RUN dotnet restore "Gateway/Gateway.csproj"
COPY . ./Gateway
WORKDIR "/src/Gateway"
RUN dotnet build "Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gateway.csproj" \
    -c Release \
    -o /app/publish \
    --runtime linux-musl-x64 \
    --no-self-contained \
    -p:PublishReadyToRun=true \
    -p:PublishReadyToRunShowWarnings=true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#COPY ocelot-config/ocelot.global.json ./ocelot.global.json
#COPY ocelot-config/ocelot.json ./ocelot.json
ENTRYPOINT ["dotnet", "Gateway.dll"]
