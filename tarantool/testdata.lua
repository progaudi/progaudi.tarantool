local log = require("log")
local remote = require("net.box")
local compat = require("compat")

local new_parts_format_supported = compat.check_version{1, 7, 6}
local is_nullable_supported = compat.check_version{1, 7, 6}

local function create_space(space)
    log.info({space=space.name, status="creating"})
    local format = {}
    for name, field in pairs(space.fields) do
        format[field.index] = { name=name, type=field.type, is_nullable=(field.is_nullable == true) }
    end

    if space.sequence ~= nil then
        box.schema.sequence.create(space.sequence, {if_not_exists = true})
    end

    local created_space = box.schema.space.create(space.name, { format = format, if_not_exists = true, temporary = space.temporary})
    log.info({space=space.name, status="created"})
    return created_space
end

local function create_index(space, name, unique, type, sequence, ...)
	local parts = {}
	for i, v in ipairs({...}) do
		if new_parts_format_supported then
			table.insert(parts, v.name)
		else
			table.insert(parts, v.index)
			table.insert(parts, v.type)
		end
	end

	local options = {
		unique = unique,
		type = type,
		parts = parts,
		if_not_exists = true,
		sequence = sequence
	}

	local index = space:create_index(name, options)

	return index
end

local function create_index(space, name, unique, type, sequence_name, ...)
    log.info({space=space.name, index=name, status="creating"})
    local parts = {}

    for k, v in pairs({...}) do
        table.insert(parts, v)
    end

    local options = {
        unique = unique,
        type = type,
        parts = parts,
        if_not_exists = true,
    }

    log.info({space=space.name, index=name, status="options", options=options})	
    local index = space:create_index(name, options)
    if sequence_name ~= nil then
        index:alter({sequence = sequence_name})
    end

    log.info({space=space.name, index=name, status="created"})
    return index
end

local spaces = {
	primary_only_index = {
		name = "primary_only_index",
		fields = {
			id =      { index = 1, type = "unsigned" },
			name =    { index = 2, type = "string" },
			price =   { index = 3, type = "scalar", is_nullable=true }
		}
	},
	performance = {
		name = "performance",
		fields = {
			id =      { index = 1, type = "unsigned" },
			name =    { index = 2, type = "string" }
		}
	},
	treeIndexMethods = {
		name = "space_TreeIndexMethods",
		sequence = "space_TreeIndexMethods_id",
		fields = {
			id =      { index = 1, type = "unsigned" },
			name =    { index = 2, type = "string" }
		}
	},
	with_scalar_index = {
		name = "with_scalar_index",
		fields = {
			id =      { index = 1, type = "scalar" }
		}
	},
	pivot = {
		name = "pivot",
		temporary = true,
		fields = {
			id =      { index = 1, type = "unsigned"},
			arr =     { index = 2, type = "array" }
		}
	}
}

local function create_spaces_and_indecies()
	local space = create_space(spaces.primary_only_index)
	create_index(space, "primary", true, "HASH", nil, "id")

	space = create_space(spaces.performance)
	create_index(space, "primary", true, "HASH", nil, "id")

	space = create_space(spaces.with_scalar_index)
	create_index(space, "primary", true, "TREE", nil, "id")

	space = create_space(spaces.pivot)
	create_index(space, "primary", true, "TREE", nil, "id")
	create_index(space, "rtree", false, "RTREE", nil, "arr")

	space = create_space(spaces.treeIndexMethods)
	create_index(space, "primary", true, "TREE", spaces.treeIndexMethods.sequence, "id")

	space:insert{nil, "asdf"}
	space:insert{nil, "zcxv"}
	space:insert{nil, "qwer"}
end

local function init()
	log.info{stage="Spaces",status="begin"}
	create_spaces_and_indecies()
	log.info{stage="Spaces",status="end"}

	log.info{stage="Users",status="begin"}
	box.schema.user.create("notSetPassword", { if_not_exists = true })
	box.schema.user.create("emptyPassword", { password = "", if_not_exists = true })

	box.schema.user.create("operator", {password = "operator", if_not_exists = true })
	log.info{stage="Users",status="end"}
	
	log.info{stage="Grants",status="begin"}

	local users = {"operator", "guest", "emptyPassword"}

	for _, user in pairs(users) do
		box.schema.user.grant(user, "execute", "universe", nil, { if_not_exists = true })

		for _, space in pairs(spaces) do
			box.schema.user.grant(user, 'read,write', 'space', space.name, {if_not_exists = true})
			if space.sequence ~= nil then
				box.schema.user.grant(user, 'read,write', 'sequence', space.sequence, {if_not_exists = true})
			end
		end
	end

	log.info{stage="Grants",status="end"}
end

box.once("init", init)

local log = require("log")

function log_connect ()
	local m = "Connection. user=" .. box.session.user() .. " id=" .. box.session.id()
	log.info(m)
end
function log_disconnect ()
	local m = "Disconnection. user=" .. box.session.user() .. " id=" .. box.session.id()
	log.info(m)
end

function log_auth ()
	local m = "Authentication attempt"
	log.info(m)
end
function log_auth_ok (user_name)
	local m = "Authenticated user " .. user_name
	log.info(m)
end

box.session.on_connect(log_connect)
box.session.on_disconnect(log_disconnect)
box.session.on_auth(log_auth)
box.session.on_auth(log_auth_ok)

function return_null()
	log.info("return_null called")
	return require("msgpack").NULL
end

function return_tuple_with_null()
	log.info("return_tuple_with_null called")
	return { require("msgpack").NULL }
end

function return_tuple()
	log.info("return_tuple called")
	return { 1, 2 }
end


function return_array()
	log.info("return_array called")
	return {{ "abc", "def" }}
end

function return_scalar()
	log.info("return_scalar called")
	return 1
end

function return_nothing()
	log.info("return_nothing called")
end

local truncate_space = function(name)
	local space = box.space[name]

	if space then
		log.info("Truncating space %s...", name)
		space:truncate()
		log.info("Space %s trancated.", name)
	else
		log.warning("There is no space %s", name)
	end
end

function create_sql_test()
	box.sql.execute("create table sql_test(id int primary key, name text)")
	box.sql.execute("insert into sql_test values (1, 'asdf'), (2, 'zxcv'), (3, 'qwer')")
end

function drop_sql_test()
	box.sql.execute("drop table sql_test")
end

function clear_data(spaceNames)
	log.info("clearing data...")
	for _, spaceName in ipairs(spaceNames) do
		truncate_space(spaceName)
	end
end

local test_int = 0
function test_for_benchmarking()
	test_int = test_int + 1
	return test_int
end
