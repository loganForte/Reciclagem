# language: pt
Funcionalidade: Disponibilidade e autenticação da API
  Como gestor ou operador de um ecoponto
  Quero me autenticar na API
  Para que apenas usuários autorizados alterem dados (governança)

  @positivo @smoke
  Cenário: Health check responde que a API está saudável
    Quando envio uma requisição GET para "/health"
    Então o status code deve ser 200

  @positivo
  Cenário: Login válido do gestor retorna token JWT
    Quando faço login com usuário "gestor01" e senha "pass123"
    Então o status code deve ser 200
    E o corpo deve respeitar o contrato "login-response.json"

  @positivo
  Cenário: Login válido do operador retorna token JWT
    Quando faço login com usuário "operador01" e senha "pass123"
    Então o status code deve ser 200
    E o corpo deve respeitar o contrato "login-response.json"

  @negativo
  Cenário: Login com senha incorreta é rejeitado
    Quando faço login com usuário "gestor01" e senha "senha-errada"
    Então o status code deve ser 401

  @negativo
  Cenário: Login sem informar usuário e senha é rejeitado
    Quando faço login com usuário "" e senha ""
    Então o status code deve ser 400
