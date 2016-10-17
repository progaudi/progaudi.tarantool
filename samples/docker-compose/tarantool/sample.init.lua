#!/usr/bin/env tarantool

-- Add Taranrocks pathes. https://github.com/rtsisyk/taranrocks/blob/master/README.md
local home = os.getenv("HOME")
package.path = [[/usr/local/share/tarantool/lua/?/init.lua;]]..package.path
package.path = [[/usr/local/share/tarantool/lua/?.lua;]]..package.path
package.path = home..[[/.tarantool/share/tarantool/lua/?/init.lua;]]..package.path
package.path = home..[[/.tarantool/share/tarantool/lua/?.lua;]]..package.path
package.cpath = [[/usr/local/lib/tarantool/lua/?.so;]]..package.cpath
package.cpath = home..[[/.tarantool/lib/tarantool/lua/?.so;]]..package.cpath

local log = require('log')

box.cfg
{
    pid_file = nil,
    background = false,
    log_level = 5
}

local function init()
    box.schema.user.create('operator', {password = '123123', if_not_exists = true})
    box.schema.user.grant('operator', 'read,write,execute', 'universe', nil, { if_not_exists = true })

    box.schema.user.create('replicator', {password = '234234', if_not_exists = true})
    box.schema.user.grant('replicator','execute','role','replication', { if_not_exists = true })

    some_space = box.schema.space.create('some_space', { field_count = 3, format = {
        [1] = {["name"] = "id"},
        [2] = {["name"] = "text"},
        [3] = {["name"] = "int"}
    }})
    log.info(some_space.name .. " space was created.")

    some_space:create_index('primary', {
        if_not_exists = true,
        type = 'TREE',
        unique = true,
        parts = {1, 'INT'}
    })

    some_space:create_index('some_secondary_index', {
        if_not_exists = true,
        type = 'TREE',
        unique = false,
        parts = {3, 'INT'}
    })
end

local function data()
    box.space.some_space:auto_increment{'Masya', 10}
    box.space.some_space:auto_increment{'Armata', 0}
end

box.once('init', init)
box.once('data', data)