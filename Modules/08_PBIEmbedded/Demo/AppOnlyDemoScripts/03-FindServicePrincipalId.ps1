
# connect to Azure AD
$authResult = Connect-AzureAD

$adApplicationName = "App-only Auth Demo"

$servicePrincipal = Get-AzureADServicePrincipal -Filter "DisplayName eq '$adApplicationName'"

$servicePrincipal | Format-List DisplayName, AppId, ObjectId