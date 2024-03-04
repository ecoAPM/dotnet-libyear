# dotnet-libyear

A simple measure of dependency freshness

[![NuGet version](https://img.shields.io/nuget/v/LibYear?logo=nuget&label=Install)](https://nuget.org/packages/LibYear)
[![CI](https://github.com/ecoAPM/dotnet-libyear/actions/workflows/CI.yml/badge.svg)](https://github.com/ecoAPM/dotnet-libyear/actions/workflows/CI.yml)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=ecoAPM_dotnet-libyear&metric=coverage)](https://sonarcloud.io/dashboard?id=ecoAPM_dotnet-libyear)

[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=ecoAPM_dotnet-libyear&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=ecoAPM_dotnet-libyear)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=ecoAPM_dotnet-libyear&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=ecoAPM_dotnet-libyear)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=ecoAPM_dotnet-libyear&metric=security_rating)](https://sonarcloud.io/dashboard?id=ecoAPM_dotnet-libyear)

## Requirements

- .NET 8 SDK

## Installation

`dotnet tool install -g libyear`

## Usage

`dotnet libyear [args] [{csproj}|{dir}]`

- Zero or more directories or `csproj` files may be passed
- If no arguments are passed, the current directory is searched
- If no `csproj` is found in a directory, subdirectories are searched

### Arguments:

`-h`, `--help`: display this help message

`-q`, `--quiet`: only show outdated packages

`-u`, `--update`: update project files after displaying packages

`-r`, `--recursive`: search recursively for all compatible files, even if one is found in a directory passed as an argument

#### Limits:

`-l`, `--limit`: exits with error code if total libyears behind is greater than this value

`-p`, `--limit-project`: exits with error code if any project is more libyears behind than this value

`-a`, `--limit-any`: exits with error code if any dependency is more libyears behind than this value