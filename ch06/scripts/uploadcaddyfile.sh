
#sh uploadcaddyfile.sh 'strsonarqube' 'caddyconfig' 'Caddyfile1' 'caddyconfig/ok' 'RG-DEVOPS'

account=$1          #strsonarqube
sharename=$2        #sonarqubeextensions
sourcefile=$3       #sonar-auth-aad-plugin-1.2.0.jar
pathfile=$4         #sonarqubeextensions/plugins/sonar-auth-aad-plugin-1.2.0.jar
resourcegroup=$5    #RG-DEVOPS

#az storage account keys list -n strsonarqube -g RG-DEVOPS --query "[?keyName=='key1'].{value:value}" -o tsv
accountkey=$(az storage account keys list -n $account -g $resourcegroup --query "[?keyName=='key1'].{value:value}" -o tsv)
echo $accountkey

az storage file upload \
    --account-name $account \
    --account-key $accountkey \
	--share-name $sharename \
	--source $sourcefile
    #--path $pathfile