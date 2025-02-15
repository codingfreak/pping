# Installer guide

## Enabling WIX

- Install the [WiX Toolset 3.14](https://github.com/wixtoolset/wix3/releases/tag/wix3141rtm).
- Also add the [Visual Studio Extension](https://marketplace.visualstudio.com/items?itemName=WixToolset.WixToolsetVisualStudio2022Extension).

## License RTF

In order to update the license info change the content of `Assets\license.md`. Then ensure that `pandoc` is installed one of:

```shell
choco install pandoc
winget install --source winget --exact --id JohnMacFarlane.Pandoc
brew install pandoc
```

Then in the assets folder run

```shell
pandoc -f markdown -s 1.md -o file.rtf
```

And rebuild the installer.

## Building

Run `dotnet build