local log = require("log")
local remote = require('net.box')
local compat = require('compat')

local new_parts_format_supported = compat.check_version{1, 7, 6}
local is_nullable_supported = compat.check_version{1, 7, 6}

local function create_space(space)
	log.info{message2="Creating space.", name=space.name}
	local format = {}
	for name, field in pairs(space.fields) do
		format[field.index] = { name=name, type=field.type }
		if is_nullable_supported then
			format[field.index].is_nullable = false
			if field.is_nullable then
				format[field.index].is_nullable = true
			end
		end
	end

	local created_space = box.schema.space.create(space.name, { format = format, if_not_exists=true, field_count=#format })
	log.info{message2="Created space.", name=space.name}
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

	log.info{message2="Creating index", space=space.name, name=name, options=options}

	local index = space:create_index(name, options)

	log.info{message2="Created index", space=space.name, name=name}
	return index
end

local spaces = {
	primary_only_index = {
		name = "primary_only_index",
		fields = {
			id =      { index = 1, name="id", type = "unsigned" },
			name =    { index = 2, name="name", type = "string" },
			price =   { index = 3, name="price", type = "scalar", is_nullable=true }
		}
	},
	performance = {
		name = "performance",
		fields = {
			id =      { index = 1, name="id", type = "unsigned" },
			name =    { index = 2, name="name", type = "string" }
		}
	},
	treeIndexMethods = {
		name = "space_TreeIndexMethods",
		fields = {
			id =      { index = 1, name="id", type = "unsigned" },
			name =    { index = 2, name="name", type = "string" }
		}
	}
}

local function create_spaces_and_indecies()
	local space = create_space(spaces.primary_only_index)
	create_index(space, "primary", true, "HASH", nil, spaces.primary_only_index.fields.id)

	space = create_space(spaces.performance)
	create_index(space, "primary", true, "HASH", nil, spaces.performance.fields.id)

	space = box.schema.space.create('with_scalar_index', { if_not_exists = true })
	space:create_index('primary', {type='tree', parts={1, 'scalar'}, if_not_exists = true})
end

local function init()
	create_spaces_and_indecies()

	box.schema.user.create('notSetPassword', { if_not_exists = true })
	box.schema.user.create('emptyPassword', { password = '', if_not_exists = true })

	box.schema.user.create('operator', {password = 'operator', if_not_exists = true })
	box.schema.user.grant('operator','read,write,execute','universe', { if_not_exists = true })
	box.schema.user.grant('guest','read,write,execute','universe', { if_not_exists = true })
	box.schema.user.grant('emptyPassword','read,write,execute','universe', { if_not_exists = true })
	box.schema.user.passwd('admin', 'adminPassword')
end

local function space_TreeIndexMethods()
	local sequence = box.schema.sequence.create('space_TreeIndexMethods_id')
	local space = create_space(spaces.treeIndexMethods)
	create_index(space, "treeIndex", true, "TREE", sequence.name, spaces.treeIndexMethods.fields.id)

	space:insert{nil, 'asdf'}
	space:insert{nil, 'zcxv'}
	space:insert{nil, 'qwer'}
end

box.once('init', init)
box.once('space_TreeIndexMethods', space_TreeIndexMethods)

local log = require('log')

function log_connect ()
	local m = 'Connection. user=' .. box.session.user() .. ' id=' .. box.session.id()
	log.info(m)
end
function log_disconnect ()
	local m = 'Disconnection. user=' .. box.session.user() .. ' id=' .. box.session.id()
	log.info(m)
end

function log_auth ()
	local m = 'Authentication attempt'
	log.info(m)
end
function log_auth_ok (user_name)
	local m = 'Authenticated user ' .. user_name
	log.info(m)
end

box.session.on_connect(log_connect)
box.session.on_disconnect(log_disconnect)
box.session.on_auth(log_auth)
box.session.on_auth(log_auth_ok)

function return_null()
	log.info('return_null called')
	return require('msgpack').NULL
end

function return_tuple_with_null()
	log.info('return_tuple_with_null called')
	return { require('msgpack').NULL }
end

function return_tuple()
	log.info('return_tuple called')
	return { 1, 2 }
end

function return_array()
	log.info('return_array called')
	return {{ "abc", "def" }}
end

function return_scalar()
	log.info('return_scalar called')
	return 1
end

function return_nothing()
	log.info('return_nothing called')
end

function replace(t)
	return box.space.with_scalar_index:replace(t)
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
	box.sql.execute('create table sql_test(id int primary key, name text)')
	box.sql.execute("insert into sql_test values (1, 'asdf'), (2, 'zxcv'), (3, 'qwer')")
end

function drop_sql_test()
	box.sql.execute('drop table sql_test')
end

function clear_data(spaceNames)
	log.info('clearing data...')
	for _, spaceName in ipairs(spaceNames) do
		truncate_space(spaceName)
	end
end

local test_int = 0
function test_for_benchmarking()
	test_int = test_int + 1
	return test_int
end
