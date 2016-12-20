#!/usr/bin/env bash

echo 'Linux test script'

dotnet build -c Release -f netstandard1.4 src/tarantool.client/tarantool.client.csproj
dotnet build -c Release -f netcoreapp1.0 tests/tarantool.client.tests/tarantool.client.tests.csproj
dotnet test -c Release tests/tarantool.client.tests/tarantool.client.tests.csproj

curl -o /dev/null --fail http://localhost:5000