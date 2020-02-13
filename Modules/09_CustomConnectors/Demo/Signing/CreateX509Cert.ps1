cls

# create certificate in local certificate store
$certName = "MyDataConnectorCert"

$cert = New-SelfSignedCertificate -CertStoreLocation cert:\localmachine\my `
                                  -Subject $certName `
                                  -FriendlyName $certName `
                                  -NotAfter (Get-Date).AddYears(2)

# get certificate thumbprint                                  
$thumbprint = $cert.Thumbprint

# get certifcate path
$certPath = 'cert:\localMachine\my\' + $thumbprint
Write-Host "Certificate created at $certPath"

# export private key in pfx file
$password = "pass@word1"
$securePassword = ConvertTo-SecureString -String $password -Force -AsPlainText
$pfx = Export-PfxCertificate -cert $certPath -FilePath "$PSScriptRoot\Cert\$certName.pfx" -Password $securePassword


# create text file with certificate info
$output = New-Object System.Text.StringBuilder
$result = $output.AppendLine('Thumbprint:');
$result = $output.AppendLine($thumbprint);

$output.ToString() | Out-File -FilePath "$PSScriptRoot\Cert\$certName.txt" -Encoding ascii
&"NOTEPAD.EXE" "$PSScriptRoot\Cert\$certname.txt"

Write-Host "Removing Certificate from User's Local Store"
$result = Remove-Item $cert.PSPath

Write-Host "All work has been completed"
