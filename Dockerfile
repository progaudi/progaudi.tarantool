FROM progaudi/dotnet:1.1-preview4

WORKDIR /app

# copy csproj and restore as distinct layers
COPY . .
RUN dotnet --version
RUN dotnet restore
RUN dotnet build -c Release -f netstandard1.4 src/tarantool.client/tarantool.client.csproj
RUN dotnet build -c Release -f netcoreapp1.0 tests/tarantool.client.tests/tarantool.client.tests.csproj