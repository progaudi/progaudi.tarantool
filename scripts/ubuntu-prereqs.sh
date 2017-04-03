#!/usr/bin/env bash

set -ev

apt-get -qq update
apt-get install -y \
    jq \
    libunwind8 \
    libunwind8-dev \
    gettext \
    libicu-dev \
    liblttng-ust-dev \
    libcurl4-openssl-dev \
    libssl-dev \
    uuid-dev \
    unzip