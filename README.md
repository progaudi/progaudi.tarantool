# progaudi.tarantool

Dotnet client for Tarantool NoSql database.

# Key features
 - Full [IProto](https://tarantool.org/doc/dev_guide/box-protocol.html) protocol coverage.
 - Async API.
 - Multiplexing.

# Installation

Simpliest way to start using ```progaudi.tarantool``` in your project is to install it from [Nuget](https://www.nuget.org/packages/progaudi.tarantool/).

# Demo

[We have a small demo](https://github.com/progaudi/progaudi.tarantool/blob/master/samples/docker-compose/). It illustrates usage of library in aspnet core with docker-compose. Docker 1.12 is preferred.

# Usage

You can find basic usage scenarios in [index](https://github.com/progaudi/progaudi.tarantool/blob/master/tests/progaudi.tarantool.tests/Index/Smoke.cs) and [space](https://github.com/progaudi/progaudi.tarantool/blob/master/tests/progaudi.tarantool.tests/Space/Smoke.cs) smoke tests.

# Build statuses for master branch

Windows build status:

[![Windows build status](https://server-ci.evote.work/app/rest/builds/buildType:(id:Progaudi_Tarantool)/statusIcon)](https://server-ci.evote.work/viewType.html?buildTypeId=Progaudi_Tarantool)

Linux and OSX build status:

[![Linux/OSX build Status](https://travis-ci.org/progaudi/progaudi.tarantool.svg?branch=master)](https://travis-ci.org/progaudi/progaudi.tarantool)

# Limitations
We were trying to make API similar with tarantool API. But that connector is just implementing of [IProto protocol](https://tarantool.org/doc/dev_guide/internals_index.html). Methods, which can be implemented as 1 IProto request is implemented. But some other methods (for example Upsert methods) should make several requests. Another methods (for example DLL methods) can't be implemented in any other way except using CALL or EVAL requests. That is why some methods exists in API, but not implemented:
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
 
Also, type SCALAR are not supported - if you create index with field_type = 'scalar', add for example tuples with integer and string on that field, then you will not be able to make Select from that index.
