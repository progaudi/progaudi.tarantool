#!/usr/bin/env bash

echo 'Linux init script'

dotnet --info
dotnet restore

docker -v

docker-compose down 
docker-compose up -d

pushd samples/docker-compose
docker-compose down
docker-compose up -d --build
popd 