# dotnet-libyear

A simple measure of dependency freshness

## Installation

1. get your project set up

    1. `dotnet new -i libyear::*`

    1. `dotnet new libyear`
    
    or add the following to your existing csproj:
```
<ItemGroup>
  <DotNetCliToolReference Include="libyear" Version="*" />
</ItemGroup>
```

2. `dotnet restore`

## Usage

`dotnet libyear [args] [{csproj}|{dir}]`

- Zero or more directories or csproj files may be passed
- If no arguments are passed, the current directory is searched
- If no csproj is found in a directory, subdirectories are searched

### Arguments:

`-h`, `--help`: display this help message

`-q`, `--quiet`: only show outdated packages

`-u`, `--update`: update project files after displaying packages
