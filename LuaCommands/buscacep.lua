function Get(uri)
    local handle = io.popen("curl " .. uri)
    local result = handle:read("*a") -- Lê toda a saída do comando
    handle:close()
    return result
end

function command(cep)
    return Get("https://viacep.com.br/ws/" .. cep .. "/json/")
end

print(command("21911130"))
