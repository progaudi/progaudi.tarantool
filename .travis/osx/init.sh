#!/usr/bin/env bash

echo 'Mac init script'
brew tap caskroom/cask
brew cask install docker
docker -v || echo 'docker failed'