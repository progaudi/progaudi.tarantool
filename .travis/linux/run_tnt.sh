#!/usr/bin/env bash

echo 'Linux start tarantool script'

docker -v

if [[ "$BUILD_DOCKER" == false ]]; then docker-compose down ; fi
if [[ "$BUILD_DOCKER" == false ]]; then docker-compose up -d ; fi

pushd samples/docker-compose
docker-compose down
docker-compose up -d --build
popd 