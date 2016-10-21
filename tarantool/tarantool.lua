box.cfg
{
    pid_file = nil,
    background = false,
    log_level = 5
}

local function init()
    space1 = box.schema.space.create('primary_only_index', { if_not_exists = true })
    space1:create_index('primary', {type='hash', parts={1, 'unsigned'}, if_not_exists = true})

    performanceSpace = box.schema.space.create('performance', { if_not_exists = true })
    performanceSpace:create_index('primary', {type='hash', parts={1, 'unsigned'}, if_not_exists = true})

    space2 = box.schema.space.create('primary_and_secondary_index', { if_not_exists = true })
    space2:create_index('hashIndex', {type='hash', parts={1, 'unsigned'}, if_not_exists = true })
    space2:create_index('treeIndex', {type='tree', parts={1, 'unsigned'}, if_not_exists = true })

    box.schema.user.create('notSetPassword', { if_not_exists = true })
    box.schema.user.create('emptyPassword', { password = '', if_not_exists = true })

    box.schema.user.create('operator', {password = 'operator', if_not_exists = true })
    box.schema.user.grant('operator','read,write,execute','universe', { if_not_exists = true })

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
end

box.once('init', init)