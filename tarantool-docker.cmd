docker rm -f tarantool-tests
docker build --tag=tarantool/csharp.client.tests .
docker run --publish=3301:3301 --name=tarantool-tests -t -i tarantool/csharp.client.tests