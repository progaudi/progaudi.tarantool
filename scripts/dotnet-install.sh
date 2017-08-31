#!/usr/bin/env bash

set -ev

curl -o ./dotnet-install.sh -fL https://raw.githubusercontent.com/dotnet/cli/master/scripts/obtain/dotnet-install.sh
chmod +x ./dotnet-install.sh
./dotnet-install.sh --version 1.0.4
./dotnet-install.sh --version 2.0.0
