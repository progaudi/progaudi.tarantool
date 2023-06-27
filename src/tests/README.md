To run integration tests you should have runned Tarantool instance.
You can run it locally via command

docker-compose up -d

It runs empty Tarantool instances of different versions, exposed on different ports.
Command examples to run integration tests suite for particular Tarantool instance

dotnet test --filter "DisplayName~progaudi.tarantool.integration.tests" -e TARANTOOL_HOST_FOR_TESTS=127.0.0.1:3310
dotnet test --filter "DisplayName~progaudi.tarantool.integration.tests" -e TARANTOOL_HOST_FOR_TESTS=127.0.0.1:3311
