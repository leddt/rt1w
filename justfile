set windows-powershell := true

build CONFIG='Release':
    dotnet build -c {{CONFIG}}

render SCENE='1' OUTPUT='image.png' CONFIG='Release': (build CONFIG)
    cd RTConsole; dotnet run --no-build -- {{OUTPUT}} {{SCENE}}