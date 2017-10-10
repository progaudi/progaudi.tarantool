FROM microsoft/dotnet:2.0.0-sdk

WORKDIR /app
ADD dotnet.csproj /app/dotnet.csproj
RUN dotnet restore

ADD . /app
RUN dotnet build -c Release -f netcoreapp2.0 /app/dotnet.csproj

EXPOSE 5000/tcp

CMD ["dotnet", "run"]
