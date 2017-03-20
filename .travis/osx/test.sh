#!/usr/bin/env bash

echo 'Mac test script'

dotnet build -c Release -f netstandard1.4 src/progaudi.tarantool/progaudi.tarantool.csproj
dotnet build -c Release -f netcoreapp1.0 tests/progaudi.tarantool.tests/progaudi.tarantool.tests.csproj
dotnet test -c Release tests/progaudi.tarantool.tests/progaudi.tarantool.tests.csproj