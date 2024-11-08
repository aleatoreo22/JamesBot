local cep = tostring(arg[1])

function Get(uri)
    local req_body = arg[2]
    local req_timeout = 10

    local request = require "http.request"

    local req = request.new_from_uri(uri)
    if req_body then
        req.headers:upsert(":method", "POST")
        req:set_body(req_body)
    end
    for k, v in req.headers:each() do
    end
    if req.body then
    end
    local headers, stream = req:go(req_timeout)
    if headers == nil then
        io.stderr:write(tostring(stream), "\n")
        os.exit(1)
    end
    for k, v in headers:each() do
    end
    local body, err = stream:get_body_as_string()
    if not body and err then
        io.stderr:write(tostring(err), "\n")
        os.exit(1)
    end
    return body
end

print(Get("https://viacep.com.br/ws/" .. cep .. "/json/"))
