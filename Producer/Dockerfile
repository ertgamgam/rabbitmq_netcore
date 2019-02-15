FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY Producer/Producer.csproj Producer/
RUN dotnet restore Producer/Producer.csproj
COPY . .
WORKDIR /src/Producer
RUN dotnet build Producer.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Producer.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Producer.dll"]
