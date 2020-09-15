# Execute the dotnet publish command for self-contained EXE and Windows
dotnet publish -c Release
# Calculate the new Hash:
$hashExe = Get-FileHash .\bin\Release\netcoreapp3.1\publish\pping.exe | Select -ExpandProperty Hash
$hashDll = Get-FileHash .\bin\Release\netcoreapp3.1\publish\pping.dll | Select -ExpandProperty Hash
# Replace hash in verification.txt
(Get-Content verification.txt) -replace '- pping.exe \(SHA256: (.*)', "- pping.exe (SHA256: $hashExe)" | Out-File verification.txt
(Get-Content verification.txt) -replace '- pping.dll \(SHA256: (.*)', "- pping.dll (SHA256: $hashDll)" | Out-File verification.txt
# copy files to publish-folder for packing
cp pping.nuspec .\bin\Release\netcoreapp3.1\publish\pping.nuspec
cp verification.txt .\bin\Release\netcoreapp3.1\publish\verification.txt
cp license.txt .\bin\Release\netcoreapp3.1\publish\license.txt
# pack choco package
choco pack .\bin\Release\netcoreapp3.1\publish\pping.nuspec --output-directory .\bin\Release\netcoreapp3.1\publish\
# read current version from nuspec
$xmlFile = ".\bin\Release\netcoreapp3.1\publish\pping.nuspec"
[XML]$xml = Get-Content $xmlFile
$version = $xml.package.metadata.version
# push package to choco
choco push .\bin\Release\netcoreapp3.1\publish\pping.$version.nupkg
# commit and push changea to verification txt
git add .\verification.txt
git commit -m "Changed verfication hash due to build"
git push