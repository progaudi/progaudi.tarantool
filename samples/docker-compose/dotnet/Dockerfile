FROM progaudi/dotnet:1.0.1-xenial

WORKDIR /app
ADD dotnet.csproj /app/dotnet.csproj
RUN /root/.dotnet/dotnet restore

ADD . /app
RUN /root/.dotnet/dotnet build -c Release -f netcoreapp1.1 /app/dotnet.csproj

EXPOSE 5000/tcp

CMD ["/root/.dotnet/dotnet", "run", "--server.urls", "http://*:5000"]
