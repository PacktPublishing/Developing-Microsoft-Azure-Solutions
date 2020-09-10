#sh updatedns.sh CaddyfileTemplate testfile avalanche-sonarqube.eastus.azurecontainer.io
#sh config/caddyconfig/updatedns.sh config/caddyconfig/CaddyfileTemplate Caddyfile avalanche-sonarqube.eastus.azurecontainer.io
filename=$1
outputfile=$2
dnsname=$3

sed 's/dnsname/'$dnsname'/g' $filename > $outputfile





