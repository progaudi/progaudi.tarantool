$globalJson = Get-Content -Raw -Path global.json | ConvertFrom-Json
$version = $globalJson.Sdk.Version

Write-Host "##teamcity[setParameter name='DotnetCoreVersion' value='$version']"