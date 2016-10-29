#!/usr/bin/env bash

echo 'Mac init script'
brew tap caskroom/cask
brew cask install docker
find /usr/local/Caskroom/docker
ls /usr/local/Caskroom/docker/1.12.0.9996/Applications