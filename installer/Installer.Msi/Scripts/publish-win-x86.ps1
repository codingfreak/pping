dotnet publish $PSScriptRoot\..\..\..\src\Ui\Ui.ConsoleApp -c Release -r win-x86 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true -o $PSScriptRoot\..\..\..\src\Ui\Ui.ConsoleApp\publish\win-x86