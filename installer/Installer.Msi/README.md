# Installer guide

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