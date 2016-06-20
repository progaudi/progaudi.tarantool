FROM tarantool/tarantool

RUN mkdir -p /etc/tarantool/client-tests

COPY tarantool.lua /etc/tarantool/client-tests/tarantool.lua

RUN apt-get install -qqy dos2unix && \
	dos2unix /etc/tarantool/client-tests/tarantool.lua

CMD ["tarantool", "/etc/tarantool/client-tests/tarantool.lua"]