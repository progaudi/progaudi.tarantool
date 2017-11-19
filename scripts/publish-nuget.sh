#!/usr/bin/env bash

set -e -x

version=$(git describe --tags | sed s/-/./)
dotnet_path=/$(echo $1 | sed 's/\\/\//g' | sed 's/://')
./scripts/if_there_are_changes_in.sh 'src/progaudi.tarantool' --then $dotnet_path/dotnet pack --include-symbols -c Release -o ../../Publish src/progaudi.tarantool/progaudi.tarantool.csproj /property:Version=$version
./scripts/if_there_are_changes_in.sh 'src/progaudi.tarantool' --then $dotnet_path/dotnet nuget push Publish/progaudi.tarantool.$version.nupkg -k %NugetKey% -s https://api.nuget.org/v3/index.json
