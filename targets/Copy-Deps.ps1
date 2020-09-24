#! /bin/pwsh

param(
    [string[]] $Dependencies,
    [string] $RootDir = "../",
    [string] $Configuration = "Debug",
    [string] $Framework = "netcoreapp3.1"
)

function Get-LatestVersion([string] $dep) {
    return $(Get-ChildItem ~/.nuget/packages/$dep | Sort-Object -Property Name -Descending)[0]
}

function Get-DepContents([string]$dep) {
    $v = Get-LatestVersion($dep)
    
    foreach ($j in @("netcoreapp3.1", "netstandard2.1", "netcoreapp3.0", "netstandard2.0", "netcoreapp2.1", "net48", "net47", "netcoreapp1.1", "net45")) {
        if ([System.IO.Directory]::Exists("$v/lib/$j/")) {
            return "$v/lib/$j/*"
        }
    }
}

foreach ($d in $Dependencies) {    
    Invoke-Expression "Copy-Item -Path $(Get-DepContents($d)) -Destination $($RootDir)Build/bin/ProteusWorkstation/$Configuration/$Framework"
}
