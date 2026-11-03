# language: pt
Funcionalidade: Registro de descartes de resíduos
  Como operador
  Quero registrar cada descarte feito em um ecoponto
  Para que a carga do ponto e o histórico fiquem rastreáveis

  @positivo
  Cenário: Operador registra um descarte e a carga do ponto é atualizada
    Dado que existe um ponto de coleta com capacidade de 100 kg
    Quando o operador registra um descarte de 10 kg nesse ponto
    Então o status code deve ser 201
    E o corpo deve respeitar o contrato "descarte-criado.json"
    Quando consulto o ponto de coleta criado
    Então o campo "cargaAtualKg" deve ser igual a "10"

  @positivo
  Cenário: Descarte registrado pode ser consultado pelo id (rastreabilidade)
    Dado que existe um ponto de coleta com capacidade de 100 kg
    E que o operador registrou um descarte de 10 kg nesse ponto
    Quando consulto o descarte registrado
    Então o status code deve ser 200
    E o corpo deve respeitar o contrato "descarte.json"
    E o campo "pesoKg" deve ser igual a "10"

  @positivo
  Cenário: Lista de descartes
    Quando envio uma requisição GET para "/api/descarte"
    Então o status code deve ser 200
    E o corpo deve respeitar o contrato "lista-descartes.json"

  @negativo
  Cenário: Descarte em ponto inexistente é rejeitado
    Quando o operador registra um descarte de 10 kg no ponto 999999
    Então o status code deve ser 400

  @negativo
  Cenário: Descarte com peso negativo é rejeitado
    Dado que existe um ponto de coleta com capacidade de 100 kg
    Quando o operador registra um descarte de -5 kg nesse ponto
    Então o status code deve ser 400

  @negativo
  Cenário: Descarte sem token é bloqueado
    Dado que não estou autenticado
    Quando registro um descarte de 10 kg no ponto 1
    Então o status code deve ser 401

  @negativo
  Cenário: Consulta de descarte inexistente retorna 404
    Quando envio uma requisição GET para "/api/descarte/999999"
    Então o status code deve ser 404
