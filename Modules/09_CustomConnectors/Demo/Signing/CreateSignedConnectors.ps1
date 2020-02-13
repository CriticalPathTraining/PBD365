cls

$input = "$PSScriptRoot\Connectors\TripPin.mez"

$output = "$PSScriptRoot\Connectors\SignedTripPin.pqx"

$certPath = "$PSScriptRoot\Cert\MyDataConnectorCert.pfx"
$certPassword = "pass@word1"

.\MakePQX.exe pack -mz $input -t $output

.\MakePQX sign $output --certificate $certPath --password $certPassword.\MakePQX.exe verify $output