dotnet publish -r osx-x64 -c Release /p:PublishSingleFile=true
openssl dgst -sha256 bin/Release/netcoreapp3.1/osx-x64/publish/pping