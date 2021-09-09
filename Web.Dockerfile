#############################################
# Build Stage (Dotnet)
#############################################
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS dotnet-builder

# Project
WORKDIR /workdir

# Restore
COPY src/shared/NomaNova.Ojeda.Models/NomaNova.Ojeda.Models.csproj ./src/shared/NomaNova.Ojeda.Models/
RUN dotnet restore src/shared/NomaNova.Ojeda.Models/NomaNova.Ojeda.Models.csproj

COPY src/shared/NomaNova.Ojeda.Client/NomaNova.Ojeda.Client.csproj ./src/shared/NomaNova.Ojeda.Client/
RUN dotnet restore src/shared/NomaNova.Ojeda.Client/NomaNova.Ojeda.Client.csproj

COPY src/web/NomaNova.Ojeda.Web/NomaNova.Ojeda.Web.csproj ./src/web/NomaNova.Ojeda.Web/
RUN dotnet restore src/web/NomaNova.Ojeda.Web/NomaNova.Ojeda.Web.csproj

# Copy source
COPY . .

# Publish
RUN dotnet publish src/web/NomaNova.Ojeda.Web/NomaNova.Ojeda.Web.csproj -c Release -o /publish

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
