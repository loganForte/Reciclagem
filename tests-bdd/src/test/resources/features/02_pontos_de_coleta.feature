# language: pt
Funcionalidade: Gestão de pontos de coleta
  Como gestor
  Quero cadastrar, consultar, atualizar e excluir ecopontos
  Para acompanhar a capacidade de cada ponto de coleta

  @positivo
  Cenário: Gestor cadastra um ponto de coleta
    Dado que estou autenticado como "gestor01"
    Quando cadastro um novo ponto de coleta com capacidade de 100 kg
    Então o status code deve ser 201
    E o corpo deve respeitar o contrato "ponto-coleta.json"
    E o campo "cargaAtualKg" deve ser igual a "0"

  @positivo
  Cenário: Lista paginada de pontos de coleta
    Quando envio uma requisição GET para "/api/pontocoleta?pagina=1&tamanho=5"
    Então o status code deve ser 200
    E o corpo deve respeitar o contrato "lista-pontos.json"
    E o campo "tamanho" deve ser igual a "5"

  @positivo
  Cenário: Consulta de ponto de coleta pelo id
    Dado que estou autenticado como "gestor01"
    E que existe um ponto de coleta com capacidade de 100 kg
    Quando consulto o ponto de coleta criado
    Então o status code deve ser 200
    E o corpo deve respeitar o contrato "ponto-coleta.json"

  @positivo
  Cenário: Gestor atualiza um ponto de coleta
    Dado que estou autenticado como "gestor01"
    E que existe um ponto de coleta com capacidade de 100 kg
    Quando atualizo o ponto de coleta criado com capacidade de 250 kg
    Então o status code deve ser 204
    Quando consulto o ponto de coleta criado
    Então o campo "capacidadeMaximaKg" deve ser igual a "250"

  @positivo
  Cenário: Gestor exclui um ponto de coleta e ele deixa de existir
    Dado que estou autenticado como "gestor01"
    E que existe um ponto de coleta com capacidade de 100 kg
    Quando excluo o ponto de coleta criado
    Então o status code deve ser 204
    Quando consulto o ponto de coleta criado
    Então o status code deve ser 404

  @negativo
  Cenário: Consulta de ponto inexistente retorna 404
    Quando envio uma requisição GET para "/api/pontocoleta/999999"
    Então o status code deve ser 404

  @negativo
  Cenário: Cadastro com dados inválidos retorna 400
    Dado que estou autenticado como "gestor01"
    Quando cadastro um ponto de coleta com dados inválidos
    Então o status code deve ser 400

  @negativo
  Cenário: Cadastro sem token é bloqueado
    Dado que não estou autenticado
    Quando cadastro um novo ponto de coleta com capacidade de 100 kg
    Então o status code deve ser 401

  @negativo
  Cenário: Operador não pode cadastrar ponto de coleta
    Dado que estou autenticado como "operador01"
    Quando cadastro um novo ponto de coleta com capacidade de 100 kg
    Então o status code deve ser 403

  @negativo
  Cenário: Exclusão de ponto inexistente retorna 404
    Dado que estou autenticado como "gestor01"
    Quando envio uma requisição DELETE para "/api/pontocoleta/999999"
    Então o status code deve ser 404
