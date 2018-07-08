FROM microsoft/dotnet:2.1-sdk as sdk

WORKDIR /app

# copy csproj and restore as distinct layers
COPY . .
RUN dotnet build -c Release progaudi.tarantool.sln
RUN dotnet msbuild /t:publish /p:NoBuild=True /p:Configuration=Release samples/insert-performance/insert-performance.csproj

FROM microsoft/dotnet:2.1-runtime as runtime

WORKDIR /app

COPY --from=sdk /app/samples/insert-performance/bin/Release/netcoreapp2.1/publish .

CMD ["dotnet", "insert-performance.dll"]
