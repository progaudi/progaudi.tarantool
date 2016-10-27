#!/usr/bin/env bash

echo 'Linux init script'

docker -v
docker-compose down
docker-compose up -d
pushd samples/docker-compose
docker-compose down
docker-compose up -d --build
popd