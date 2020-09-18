$folder = ".\bin\Release\netcoreapp3.1\publish"
# Execute the dotnet publish command for self-contained EXE and Windows
dotnet publish -c Release
# Calculate the new Hash:
$hashExe = Get-FileHash $folder\pping.exe | Select -ExpandProperty Hash
$hashDll = Get-FileHash $folder\pping.dll | Select -ExpandProperty Hash
# Replace hash in verification.txt
(Get-Content verification.txt) -replace '- pping.exe \(SHA256: (.*)', "- pping.exe (SHA256: $hashExe)" | Out-File verification.txt
(Get-Content verification.txt) -replace '- pping.dll \(SHA256: (.*)', "- pping.dll (SHA256: $hashDll)" | Out-File verification.txt
# copy files to publish-folder for packing
cp pping.nuspec $folder\pping.nuspec
cp verification.txt $folder\verification.txt
cp license.txt $folder\license.txt
cp pping.runtimeconfig.json $folder\pping.runtimeconfig.json
# pack choco package
choco pack $folder\pping.nuspec --output-directory $folder\
# read current version from nuspec
$xmlFile =  $folder + "pping.nuspec"
[XML]$xml = Get-Content $xmlFile
$version = $xml.package.metadata.version
# push package to choco
choco push $folder\pping.$version.nupkg
# commit and push changea to verification txt
git add .\verification.txt
git commit -m "Changed verfication hash due to build"
git push