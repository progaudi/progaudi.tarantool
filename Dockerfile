FROM tarantool/tarantool

RUN mkdir -p /opt/tarantool/work_dir

COPY tarantool.lua /opt/tarantool/tarantool.lua

RUN apt-get install -qqy dos2unix && \
	dos2unix /opt/tarantool/tarantool.lua

CMD ["tarantool", "/opt/tarantool/tarantool.lua"]