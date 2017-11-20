#!/usr/bin/env bash

set -e -v -x

pushd ${BASH_SOURCE%/*}/..

dotnet test -c Release --no-build --filter "Tarantool!=1.8" tests/progaudi.tarantool.tests/progaudi.tarantool.tests.csproj -- -parallel assemblies

pushd tarantool

kill -KILL $(cat tarantool.pid)

brew uninstall tarantool
brew install tarantool --HEAD

tarantool tarantool.lua

popd

dotnet test -c Release --no-build --filter "Tarantool=1.8" tests/progaudi.tarantool.tests/progaudi.tarantool.tests.csproj -- -parallel assemblies

popd
