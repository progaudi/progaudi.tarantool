#!/usr/bin/env bash

set -e -v -x

pushd ${BASH_SOURCE%/*}/..

TARANTOOL_1_7_REPLICATION_SOURCE=localhost:3301 TARANTOOL_1_8_REPLICATION_SOURCE=localhost:3301 dotnet test -c Release --no-build $TEST_FILTER tests/progaudi.tarantool.tests/progaudi.tarantool.tests.csproj -- -parallel assemblies
