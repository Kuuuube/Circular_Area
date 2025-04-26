Remove-Item -Recurse -Force bld
$options = @('--configuration', 'Release', '-p:DebugType=embedded')
dotnet publish ./Circular_Area $options --runtime win-x64 --framework net6.0 -o ./bld


Write-Host -NoNewLine 'Press any key to continue...';
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');
