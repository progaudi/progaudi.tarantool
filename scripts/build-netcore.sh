#!/usr/bin/env bash

set -ev

pushd ${BASH_SOURCE%/*}

cd ..

dotnet restore
dotnet build -c Release -f netstandard1.4 src/progaudi.tarantool/progaudi.tarantool.csproj
dotnet build -c Release -f netstandard2.0 src/progaudi.tarantool/progaudi.tarantool.csproj
dotnet build -c Release -f netcoreapp1.0 tests/progaudi.tarantool.tests/progaudi.tarantool.tests.csproj
dotnet build -c Release -f netcoreapp1.1 tests/progaudi.tarantool.tests/progaudi.tarantool.tests.csproj
dotnet build -c Release -f netcoreapp2.0 tests/progaudi.tarantool.tests/progaudi.tarantool.tests.csproj

popd
