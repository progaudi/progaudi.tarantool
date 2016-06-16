box.cfg
	{
	 listen = 3301
	}

box.schema.user.passwd('')
box.schema.user.grant('guest','read,write,execute','universe')

function log_connect ()
  local log = require('log')
  local m = 'Connection. user=' .. box.session.user() .. ' id=' .. box.session.id()
  log.info(m)
end
function log_disconnect ()
  local log = require('log')
  local m = 'Disconnection. user=' .. box.session.user() .. ' id=' .. box.session.id()
  log.info(m)
end

function log_auth ()
  local log = require('log')
  local m = 'Authentication attempt'
  log.info(m)
end
function log_auth_ok (user_name)
  local log = require('log')
  local m = 'Authenticated user ' .. user_name
  log.info(m)
end

box.session.on_connect(log_connect)
box.session.on_disconnect(log_disconnect)
box.session.on_auth(log_auth)
box.session.on_auth(log_auth_ok)