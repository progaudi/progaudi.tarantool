-- All credits goes to https://github.com/tarantool/queue/blob/master/queue/compat.lua

local fun   = require('fun')

local iter, op  = fun.iter, fun.operator

local function split(self, sep)
    local sep, fields = sep or ":", {}
    local pattern = string.format("([^%s]+)", sep)
    self:gsub(pattern, function(c) table.insert(fields, c) end)
    return fields
end

local function reducer(res, l, r)
    if res ~= nil then
        return res
    end
    if tonumber(l) == tonumber(r) then
        return nil
    end
    return tonumber(l) > tonumber(r)
end

local function split_version(version_string)
    local vtable  = split(version_string, '.')
    local vtable2 = split(vtable[3],  '-')
    vtable[3], vtable[4] = vtable2[1], vtable2[2]
    return vtable
end

local function check_version(expected, version)
    version = version or _TARANTOOL
    if type(version) == 'string' then
        version = split_version(version)
    end
    local res = iter(version):zip(expected):reduce(reducer, nil)
    if res or res == nil then res = true end
    return res
end

return {
    check_version = check_version
}