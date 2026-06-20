You can perform all the steps in this file by simply `executing publish-choco-win64.ps1` in the project folder. Be 
aware that before doing so you must:

1. Increase the version number in the csproj.
2. Increase the version number in the nuspec.
3. Add releaseNotes in the nuspec.

# Generate self-contained EXE for Windows x64

1. Open the Ui.Console folder in a PowerShell command prompt on Windows.
2. Define the ENV variable `CHOCO_API_KEY` with `$env:CHOCO_API_KEY="THEKEY"`.
2. Execute the script `.\publish-choco.ps1`
