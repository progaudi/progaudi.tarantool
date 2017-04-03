#!/usr/bin/env bash

set -ev

pushd ${BASH_SOURCE%/*}

cd ..

dotnet test -c Release -f netcoreapp1.0 --no-build tests/progaudi.tarantool.tests/progaudi.tarantool.tests.csproj -- -parallel assemblies
dotnet test -c Release -f netcoreapp1.1 --no-build tests/progaudi.tarantool.tests/progaudi.tarantool.tests.csproj -- -parallel assemblies

popd
