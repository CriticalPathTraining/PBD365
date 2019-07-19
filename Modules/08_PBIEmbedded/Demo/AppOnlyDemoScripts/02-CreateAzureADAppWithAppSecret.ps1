cls

$appDisplayName = "App-only Auth Demo"
$replyUrl = "https://localhost:44300"

# connect to Azure AD
$authResult = Connect-AzureAD

$tenantId = $authResult.TenantId.ToString()
$tenantDomain = $authResult.TenantDomain
$tenantDisplayName = (Get-AzureADTenantDetail).DisplayName

$userAccountId = $authResult.Account.Id
$user = Get-AzureADUser -ObjectId $userAccountId
$userDisplayName = $user.DisplayName

# create app secret
$newGuid = New-Guid
$appSecret = ([System.Convert]::ToBase64String([System.Text.Encoding]::UTF8.GetBytes(($newGuid))))+"="
$startDate = Get-Date	
$passwordCredential = New-Object -TypeName Microsoft.Open.AzureAD.Model.PasswordCredential
$passwordCredential.StartDate = $startDate
$passwordCredential.EndDate = $startDate.AddYears(1)
$passwordCredential.KeyId = $newGuid
$passwordCredential.Value = $appSecret 

Write-Host "Registering new app $appDisplayName in $tenantDomain"

# create Azure AD Application
$aadApplication = New-AzureADApplication `
                        -DisplayName $appDisplayName  `
                        -PublicClient $false `
                        -AvailableToOtherTenants $false `
                        -ReplyUrls @($replyUrl) `
                        -Homepage $replyUrl `
                        -PasswordCredentials $passwordCredential

# create applicaiton's service principal 
$appId = $aadApplication.AppId
$appObjectId = $aadApplication.ObjectId
$serviceServicePrincipal = New-AzureADServicePrincipal -AppId $appId
$serviceServicePrincipalObjectId = $serviceServicePrincipal.ObjectId

# assign current user as owner
Add-AzureADApplicationOwner -ObjectId $aadApplication.ObjectId -RefObjectId $user.ObjectId

$outputFile = "$PSScriptRoot\$appDisplayName.txt"
Out-File -FilePath $outputFile -InputObject "--- Info for $appDisplayName ---"
Out-File -FilePath $outputFile -Append -InputObject "TenantId: $tenantId"
Out-File -FilePath $outputFile -Append -InputObject "AppId: $appId"
Out-File -FilePath $outputFile -Append -InputObject "AppSecret: $appSecret"
Out-File -FilePath $outputFile -Append -InputObject "ReplyUrl: $replyUrl"
Out-File -FilePath $outputFile -Append -InputObject "Service Principal Object ID: $serviceServicePrincipalObjectId"

Notepad $outputFile

