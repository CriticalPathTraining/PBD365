cls

# create certificate in local certificate store
$certName = "AppCert1"

$cert = New-SelfSignedCertificate -CertStoreLocation cert:\localmachine\my `
                                  -Subject $certName `
                                  -FriendlyName $certName `
                                  -NotAfter (Get-Date).AddYears(2)

# get certificate thumbprint                                  
$thumbprint = $cert.Thumbprint

# get certificate hash
$certHash = $cert.GetCertHash()

# get certifcate path
$certPath = 'cert:\localMachine\my\' + $thumbprint
Write-Host "Certificate created at $certPath"

# export public key in cer file
$cer = Export-Certificate -Cert $certPath -FilePath "$PSScriptRoot\Cert\$certName.cer"

# export private key in pfx file
$password = "pass@word1"
$securePassword = ConvertTo-SecureString -String $password -Force -AsPlainText
$pfx = Export-PfxCertificate -cert $certPath -FilePath "$PSScriptRoot\Cert\$certName.pfx" -Password $securePassword


Write-Host "Generate JSON for Certificate Registration"
$keyId = ([System.Guid]::NewGuid()).ToString()
$rawCertHash = $cert.GetCertHash();
$base64CertHash = [System.Convert]::ToBase64String($rawCertHash);
$rawCert = $cert.GetRawCertData()
$base64Cert  = [System.Convert]::ToBase64String($rawCert)

# create text file with certificate info
$json = New-Object System.Text.StringBuilder
$result = $json.AppendLine('{');
$result = $json.AppendLine('  "customKeyIdentifier": "' + $base64CertHash + '",');
$result = $json.AppendLine('  "keyId": "' + $keyId + '",');
$result = $json.AppendLine('  "type": "AsymmetricX509Cert",');
$result = $json.AppendLine('  "usage": "Verify",');
$result = $json.AppendLine('  "value": "' + $base64Cert + '"');
$result = $json.AppendLine('}');

$json.ToString() | Out-File -FilePath "$PSScriptRoot\Cert\$certName.json.txt" -Encoding ascii
&"NOTEPAD.EXE" "$PSScriptRoot\Cert\$certname.json.txt"

Write-Host "Removing Certificate from User's Local Store"
$result = Remove-Item $cert.PSPath

Write-Host "All work has been completed"
