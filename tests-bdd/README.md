# 🧪 Testes BDD — Cidades ESGInteligentes

Suíte de testes de API em **BDD (Gherkin)** com **Java 17 + Cucumber + REST Assured**, validando status code, corpo JSON e **contrato com JSON Schema** da API .NET 8.

## Estrutura

```
tests-bdd/
├── pom.xml
├── Dockerfile · docker-compose.yml
└── src/test/
    ├── java/.../RunCucumberTest.java   # runner (JUnit Platform Suite)
    ├── java/.../ApiSteps.java          # implementação dos steps
    └── resources/
        ├── features/                   # cenários Gherkin (pt)
        ├── schemas/                    # contratos JSON Schema
        └── payloads/                   # corpos das requisições
```

## Cenários

| Feature | Cenários | Cobertura |
|---|---|---|
| 01 Saúde e autenticação | health, login gestor/operador, senha incorreta, login vazio (400) | `/health`, `POST /api/auth/login` |
| 02 Pontos de coleta | cadastrar, listar paginado, consultar, atualizar, excluir, 404, dados inválidos (400), sem token (401), operador (403) | `/api/pontocoleta` (GET, POST, PUT, DELETE) |
| 03 Descartes | registrar e atualizar carga, consultar por id, listar, ponto inexistente (400), peso negativo (400), sem token (401), 404 | `/api/descarte` (GET, POST) |
| 04 Alertas | alerta aos 80%, sem alerta abaixo de 80%, listar, resolver (zera a carga), operador (403), sem token (401), 404 | `/api/alerta` (GET, PUT resolver) |
| 05 Relatórios | contrato do resumo, descarte refletido no total | `GET /api/relatorio/resumo` |

Todos os cenários validam o status code; os que retornam JSON também validam o corpo e o **contrato JSON Schema** (`src/test/resources/schemas/`). Cenários marcados `@positivo` ou `@negativo`.

## Como executar

**Pré-requisitos:** Java 17 + Maven 3.9 (ou apenas Docker).

**Opção 1 — Docker Compose (sobe API + testes):** na raiz do repositório

```bash
docker compose -f tests-bdd/docker-compose.yml up --build --abort-on-container-exit --exit-code-from tests
docker compose -f tests-bdd/docker-compose.yml down -v
```

**Opção 2 — Maven contra uma API já rodando:**

```bash
docker compose up -d --build                 # API em http://localhost:8080 (raiz do repo)
cd tests-bdd
mvn test                                     # usa http://localhost:8080
mvn test -DbaseUrl=https://SEU-APP.azurewebsites.net   # outro ambiente (ex.: staging)
```

Executar só um grupo de cenários: `mvn test -Dcucumber.filter.tags="@negativo"`.

**Relatórios:** `tests-bdd/target/cucumber-reports/report.html` (e `report.json`).

## CI (GitHub Actions)

O workflow `.github/workflows/bdd-tests.yml` sobe a API via Docker Compose, executa a suíte e publica o relatório como artefato (`cucumber-report`).

## Usuários de demonstração

`gestor01` e `operador01`, senha `pass123` (seed da API).

## Documentação (PDF)

`docs/Documentacao_Testes_BDD.pdf` descreve os cenários, contratos, execução e evidências. Para regenerar com os prints da sua execução: salve os prints do GitHub Actions em `docs/evidencias/` (nomes listados em `docs/gerar_documentacao.py`), rode `pip install reportlab pillow` e `python docs/gerar_documentacao.py`.
