#!/usr/bin/env bash

echo 'Docker teardown script'

docker-compose -f docker-compose.yml -f docker-compose.tests.yml down 