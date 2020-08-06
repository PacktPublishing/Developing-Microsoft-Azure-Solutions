#
# Base image for deploying and running based on Ubuntu
#
# Support ASP.NET and does not include .NET SDK or NodeJs
# 
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
#
# Base image for building .NET based on Ubuntu
#
# 1. Uses .NET SDK image as the starting point 
# 2. Restore NuGet packages
# 3. Build the ASP.NET Core application
#
# Destination is /app/build which is copied to /app later on
#
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-bionic AS build
WORKDIR /src
COPY ["deckofcards.csproj", "./"]
RUN dotnet restore "./deckofcards.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "deckofcards.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-bionic AS debug
RUN curl --silent --location https://deb.nodesource.com/setup_12.x | bash -
RUN apt-get install --yes nodejs
ENTRYPOINT [ "sleep", "infinity" ]

#
# Base image for building React based on Node/Ubuntu
#
# Destination is /app/ClientApp/build which is copied to /clientapp later
#
# NOTE: npm run build puts the output in the build directory
#
FROM node:12.18-buster-slim AS clientbuild
WORKDIR /src
COPY ./ClientApp /app/ClientApp
WORKDIR "/app/ClientApp"
RUN npm install
RUN npm run build

#
# Copy clientbuild:/app/ClientApp to /app/ClientApp
#
# Copy build:/app to /app
#
FROM base as final
WORKDIR /app/ClientApp
COPY --from=clientbuild /app/ClientApp .
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "deckofcards.dll"]

