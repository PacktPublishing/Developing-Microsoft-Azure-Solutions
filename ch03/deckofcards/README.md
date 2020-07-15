docker build . -f ./Dockerfile -t deckofcards:local

docker run -d -p80:80 -e "ASPNETCORE_URLS=http://+:80" deckofcards:local