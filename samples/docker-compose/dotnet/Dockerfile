FROM microsoft/dotnet:1.0.0-preview2-sdk

ENV APP_HOME /app
RUN mkdir -p $APP_HOME

ADD project.json /tmp/build/project.json

RUN cd /tmp/build \
    && dotnet restore . \
    && mv /tmp/build/* /app

ADD . /app
WORKDIR /app

RUN dotnet build

EXPOSE 5000/tcp

CMD ["dotnet", "run", "--server.urls", "http://*:5000"]
