# tarantool-csharp

Dotnet client for Tarantool NoSql database.

# Key features
 - Full [IProto](https://tarantool.org/doc/dev_guide/box-protocol.html) protocol coverage.
 - Async API.
 - Multiplexing.

# Installation

Simpliest way to start using Tarantool-csharp in your project is to install it from [Nuget](https://www.nuget.org/packages/Tarantool.CSharp/).

# Demo

[We have a small demo](https://github.com/progaudi/tarantool-csharp/blob/master/samples/docker-compose/). It illustrates usage of library in aspnet core with docker-compose. Docker 1.12 is preferred.

# Usage

You can find basic usage scenarios in [index](https://github.com/progaudi/tarantool-csharp/blob/master/tests/tarantool.client.tests/Index/Smoke.cs) and [space](https://github.com/progaudi/tarantool-csharp/blob/master/tests/tarantool.client.tests/Space/Smoke.cs) smoke tests.

# Build statuses for master branch

Windows build status:

[![Windows build status](https://server-ci.evote.work/app/rest/builds/buildType:(id:Progaudi_Tarantool)/statusIcon)](https://server-ci.evote.work/viewType.html?buildTypeId=Progaudi_Tarantool)

Linux and OSX build status:

[![Linux/OSX build Status](https://travis-ci.org/progaudi/tarantool-csharp.svg?branch=master)](https://travis-ci.org/progaudi/tarantool-csharp)