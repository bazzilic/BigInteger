Invoke-Expression "& `"$($PSScriptRoot)\nuget.exe`" pack `"..\BigInteger\BigInteger.csproj`" -Prop Configuration=Release"

Invoke-Expression "& `"$($PSScriptRoot)\nuget.exe`" add `"$(Get-ChildItem *.nupkg | Select-Object -First 1)`" -Source `"..\..\.NuGetFeed`""

Get-ChildItem *.nupkg | ForEach-Object {
	Remove-Item $_ -Force
}