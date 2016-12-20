#!/usr/bin/env bash

echo 'Mac test script'

dotnet build -c Release -f netstandard1.4 src/tarantool.client/tarantool.client.csproj
dotnet build -c Release -f netcoreapp1.0 tests/tarantool.client.tests/tarantool.client.tests.csproj
dotnet test -c Release tests/tarantool.client.tests/tarantool.client.tests.csproj