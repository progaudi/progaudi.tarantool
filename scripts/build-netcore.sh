#!/usr/bin/env bash

set -evx

pushd ${BASH_SOURCE%/*}/..

dotnet build -c Release progaudi.tarantool.sln

popd
