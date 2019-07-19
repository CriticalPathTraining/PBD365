# log into Azure AD
$userName = "user1@tenant1.onMicrosoft.com"
$password = ""

$securePassword = ConvertTo-SecureString –String $password –AsPlainText -Force
$credential = New-Object –TypeName System.Management.Automation.PSCredential `
                         –ArgumentList $userName, $securePassword

$authResult = Connect-AzureAD -Credential $credential


# display name for new public client app
$appDisplayName = "Power BI Service App 2"

# get user account ID for logged in user
$user = Get-AzureADUser -ObjectId $authResult.Account.Id

# get tenant name of logged in user
$tenantName = $authResult.TenantDomain

# create Azure AD Application
$replyUrl = "https://localhost/app1234"
$aadApplication = New-AzureADApplication `
                        -DisplayName $appDisplayName `
                        -PublicClient $true `
                        -AvailableToOtherTenants $false `
                        -ReplyUrls @($replyUrl)

# create service principal for application
$appId = $aadApplication.AppId
$serviceServicePrincipal = New-AzureADServicePrincipal -AppId $appId

# assign current user as application owner
Add-AzureADApplicationOwner -ObjectId $aadApplication.ObjectId -RefObjectId $user.ObjectId

# configure delegated permisssions for the Power BI Service API
$requiredAccess = New-Object -TypeName "Microsoft.Open.AzureAD.Model.RequiredResourceAccess"
$requiredAccess.ResourceAppId = "00000009-0000-0000-c000-000000000000"

# create first delegated permission - Report.Read.All
$permission1 = New-Object -TypeName "Microsoft.Open.AzureAD.Model.ResourceAccess" `
                          -ArgumentList "4ae1bf56-f562-4747-b7bc-2fa0874ed46f","Scope"

# create second delegated permission - Dashboards.Read.All
$permission2 = New-Object -TypeName "Microsoft.Open.AzureAD.Model.ResourceAccess" `
                          -ArgumentList "2448370f-f988-42cd-909c-6528efd67c1a","Scope"

# add permissions to ResourceAccess list
$requiredAccess.ResourceAccess = $permission1, $permission2

# add permissions by updating application with RequiredResourceAccess object
Set-AzureADApplication -ObjectId $aadApplication.ObjectId -RequiredResourceAccess $requiredAccess

$outputFile = "$PSScriptRoot\PowerBiServiceApp2.txt"
Out-File -FilePath $outputFile -InputObject "--- Public Client App Info for PowerBiServiceApp2 ---"
Out-File -FilePath $outputFile -Append -InputObject "ClientId: $appId"
Out-File -FilePath $outputFile -Append -InputObject "ReplyUrl: $replyUrl"
Out-File -FilePath $outputFile -Append -InputObject "TenantName: $tenantName"

Notepad $outputFile