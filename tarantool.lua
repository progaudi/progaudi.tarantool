local port = 3301
local max_memory_gb = 0.1
local background = false
local logger = nil
local pid_file = nil
local work_dir = '/opt/tarantool/work_dir'

if arg[1] == "daemon" then
    background = true
    logger = "tarantool.log"
    pid_file = "tarantool.pid"
end

box.cfg
{
  listen = port,
  slab_alloc_arena = max_memory_gb,
  logger = logger,
  pid_file = pid_file,
  background = background,
  work_dir = work_dir
}

space1 = box.schema.space.create('primary_only_index')
space1:create_index('primary', {type='hash',  parts={1, 'NUM'}})

performanceSpace = box.schema.space.create('performance')
performanceSpace:create_index('primary', {type='hash',  parts={1, 'NUM'}})


space2 = box.schema.space.create('primary_and_secondary_index')
space2:create_index('hashIndex', {type='hash',  parts={1, 'NUM'}})
space2:create_index('treeIndex', {type='tree',  parts={1, 'NUM'}})



box.schema.user.passwd('')
box.schema.user.grant('guest','read,write,execute','universe')


box.schema.user.create('notSetPassword')
box.schema.user.create('emptyPassword', {password = ''})

box.schema.user.create('operator', {password = 'operator'})
box.schema.user.grant('operator','read,write,execute','universe')

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