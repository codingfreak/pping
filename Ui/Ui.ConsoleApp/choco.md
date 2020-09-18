You can perform all the steps in this file by simply `executing publish-choco-win64.ps1` in the project folder. Be 
aware that before doing so you must:

1. Increase the version number in the csproj.
2. Increase the version number in the nuspec.
3. Add releaseNotes in the nuspec.

# Generate self-contained EXE for Windows x64

1. Open the Ui.Console folder in a PowerShell command prompt on Windows.
2. Execute the dotnet publish command for self-contained EXE and Windows:

    ```
    dotnet publish -c Release
    ```

3. Calculate the new Hash:

    ```
    $hashExe = Get-FileHash .\bin\Release\netcoreapp3.1\publish\pping.exe | Select -ExpandProperty Hash 
    $hashDll = Get-FileHash .\bin\Release\netcoreapp3.1\publish\pping.dll | Select -ExpandProperty Hash 
    ```

4. Replace property in verification.txt:
   
    ```
    (Get-Content verification.txt) -replace '- pping.exe \(SHA256: (.*)', "- pping.exe (SHA256: $hashExe)" | Out-File verification.txt
    (Get-Content verification.txt) -replace '- pping.dll \(SHA256: (.*)', "- pping.exe (SHA256: $hashDll)" | Out-File verification.txt
    ```

5. Copy additional files to publish-folder:

    ```
    cp pping.nuspec .\bin\Release\netcoreapp3.1\win-x64\publish\pping.nuspec
    cp verification.txt .\bin\Release\netcoreapp3.1\win-x64\publish\verification.txt
    cp license.txt .\bin\Release\netcoreapp3.1\win-x64\publish\license.txt
    cp pping.runtimeconfig.json .\bin\Release\netcoreapp3.1\win-x64\publish\pping.runtimeconfig.json
    ```

6. Generate choco package:

    ```
    choco pack .\bin\Release\netcoreapp3.1\win-x64\publish\pping.nuspec --output-directory .\bin\Release\netcoreapp3.1\win-x64\publish\
    ```

7. Get version out of nuspec:

    ```
    $xmlFile = ".\bin\Release\netcoreapp3.1\win-x64\publish\pping.nuspec"
    [XML]$xml = Get-Content $xmlFile
    $version = $xml.package.metadata.version
    ```

8. Push choco:

    ```
    choco push .\bin\Release\netcoreapp3.1\win-x64\publish\pping.$version.nupkg
    ```