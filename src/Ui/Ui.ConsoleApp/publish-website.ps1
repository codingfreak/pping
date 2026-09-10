$ErrorActionPreference = 'Stop'
$publishDirPattern = "$PSScriptRoot/bin/Release/net10.0/%PLATFORM%/publish/%FILE%"
$targetDir = "~/Desktop/publish"
if (Test-Path $targetDir) {
    $null = Remove-Item -Force -Recurse $targetDir
}
$null = mkdir $targetDir
$platforms = @(
    [PSCustomObject]@{
        name   = "win-x64"
        file   = "pping.exe"
        hash   = ""
        target = ""
    },
    [PSCustomObject]@{
        name   = "linux-x64"
        file   = "pping"
        hash   = ""
        target = ""
    },
    [PSCustomObject]@{
        name   = "osx-x64"
        file   = "pping"
        hash   = ""
        target = ""
    },
    [PSCustomObject]@{
        name   = "osx-arm64"
        file   = "pping"
        hash   = ""
        target = ""
    }
)
foreach ($platform in $platforms) {
    Write-Host "Handling platform $($platform.name)..."
    $null = dotnet publish -r $platform.name -c Release /p:PublishSingleFile=true
    $binFile = $publishDirPattern.Replace("%PLATFORM%", $platform.name).Replace("%FILE%", $platform.file)
    $targetFile = "$targetDir/pping-$($platform.name)-latest.zip"
    Write-Host "Creating zip $($targetFile)..."
    Compress-Archive $binFile $targetFile
    Write-Host "Calculating hash..."
    $platform.target = $targetFile
    $platform.hash = (Get-FileHash $targetFile).Hash
}

$platforms | Select-Object @{N = 'Filename'; E = { Resolve-Path $_.target } }, @{N = 'Hash'; E = { $_.hash } } | Format-Table -AutoSize