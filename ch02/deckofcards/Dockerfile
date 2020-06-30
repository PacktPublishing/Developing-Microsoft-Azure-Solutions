FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["deckofcards/deckofcards.csproj", "deckofcards/"]
RUN dotnet restore "deckofcards/deckofcards.csproj"
COPY . .
WORKDIR "/src/deckofcards"
RUN dotnet build "deckofcards.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "deckofcards.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "deckofcards.dll"]
