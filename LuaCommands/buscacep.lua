local http = require("socket.http")

function command(cep)
    local response, _ = http.request("https://viacep.com.br/ws/" + cep + "/json/")
    return response
end
