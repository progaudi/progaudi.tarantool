#!/usr/bin/env bash

set -e -x

docker-compose build net

docker-compose down && docker-compose up -d tarantool_1_8
docker-compose up go

# docker-compose down && docker-compose up -d tarantool_1_8
# docker-compose up net20-baseline

# docker-compose down && docker-compose up -d tarantool_1_8
# docker-compose up net-baseline

docker-compose down && docker-compose up -d tarantool_1_8
docker-compose up net
