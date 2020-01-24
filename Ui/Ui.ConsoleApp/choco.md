Follow these instruction in order to update the chocolaty package:

    dotnet build -c Release
    cd bin\Release\netcoreapp3.1
    choco pack
    choco push pping.2.0.0.nupkg