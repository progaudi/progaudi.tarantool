#!/usr/bin/env bash

echo 'Mac init script'

brew tap caskroom/cask

mkdir -p /usr/local/lib
ln -s /usr/local/opt/openssl/lib/libcrypto.1.0.0.dylib /usr/local/lib/
ln -s /usr/local/opt/openssl/lib/libssl.1.0.0.dylib /usr/local/lib/
curl -o "/tmp/dotnet.pkg" -fL https://go.microsoft.com/fwlink/?linkid=838403
sudo installer -pkg /tmp/dotnet.pkg -target /
ln -s /usr/local/share/dotnet/dotnet /usr/local/bin/

brew install tarantool --HEAD

dotnet --info
dotnet restore

tarantool tarantool/tarantool.lua &