#############################################
# Build Stage (Dotnet)
#############################################
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS dotnet-builder

# Project
WORKDIR /workdir

# Restore
COPY src/shared/NomaNova.Ojeda.Models/NomaNova.Ojeda.Models.csproj ./src/shared/NomaNova.Ojeda.Models/
RUN dotnet restore src/shared/NomaNova.Ojeda.Models/NomaNova.Ojeda.Models.csproj

COPY src/api/NomaNova.Ojeda.Core/NomaNova.Ojeda.Core.csproj ./src/api/NomaNova.Ojeda.Core/
RUN dotnet restore src/api/NomaNova.Ojeda.Core/NomaNova.Ojeda.Core.csproj

COPY src/api/NomaNova.Ojeda.Data/NomaNova.Ojeda.Data.csproj ./src/api/NomaNova.Ojeda.Data/
RUN dotnet restore src/api/NomaNova.Ojeda.Data/NomaNova.Ojeda.Data.csproj

COPY src/api/NomaNova.Ojeda.Services/NomaNova.Ojeda.Services.csproj ./src/api/NomaNova.Ojeda.Services/
RUN dotnet restore src/api/NomaNova.Ojeda.Services/NomaNova.Ojeda.Services.csproj

COPY src/api/NomaNova.Ojeda.Api/NomaNova.Ojeda.Api.csproj ./src/api/NomaNova.Ojeda.Api/
RUN dotnet restore src/api/NomaNova.Ojeda.Api/NomaNova.Ojeda.Api.csproj

COPY test/api/NomaNova.Ojeda.Api.Tests/NomaNova.Ojeda.Api.Tests.csproj ./test/api/NomaNova.Ojeda.Api.Tests/
RUN dotnet restore test/api/NomaNova.Ojeda.Api.Tests/NomaNova.Ojeda.Api.Tests.csproj

# Copy source
COPY . .

# Run tests
WORKDIR ./test/api/NomaNova.Ojeda.Api.Tests
RUN dotnet test --collect:"XPlat Code Coverage"
WORKDIR ../../..

# Publish
RUN dotnet publish src/api/NomaNova.Ojeda.Api/NomaNova.Ojeda.Api.csproj -c Release -o /publish

#############################################
# Runtime Stage
#############################################
FROM mcr.microsoft.com/dotnet/aspnet:5.0

COPY --from=dotnet-builder /publish /publish

WORKDIR /publish

ENV DOTNET_USE_POLLING_FILE_WATCHER=true
ENV ASPNETCORE_ENVIRONMENT Development
ENV ASPNETCORE_URLS http://*:80

EXPOSE 80

ENTRYPOINT [ "dotnet", "NomaNova.Ojeda.Api.dll" ]
