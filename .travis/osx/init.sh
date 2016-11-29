#!/usr/bin/env bash

echo 'Mac init script'
brew tap caskroom/cask
sudo brew cask install docker
which docker