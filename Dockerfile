FROM progaudi/dotnet:1.0.1-xenial

WORKDIR /app

# copy csproj and restore as distinct layers
COPY . .
RUN /root/.dotnet/dotnet restore
RUN /root/.dotnet/dotnet build -c Release -f netstandard1.4 src/progaudi.tarantool/progaudi.tarantool.csproj
RUN /root/.dotnet/dotnet build -c Release -f netcoreapp1.0 tests/progaudi.tarantool.tests/progaudi.tarantool.tests.csproj
RUN /root/.dotnet/dotnet build -c Release -f netcoreapp1.1 tests/progaudi.tarantool.tests/progaudi.tarantool.tests.csproj