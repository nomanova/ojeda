#############################################
# Build Stage (Dotnet)
#############################################
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS dotnet-builder

ARG SONAR_PROJECT_KEY=nomanova_ojeda_web
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
  /n:"ojeda - web" \
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

COPY src/shared/NomaNova.Ojeda.Client/NomaNova.Ojeda.Client.csproj ./src/shared/NomaNova.Ojeda.Client/
RUN dotnet restore src/shared/NomaNova.Ojeda.Client/NomaNova.Ojeda.Client.csproj

COPY src/web/NomaNova.Ojeda.Web/NomaNova.Ojeda.Web.csproj ./src/web/NomaNova.Ojeda.Web/
RUN dotnet restore src/web/NomaNova.Ojeda.Web/NomaNova.Ojeda.Web.csproj

COPY test/web/NomaNova.Ojeda.Web.Tests/NomaNova.Ojeda.Web.Tests.csproj ./test/web/NomaNova.Ojeda.Web.Tests/
RUN dotnet restore test/web/NomaNova.Ojeda.Web.Tests/NomaNova.Ojeda.Web.Tests.csproj

# Copy source
COPY . .

# Run tests
WORKDIR ./test/shared/NomaNova.Ojeda.Utils.Tests
RUN dotnet test \
  /p:CollectCoverage=true \
  /p:CoverletOutputFormat=opencover
WORKDIR ../../..

WORKDIR ./test/web/NomaNova.Ojeda.Web.Tests
RUN dotnet test \
  /p:CollectCoverage=true \
  /p:CoverletOutputFormat=opencover
WORKDIR ../../..

# Publish
RUN dotnet publish src/web/NomaNova.Ojeda.Web/NomaNova.Ojeda.Web.csproj -c Release -o /publish

# End Sonar Scanner
RUN dotnet sonarscanner end /d:sonar.login="$SONAR_TOKEN"

#############################################
# Runtime Stage
#############################################
FROM nginx:1.15

COPY --from=dotnet-builder /publish/wwwroot /usr/share/nginx/html


ADD build/web/default.conf /etc/nginx/conf.d/default.conf
ADD build/web/nginx.conf /etc/nginx/nginx.conf
ADD build/web/run.sh /root/run.sh

ENV API_LOCATION https://localhost:5001

CMD sh /root/run.sh
