
#sh uploadfile.sh 'strsonarqube' 'sonarqubeextensions' 'sonar-auth-aad-plugin-1.2.0.jar' 'sonarqubeextensions/plugins/sonar-auth-aad-plugin-1.2.0.jar' 'RG-DEVOPS' 'https://github.com/hkamel/sonar-auth-aad/releases/download/1.2.0/sonar-auth-aad-plugin-1.2.0.jar'

account=$1          #strsonarqube
sharename=$2        #sonarqubeextensions
sourcefile=$3       #sonar-auth-aad-plugin-1.2.0.jar
pathfile=$4         #sonarqubeextensions/plugins/sonar-auth-aad-plugin-1.2.0.jar
resourcegroup=$5    #RG-DEVOPS

#az storage account keys list -n strsonarqube -g RG-DEVOPS --query "[?keyName=='key1'].{value:value}" -o tsv
accountkey=$(az storage account keys list -n $account -g $resourcegroup --query "[?keyName=='key1'].{value:value}" -o tsv)
echo $accountkey

# curl -O  "https://github.com/hkamel/sonar-auth-aad/releases/download/1.2.0/sonar-auth-aad-plugin-1.2.0.jar"

command="storage file upload \
    --account-name $account \
    --account-key $accountkey \
	--share-name $sharename \
	--source $sourcefile"

#if [ -z "$pathfile" ]
if [ "$pathfile" == "na" ];
then
    #sh uploadfile.sh strsonarqube sonarqubeconfig config/sonarqubeconfig/sonar.properties "na" RG-DEVOPS
    echo "\$var is empty"
else
    #sh uploadfile.sh 'strsonarqube' 'sonarqubeextensions' 'plugins/sonar-auth-aad-plugin-1.2.0.jar' 'plugins/sonar-auth-aad-plugin-1.2.0.jar' 'RG-DEVOPS'
    command="$command --path $pathfile"
fi

command=$(az $command)
    #--path $pathfile