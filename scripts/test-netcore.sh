#!/usr/bin/env bash

set -ev

pushd ${BASH_SOURCE%/*}

cd ..

dotnet test -c Release --no-build tests/progaudi.tarantool.tests/progaudi.tarantool.tests.csproj -- -parallel assemblies

popd
