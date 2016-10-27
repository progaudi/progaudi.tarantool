#!/usr/bin/env bash

echo 'Linux test script'

dotnet test tests/tarantool.client.tests
curl -o /dev/null --fail http://localhost:5000