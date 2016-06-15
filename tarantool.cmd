docker rm -f tarantool-tests
docker build --tag=tarantool/csharp.client.tests .
docker run -d --publish=3301:3301 --name=tarantool-tests tarantool/csharp.client.tests