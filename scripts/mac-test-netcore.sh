#!/usr/bin/env bash

set -e -v -x

pushd ${BASH_SOURCE%/*}/..

dotnet test -c Release --no-build $TEST_FILTER tests/progaudi.tarantool.tests/progaudi.tarantool.tests.csproj -- -parallel assemblies
