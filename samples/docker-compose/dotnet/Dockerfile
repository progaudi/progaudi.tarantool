FROM progaudi/dotnet:1.1-preview4

ADD . /app
WORKDIR /app

RUN dotnet restore
RUN dotnet build -c Release -f netcoreapp1.1 /app/dotnet.csproj

EXPOSE 5000/tcp

CMD ["dotnet", "run", "--server.urls", "http://*:5000"]
