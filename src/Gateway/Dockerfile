FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY ["Nuyken.Vegasco.Backend.Microservices.Gateway.csproj", "Nuyken.Vegasco.Backend.Microservices.Gateway/"]
RUN dotnet restore "Nuyken.Vegasco.Backend.Microservices.Gateway/Nuyken.Vegasco.Backend.Microservices.Gateway.csproj"
COPY . ./Nuyken.Vegasco.Backend.Microservices.Gateway
WORKDIR "/src/Nuyken.Vegasco.Backend.Microservices.Gateway"
RUN dotnet build "Nuyken.Vegasco.Backend.Microservices.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Nuyken.Vegasco.Backend.Microservices.Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nuyken.Vegasco.Backend.Microservices.Gateway.dll"]
