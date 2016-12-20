#!/usr/bin/env bash

echo 'Docker init script'

docker-compose -f docker-compose.yml -f docker-compose.tests.yml up -d tarantool