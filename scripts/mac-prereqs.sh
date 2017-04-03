#!/usr/bin/env bash

set -ev

brew update
brew install openssl jq
brew install tarantool --HEAD

mkdir -p /usr/local/lib

ln -sf /usr/local/opt/openssl/lib/libcrypto.1.0.0.dylib /usr/local/lib/
ln -sf /usr/local/opt/openssl/lib/libssl.1.0.0.dylib /usr/local/lib/