box.cfg
{
	pid_file = 'tarantool.pid',
	background = true,
	log_level = 5,
	listen = 3301,
	log = 'file: tarantool.log'
}

require('testdata')
