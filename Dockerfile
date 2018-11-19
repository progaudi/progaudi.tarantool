FROM microsoft/dotnet:2.1-sdk as sdk

WORKDIR /app

COPY progaudi.tarantool.sln .

RUN mkdir -p /app/src/progaudi.tarantool
COPY src/progaudi.tarantool/progaudi.tarantool.csproj /app/src/progaudi.tarantool/progaudi.tarantool.csproj

RUN mkdir -p /app/samples/insert-performance
COPY samples/insert-performance/insert-performance.csproj /app/samples/insert-performance/insert-performance.csproj

RUN dotnet restore progaudi.tarantool.sln

# copy csproj and restore as distinct layers
COPY . .
RUN dotnet publish -c Release -f netcoreapp2.1 progaudi.tarantool.sln

FROM microsoft/dotnet:2.1-runtime as runtime

WORKDIR /app

COPY --from=sdk /app/samples/insert-performance/bin/Release/netcoreapp2.1/publish .

CMD ["dotnet", "insert-performance.dll"]
