# tarantool-dnx

[![Join the chat at https://gitter.im/aensidhe/tarantool-dnx](https://badges.gitter.im/aensidhe/tarantool-dnx.svg)](https://gitter.im/aensidhe/tarantool-dnx?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Dotnet client for Tarantool NoSql database.

# Key features
 - Full [IProto](https://tarantool.org/doc/dev_guide/box-protocol.html) protocol coverage.
 - Async API.
 - Multiplexing.

# Installation

Simpliest way to start using Tarantool-csharp in your project is to install it from [Nuget](https://www.nuget.org/packages/Tarantool.CSharp/).

# Usage

You can find basic usage scenarios in [index](https://github.com/aensidhe/tarantool-csharp/blob/master/tests/tarantool.client.tests/Index/Smoke.cs) and [space](https://github.com/aensidhe/tarantool-csharp/blob/master/tests/tarantool.client.tests/Space/Smoke.cs) smoke tests.

# Build statuses for master branch

Windows build status:

[![Windows build status](https://ci.appveyor.com/api/projects/status/s22xej0ai5n41au2/branch/master?svg=true)](https://ci.appveyor.com/project/aensidhe/tarantool-dnx/branch/master)

Right now Linux and OSX support are dropped until dotnet-core stabilization.
