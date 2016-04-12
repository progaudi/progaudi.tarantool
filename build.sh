#!/usr/bin/env bash

brew update
brew install curl
curl -sSL https://raw.githubusercontent.com/aspnet/Home/dev/dnvminstall.sh | sh && source ~/.dnx/dnvm/dnvm.sh && dnvm upgrade
dnvm update-self
dnvm upgrade
dnvm install 1.0.0-rc1-update1
dnvm list
dnvm use 1.0.0-rc1-update1
dnu restore
dnx -p tests/msgpack.tests test