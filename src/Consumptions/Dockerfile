FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Consumptions.csproj", "Consumptions/"]
RUN dotnet restore "Consumptions/Consumptions.csproj"
COPY . "./Consumptions"
WORKDIR "/src/Consumptions"
RUN dotnet publish "Consumptions.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Consumptions.dll"]
