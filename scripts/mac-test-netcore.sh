#!/usr/bin/env bash

set -e -v -x

pushd ${BASH_SOURCE%/*}/..

dotnet test -c Release --no-build --filter "Tarantool!=1.8" tests/progaudi.tarantool.tests/progaudi.tarantool.tests.csproj -- -parallel assemblies
killall -9 tarantool
brew uninstall tarantool
brew install tarantool --HEAD

pushd tarantool
tarantool tarantool.lua
dotnet test -c Release --no-build --filter "Tarantool==1.8" tests/progaudi.tarantool.tests/progaudi.tarantool.tests.csproj -- -parallel assemblies

popd
popd

