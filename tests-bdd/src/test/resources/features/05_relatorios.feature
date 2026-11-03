# language: pt
Funcionalidade: Relatório consolidado de resíduos
  Como gestor
  Quero consultar o resumo por tipo de resíduo e por ponto
  Para decidir rotas e investimentos com base em dados

  @positivo
  Cenário: Resumo geral respeita o contrato
    Quando envio uma requisição GET para "/api/relatorio/resumo"
    Então o status code deve ser 200
    E o corpo deve respeitar o contrato "relatorio-resumo.json"

  @positivo
  Cenário: Descarte aparece no total coletado do relatório
    Dado que existe um ponto de coleta com capacidade de 100 kg
    E que o operador registrou um descarte de 10 kg nesse ponto
    Quando envio uma requisição GET para "/api/relatorio/resumo"
    Então o status code deve ser 200
    E o ponto de coleta criado deve constar no relatório com 10 kg de carga
