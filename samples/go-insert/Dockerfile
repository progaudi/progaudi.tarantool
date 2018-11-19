FROM golang:alpine

RUN apk add git

WORKDIR $GOPATH/src/insert

ADD . $GOPATH/src/insert

RUN go get
RUN go install insert

CMD ["main"]
