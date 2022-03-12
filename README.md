# progaudi.tarantool

Dotnet client for Tarantool NoSql database made by Analtoy Popov 🤠🤠🤠

# Key features
 - Full [IProto](https://tarantool.org/doc/dev_guide/box-protocol.html) protocol coverage.
 - Async API.
 - Multiplexing.

# Installation

Simpliest way to start using ```progaudi.tarantool``` in your project is to install it from [Nuget](https://www.nuget.org/packages/progaudi.tarantool/).

# Demo

[We have a small demo](https://github.com/progaudi/progaudi.tarantool/blob/master/samples/docker-compose/). It illustrates usage of library in aspnet core with docker-compose. Docker 1.12+ is preferred.

# Usage

You can find basic usage scenarios in [index](https://github.com/progaudi/progaudi.tarantool/blob/master/tests/progaudi.tarantool.tests/Index/Smoke.cs) and [space](https://github.com/progaudi/progaudi.tarantool/blob/master/tests/progaudi.tarantool.tests/Space/Smoke.cs) smoke tests.

# Build statuses for master branch

Windows build status:

[![Windows build status](https://server-ci.evote.work/app/rest/builds/buildType:(id:Progaudi_Tarantool)/statusIcon)](https://server-ci.evote.work/viewType.html?buildTypeId=Progaudi_Tarantool)

Linux and OSX build status:

[![Linux/OSX build Status](https://travis-ci.org/progaudi/progaudi.tarantool.svg?branch=master)](https://travis-ci.org/progaudi/progaudi.tarantool)

# Limitations

We were trying to make API similar with tarantool lua API. So this connector is straightforward implementing of [IProto protocol](https://tarantool.org/doc/dev_guide/internals_index.html). Some methods are not implemented yet because there are no direct analogs in IProto. Implementing some methods (like Pairs) does not make any sense, because it return a lua-iterator.

When API will be finalized all methods would be implemented or removed from public API.

* Index methods:
    1. Pairs
    2. Count
    3. Alter
    4. Drop
    5. Rename
    6. BSize
    7. Alter
* Schema methods
    1. CreateSpace
* Space methods
    1. CreateIndex
    2. Drop
    3. Rename
    4. Count
    5. Lengh
    6. Increment
    7. Decrement
    8. AutoIncrement
    9. Pairs
