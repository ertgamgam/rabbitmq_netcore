FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY Consumer/Consumer.csproj Consumer/
RUN dotnet restore Consumer/Consumer.csproj
COPY . .
WORKDIR /src/Consumer
RUN dotnet build Consumer.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Consumer.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Consumer.dll"]
