FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine as BUILD

ENV DOTNET_CLI_TELEMETRY_OPTOUT=1

RUN mkdir /out
WORKDIR /build

COPY ["Stalkr/Stalkr.csproj", "Stalkr/"]
RUN dotnet restore Stalkr/Stalkr.csproj

COPY ["Stalkr", "Stalkr/"]
RUN dotnet build Stalkr/Stalkr.csproj --configuration Release --no-restore
RUN dotnet publish Stalkr/Stalkr.csproj --no-restore --no-build --configuration Release --output /out

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-alpine as RUNTIME

LABEL author="Daniel Lerps"
LABEL contact="contact@lerps.de"
LABEL app="de.lerps.stalkr"

ENV DOTNET_CLI_TELEMETRY_OPTOUT=1

WORKDIR /app
COPY --from=BUILD /out .

ENTRYPOINT [ "dotnet", "Stalkr.dll" ]