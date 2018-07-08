#!/usr/bin/env bash

set -evx

pushd ${BASH_SOURCE%/*}/..

docker-compose down && docker-compose up -d

./Scripts/build-netcore.sh

pushd samples/insert-performance/bin/Release/netcoreapp2.0/

dotnet insert-performance.dll

popd
popd