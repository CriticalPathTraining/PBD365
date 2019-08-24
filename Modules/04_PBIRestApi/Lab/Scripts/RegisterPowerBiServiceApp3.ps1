# log into Azure AD
$userName = "user1@MY_TENANT.onMicrosoft.com"
$password = ""

$securePassword = ConvertTo-SecureString –String $password –AsPlainText -Force
$credential = New-Object –TypeName System.Management.Automation.PSCredential `
                         –ArgumentList $userName, $securePassword

$authResult = Connect-AzureAD -Credential $credential

# display name for new confidential client app
$appDisplayName = "Power BI Service App 3"

# get user account ID for logged in user
$user = Get-AzureADUser -ObjectId $authResult.Account.Id
$userDisplayName = $user.DisplayName

# get tenant name of logged in user
$tenantId = $authResult.TenantId.ToString()
$tenantName = $authResult.TenantDomain

# create new password credential for client secret
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
                        -Homepage $replyUrl `
                        -PasswordCredentials $passwordCredential
                        

# create applicaiton's service principal 
$appId = $aadApplication.AppId
$appObjectId = $aadApplication.ObjectId
$serviceServicePrincipal = New-AzureADServicePrincipal -AppId $appId
$serviceServicePrincipalObjectId = $serviceServicePrincipal.ObjectId

# assign current user as owner
Add-AzureADApplicationOwner -ObjectId $aadApplication.ObjectId -RefObjectId $user.ObjectId

$adSecurityGroupName = "Power BI Apps"
$adSecurityGroup = Get-AzureADGroup -Filter "DisplayName eq '$adSecurityGroupName'"

Add-AzureADGroupMember -ObjectId $($adSecurityGroup.ObjectId) -RefObjectId $($serviceServicePrincipalObjectId)
Write-Host "Members of Azure AD group named $adSecurityGroupName"
Get-AzureADGroupMember -ObjectId $($adSecurityGroup.ObjectId) | Format-Table ObjectType, ObjectId, DisplayName

$outputFile = "$PSScriptRoot\PowerBiServiceApp3.txt"
Out-File -FilePath $outputFile -InputObject "--- Confidential Client App Info for PowerBiServiceApp3 ---"
Out-File -FilePath $outputFile -Append -InputObject "ClientId: $appId"
Out-File -FilePath $outputFile -Append -InputObject "ClientSecret: $appSecret"
Out-File -FilePath $outputFile -Append -InputObject "Service Principal Object ID: $serviceServicePrincipalObjectId"
Out-File -FilePath $outputFile -Append -InputObject "TenantName: $tenantName"
Out-File -FilePath $outputFile -Append -InputObject "TenantId: $tenantId"

Notepad $outputFile