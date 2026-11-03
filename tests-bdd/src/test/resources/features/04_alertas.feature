# language: pt
Funcionalidade: Alertas automáticos de capacidade
  Como gestor de resíduos
  Quero ser avisado quando um ecoponto atingir 80% da capacidade
  Para coletar no momento certo e evitar transbordo

  @positivo
  Cenário: Descarte que atinge 80% da capacidade gera alerta
    Dado que existe um ponto de coleta com capacidade de 100 kg
    Quando o operador registra um descarte de 85 kg nesse ponto
    Então o status code deve ser 201
    E o campo "alertaGerado" deve ser igual a "true"
    E o alerta desse ponto deve estar aberto na lista de alertas

  @positivo
  Cenário: Descarte abaixo de 80% não gera alerta
    Dado que existe um ponto de coleta com capacidade de 100 kg
    Quando o operador registra um descarte de 79 kg nesse ponto
    Então o status code deve ser 201
    E o campo "alertaGerado" deve ser igual a "false"

  @positivo
  Cenário: Lista de alertas respeita o contrato
    Quando envio uma requisição GET para "/api/alerta"
    Então o status code deve ser 200
    E o corpo deve respeitar o contrato "lista-alertas.json"

  @positivo
  Cenário: Gestor resolve o alerta e a carga do ponto é zerada
    Dado que existe um ponto de coleta com capacidade de 100 kg
    E que o operador registrou um descarte de 85 kg nesse ponto
    E que estou autenticado como "gestor01"
    Quando resolvo o alerta desse ponto
    Então o status code deve ser 204
    Quando consulto o alerta desse ponto
    Então o status code deve ser 200
    E o corpo deve respeitar o contrato "alerta.json"
    E o campo "resolvido" deve ser igual a "true"
    Quando consulto o ponto de coleta criado
    Então o campo "cargaAtualKg" deve ser igual a "0"

  @negativo
  Cenário: Operador não pode resolver alerta
    Dado que existe um ponto de coleta com capacidade de 100 kg
    E que o operador registrou um descarte de 85 kg nesse ponto
    E que estou autenticado como "operador01"
    Quando resolvo o alerta desse ponto
    Então o status code deve ser 403

  @negativo
  Cenário: Resolver alerta sem token é bloqueado
    Dado que não estou autenticado
    Quando envio uma requisição PUT para "/api/alerta/1/resolver"
    Então o status code deve ser 401

  @negativo
  Cenário: Consulta de alerta inexistente retorna 404
    Quando envio uma requisição GET para "/api/alerta/999999"
    Então o status code deve ser 404

  @negativo
  Cenário: Resolver alerta inexistente retorna 404
    Dado que estou autenticado como "gestor01"
    Quando envio uma requisição PUT para "/api/alerta/999999/resolver"
    Então o status code deve ser 404
