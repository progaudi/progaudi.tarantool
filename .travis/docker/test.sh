#!/usr/bin/env bash

echo 'Docker test script'

docker-compose -f docker-compose.yml -f docker-compose.tests.yml up --build tarantool-client 