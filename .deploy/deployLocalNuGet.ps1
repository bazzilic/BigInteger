$absPath = $PSScriptRoot
$slnAbsPath = Split-Path -parent $absPath
$deployAbsPath = "$(Split-Path -parent $slnAbsPath)\.NuGetFeed"

& "nuget" @("pack", "$slnAbsPath\BigInteger\BigInteger.csproj", "-Properties", "Configuration=Release")
& "nuget" @("add", "$(Get-ChildItem *.nupkg | Select-Object -First 1)", "-Source", "$deployAbsPath")

Get-ChildItem *.nupkg | ForEach-Object {
	Remove-Item $_ -Force
}