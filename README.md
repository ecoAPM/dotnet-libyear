# dotnet-libyear

A simple measure of dependency freshness

[![Actions Status](https://github.com/stevedesmond-ca/dotnet-libyear/workflows/CI/badge.svg)](https://github.com/stevedesmond-ca/dotnet-libyear/actions)

## Installation

`dotnet tool install -g libyear`

## Usage

`dotnet libyear [args] [{csproj}|{dir}]`

- Zero or more directories or csproj files may be passed
- If no arguments are passed, the current directory is searched
- If no csproj is found in a directory, subdirectories are searched

### Arguments:

`-h`, `--help`: display this help message

`-q`, `--quiet`: only show outdated packages

`-u`, `--update`: update project files after displaying packages
