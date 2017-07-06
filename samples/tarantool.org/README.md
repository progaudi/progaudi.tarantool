# Tutorial

The most commonly used C# driver is [progaudi.tarantool](https://github.com/progaudi/progaudi.tarantool). It was previously named `tarantool-csharp`. It is not supplied as part of the Tarantool repository; it must be installed separately. The makers recommend cross-platform installation using Nuget. To be consistent with the other instructions in this chapter, here is an way to install the driver directly on Ubuntu 16.04.

## Install .net core from Microsoft

Use .net core installation instructions at https://www.microsoft.com/net/core#ubuntu
NOTE:
1. Mono will not work, nor will .Net from xbuild. Only .net core supported on linux and mac.
2. Read the Microsoft End User License Agreement first, because it is not an ordinary open-source agreement and there will be a message during installation saying “This software may collect information about you and your use of the software, and send that to Microsoft.”
3. You can opt-out from telemetry by setting environment variable. See instructions at https://docs.microsoft.com/en-us/dotnet/core/tools/telemetry#behavior


## Create new console project

```
cd ~
mkdir progaudi.tarantool.test
cd progaudi.tarantool.test
dotnet new console
```

## Add progaudi.tarantool reference

```
dotnet add package progaudi.tarantool
```

## Change code in Program.cs

```
cat <<EOT > Program.cs
using System;
using System.Threading.Tasks;
using ProGaudi.Tarantool.Client;

public class HelloWorld
{
  static public void Main ()
  {
    Test().GetAwaiter().GetResult();
  }
  static async Task Test()
  {
    var box = await Box.Connect("127.0.0.1:3301");
    var schema = box.GetSchema();
    var space = await schema.GetSpace("examples");
    await space.Insert((99999, "BB"));
  }
}
EOT

```

## Build and run your app

Before trying to run, check that the server is listening at `localhost:3301` and that the space `examples` exists, as described [earlier](https://tarantool.org/en/doc/1.7/book/connectors/index.html#index-connector-setting).

```
dotnet restore
dotnet run
```

The program will connect using an application-specific definition of the space. The program will open a socket connection with the Tarantool server at `localhost:3301`, then send an INSERT request, then — if all is well — end without saying anything. If Tarantool is not running on localhost with listen port = 3301, or if user guest does not have authorization to connect, or if the insert request fails for any reason, the program will print an error message, among other things (stacktrace, etc).

The example program only shows one request and does not show all that’s necessary for good practice. For that, please see the [progaudi.tarantool](https://github.com/progaudi/progaudi.tarantool) driver repository.
