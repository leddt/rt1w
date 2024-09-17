set windows-powershell := true

build:
    dotnet build -c Release

render SCENE='1': build
    cd RTConsole; dotnet run --no-build -c Release -- image.png {{SCENE}}