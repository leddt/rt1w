set windows-powershell := true

build:
    dotnet build -c Release

render SCENE='1' OUTPUT='image.png': build
    cd RTConsole; dotnet run --no-build -c Release -- {{OUTPUT}} {{SCENE}}