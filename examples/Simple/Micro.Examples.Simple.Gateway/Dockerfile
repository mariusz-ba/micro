FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

COPY examples/Simple/Micro.Examples.Simple.Gateway/. ./examples/Simple/Micro.Examples.Simple.Gateway/
COPY src/. ./src/

RUN dotnet restore examples/Simple/Micro.Examples.Simple.Gateway
RUN dotnet publish examples/Simple/Micro.Examples.Simple.Gateway -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

COPY --from=build /app/out .

ENV ASPNETCORE_ENVIRONMENT=Docker

ENTRYPOINT ["dotnet", "Micro.Examples.Simple.Gateway.dll"]