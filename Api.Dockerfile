#############################################
# Build Stage (Dotnet)
#############################################
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS dotnet-builder

ARG SONAR_PROJECT_KEY=nomanova_ojeda_api
ARG SONAR_OGRANIZAION_KEY=nomanova
ARG SONAR_HOST_URL=https://sonarcloud.io
ARG SONAR_TOKEN

WORKDIR /workdir

# Install Sonar Scanner, Coverlet and Java
RUN apt-get update && apt-get install -y openjdk-11-jdk
RUN dotnet tool install --global dotnet-sonarscanner
RUN dotnet tool install --global coverlet.console
ENV PATH="$PATH:/root/.dotnet/tools"

# Start Sonar Scanner
RUN dotnet sonarscanner begin \
  /k:"$SONAR_PROJECT_KEY" \
  /o:"$SONAR_OGRANIZAION_KEY" \
  /n:"ojeda - api" \
  /d:sonar.host.url="$SONAR_HOST_URL" \
  /d:sonar.login="$SONAR_TOKEN" \
  /d:sonar.cs.opencover.reportsPaths="**/coverage.opencover.xml" \
  /d:sonar.coverage.exclusions="**Tests*.cs"

# Restore
COPY src/shared/NomaNova.Ojeda.Utils/NomaNova.Ojeda.Utils.csproj ./src/shared/NomaNova.Ojeda.Utils/
RUN dotnet restore src/shared/NomaNova.Ojeda.Utils/NomaNova.Ojeda.Utils.csproj

COPY test/shared/NomaNova.Ojeda.Utils.Tests/NomaNova.Ojeda.Utils.Tests.csproj ./test/shared/NomaNova.Ojeda.Utils.Tests/
RUN dotnet restore test/shared/NomaNova.Ojeda.Utils.Tests/NomaNova.Ojeda.Utils.Tests.csproj

COPY src/shared/NomaNova.Ojeda.Models/NomaNova.Ojeda.Models.csproj ./src/shared/NomaNova.Ojeda.Models/
RUN dotnet restore src/shared/NomaNova.Ojeda.Models/NomaNova.Ojeda.Models.csproj

COPY src/api/NomaNova.Ojeda.Core/NomaNova.Ojeda.Core.csproj ./src/api/NomaNova.Ojeda.Core/
RUN dotnet restore src/api/NomaNova.Ojeda.Core/NomaNova.Ojeda.Core.csproj

COPY src/api/NomaNova.Ojeda.Data/NomaNova.Ojeda.Data.csproj ./src/api/NomaNova.Ojeda.Data/
RUN dotnet restore src/api/NomaNova.Ojeda.Data/NomaNova.Ojeda.Data.csproj

COPY src/api/NomaNova.Ojeda.Services/NomaNova.Ojeda.Services.csproj ./src/api/NomaNova.Ojeda.Services/
RUN dotnet restore src/api/NomaNova.Ojeda.Services/NomaNova.Ojeda.Services.csproj

COPY test/api/NomaNova.Ojeda.Services.Tests/NomaNova.Ojeda.Services.Tests.csproj ./test/api/NomaNova.Ojeda.Services.Tests/
RUN dotnet restore test/api/NomaNova.Ojeda.Services.Tests/NomaNova.Ojeda.Services.Tests.csproj

COPY src/api/NomaNova.Ojeda.Api/NomaNova.Ojeda.Api.csproj ./src/api/NomaNova.Ojeda.Api/
RUN dotnet restore src/api/NomaNova.Ojeda.Api/NomaNova.Ojeda.Api.csproj

COPY test/api/NomaNova.Ojeda.Api.Tests/NomaNova.Ojeda.Api.Tests.csproj ./test/api/NomaNova.Ojeda.Api.Tests/
RUN dotnet restore test/api/NomaNova.Ojeda.Api.Tests/NomaNova.Ojeda.Api.Tests.csproj

# Copy source
COPY . .

# Run tests
WORKDIR ./test/shared/NomaNova.Ojeda.Utils.Tests
RUN dotnet test \
  /p:CollectCoverage=true \
  /p:CoverletOutputFormat=opencover
WORKDIR ../../..

WORKDIR ./test/api/NomaNova.Ojeda.Services.Tests
RUN dotnet test \
  /p:CollectCoverage=true \
  /p:CoverletOutputFormat=opencover
WORKDIR ../../..

WORKDIR ./test/api/NomaNova.Ojeda.Api.Tests
RUN dotnet test \
  /p:CollectCoverage=true \
  /p:CoverletOutputFormat=opencover
WORKDIR ../../..

# Publish
RUN dotnet publish src/api/NomaNova.Ojeda.Api/NomaNova.Ojeda.Api.csproj -c Release -o /publish

# End Sonar Scanner
RUN dotnet sonarscanner end /d:sonar.login="$SONAR_TOKEN"

#############################################
# Runtime Stage
#############################################
FROM mcr.microsoft.com/dotnet/aspnet:6.0

COPY --from=dotnet-builder /publish /publish

WORKDIR /publish

ENV DOTNET_USE_POLLING_FILE_WATCHER=true
ENV ASPNETCORE_ENVIRONMENT Development
ENV ASPNETCORE_URLS http://*:80

EXPOSE 80

ENTRYPOINT [ "dotnet", "NomaNova.Ojeda.Api.dll" ]
