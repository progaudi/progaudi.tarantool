#!/usr/bin/env bash

set -ev

curl -o ./dotnet-install.sh -fL https://raw.githubusercontent.com/dotnet/cli/master/scripts/obtain/dotnet-install.sh
chmod +x ./dotnet-install.sh
version=$(jq -r .sdk.version global.json)
./dotnet-install.sh --version $version
