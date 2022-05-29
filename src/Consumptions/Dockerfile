FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Consumptions/Consumptions.csproj", "Consumptions/"]
RUN dotnet restore "src/Consumptions/Consumptions.csproj"
COPY . .
WORKDIR "/src/Consumptions"
RUN dotnet build "Consumptions.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Consumptions.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Consumptions.dll"]
