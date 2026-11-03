package com.fiap.reciclagem.bdd;

import static io.restassured.RestAssured.given;
import static io.restassured.module.jsv.JsonSchemaValidator.matchesJsonSchemaInClasspath;
import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;
import static org.junit.jupiter.api.Assertions.assertTrue;

import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import io.cucumber.java.pt.Dado;
import io.cucumber.java.pt.Entao;
import io.cucumber.java.pt.Quando;
import io.restassured.http.ContentType;
import io.restassured.response.Response;
import io.restassured.specification.RequestSpecification;
import java.io.IOException;
import java.math.BigDecimal;
import java.nio.charset.StandardCharsets;
import java.util.HashMap;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

/** Steps de todas as features da API Cidades ESGInteligentes. */
public class ApiSteps {

    private static final String BASE_URL = resolverBaseUrl();
    private static final String SENHA_PADRAO = "pass123"; // usuarios do seed: operador01 e gestor01
    private static final Map<String, String> TOKENS = new ConcurrentHashMap<>();
    private static final ObjectMapper JSON = new ObjectMapper();

    private String token;        // token do usuario corrente (null = sem autenticacao)
    private Response response;   // ultima resposta
    private String pontoId;      // ponto de coleta criado no cenario
    private String descarteId;   // ultimo descarte registrado
    private String alertaId;     // alerta localizado para o ponto

    // ================================================================ Dado

    @Dado("que estou autenticado como {string}")
    public void autenticadoComo(String usuario) {
        token = tokenDe(usuario);
    }

    @Dado("que não estou autenticado")
    public void naoAutenticado() {
        token = null;
    }

    @Dado("que existe um ponto de coleta com capacidade de {int} kg")
    public void existePonto(int capacidade) {
        Response r = enviar("gestor01", "POST", "/api/pontocoleta", payloadPonto(capacidade));
        assertEquals(201, r.statusCode(), "Falha ao criar ponto de coleta: " + r.asString());
        pontoId = r.jsonPath().getString("pontoColetaId");
        assertNotNull(pontoId, "Resposta de criação sem pontoColetaId");
    }

    @Dado("que o operador registrou um descarte de {int} kg nesse ponto")
    public void operadorRegistrouDescarte(int peso) {
        Response r = enviar("operador01", "POST", "/api/descarte", payloadDescarte(pontoId, peso));
        assertEquals(201, r.statusCode(), "Falha ao registrar descarte: " + r.asString());
        descarteId = r.jsonPath().getString("descarte.descarteId");
    }

    // ============================================================== Quando

    @Quando("faço login com usuário {string} e senha {string}")
    public void login(String usuario, String senha) {
        response = given().baseUri(BASE_URL).contentType(ContentType.JSON)
                .body(Map.of("username", usuario, "password", senha))
                .post("/api/auth/login");
    }

    @Quando("envio uma requisição GET para {string}")
    public void get(String caminho) {
        response = comToken().get(caminho);
    }

    @Quando("envio uma requisição DELETE para {string}")
    public void delete(String caminho) {
        response = comToken().delete(caminho);
    }

    @Quando("envio uma requisição PUT para {string}")
    public void put(String caminho) {
        response = comToken().put(caminho);
    }

    @Quando("cadastro um novo ponto de coleta com capacidade de {int} kg")
    public void cadastrarPonto(int capacidade) {
        response = comToken().contentType(ContentType.JSON).body(payloadPonto(capacidade))
                .post("/api/pontocoleta");
        if (response.statusCode() == 201) {
            pontoId = response.jsonPath().getString("pontoColetaId");
        }
    }

    @Quando("cadastro um ponto de coleta com dados inválidos")
    public void cadastrarPontoInvalido() {
        response = comToken().contentType(ContentType.JSON).body(ler("payloads/ponto-coleta-invalido.json"))
                .post("/api/pontocoleta");
    }

    @Quando("consulto o ponto de coleta criado")
    public void consultarPonto() {
        response = comToken().get("/api/pontocoleta/" + pontoId);
    }

    @Quando("atualizo o ponto de coleta criado com capacidade de {int} kg")
    public void atualizarPonto(int capacidade) {
        response = comToken().contentType(ContentType.JSON).body(payloadPonto(capacidade))
                .put("/api/pontocoleta/" + pontoId);
    }

    @Quando("excluo o ponto de coleta criado")
    public void excluirPonto() {
        response = comToken().delete("/api/pontocoleta/" + pontoId);
    }

    @Quando("o operador registra um descarte de {int} kg nesse ponto")
    public void operadorRegistraNoPonto(int peso) {
        registrarComo("operador01", pontoId, peso);
    }

    @Quando("o operador registra um descarte de {int} kg no ponto {int}")
    public void operadorRegistraNoPontoId(int peso, int id) {
        registrarComo("operador01", String.valueOf(id), peso);
    }

    @Quando("registro um descarte de {int} kg no ponto {int}")
    public void registroComTokenAtual(int peso, int id) {
        response = comToken().contentType(ContentType.JSON).body(payloadDescarte(String.valueOf(id), peso))
                .post("/api/descarte");
    }

    @Quando("consulto o descarte registrado")
    public void consultarDescarte() {
        response = comToken().get("/api/descarte/" + descarteId);
    }

    @Quando("resolvo o alerta desse ponto")
    public void resolverAlerta() {
        response = comToken().put("/api/alerta/" + localizarAlertaId() + "/resolver");
    }

    @Quando("consulto o alerta desse ponto")
    public void consultarAlerta() {
        response = comToken().get("/api/alerta/" + localizarAlertaId());
    }

    // =============================================================== Então

    @Entao("o status code deve ser {int}")
    public void statusCode(int esperado) {
        assertEquals(esperado, response.statusCode(), "Corpo da resposta: " + response.asString());
    }

    @Entao("o corpo deve respeitar o contrato {string}")
    public void contrato(String arquivoSchema) {
        response.then().body(matchesJsonSchemaInClasspath("schemas/" + arquivoSchema));
    }

    @Entao("o campo {string} deve ser igual a {string}")
    public void campoIgual(String campo, String esperado) {
        Object valor = response.jsonPath().get(campo);
        assertNotNull(valor, "Campo ausente na resposta: " + campo);
        assertTrue(iguais(String.valueOf(valor), esperado),
                "Campo " + campo + ": esperado " + esperado + " mas veio " + valor);
    }

    @Entao("o alerta desse ponto deve estar aberto na lista de alertas")
    public void alertaAbertoNaLista() {
        Response r = given().baseUri(BASE_URL).get("/api/alerta?resolvido=false&pagina=1&tamanho=200");
        assertEquals(200, r.statusCode());
        JsonNode item = procurarPorPonto(lerJson(r), "pontoColetaId");
        assertNotNull(item, "Nenhum alerta aberto encontrado para o ponto " + pontoId);
        assertEquals(false, item.get("resolvido").asBoolean());
    }

    @Entao("o ponto de coleta criado deve constar no relatório com {int} kg de carga")
    public void pontoNoRelatorio(int kg) {
        JsonNode item = procurarPorPonto(lerJson(response).get("porPontoColeta"), "pontoColetaId");
        assertNotNull(item, "Ponto " + pontoId + " não consta em porPontoColeta");
        assertTrue(iguais(item.get("cargaAtualKg").asText(), String.valueOf(kg)),
                "cargaAtualKg esperado " + kg + " mas veio " + item.get("cargaAtualKg"));
    }

    // ============================================================== Helpers

    private void registrarComo(String usuario, String idPonto, int peso) {
        response = enviar(usuario, "POST", "/api/descarte", payloadDescarte(idPonto, peso));
        if (response.statusCode() == 201) {
            descarteId = response.jsonPath().getString("descarte.descarteId");
        }
    }

    private String localizarAlertaId() {
        if (alertaId == null) {
            Response r = given().baseUri(BASE_URL).get("/api/alerta?pagina=1&tamanho=200");
            assertEquals(200, r.statusCode());
            JsonNode item = procurarPorPonto(lerJson(r), "pontoColetaId");
            assertNotNull(item, "Nenhum alerta encontrado para o ponto " + pontoId);
            alertaId = item.get("alertaId").asText();
        }
        return alertaId;
    }

    private JsonNode procurarPorPonto(JsonNode lista, String campo) {
        for (JsonNode n : lista) {
            if (pontoId.equals(n.get(campo).asText())) {
                return n;
            }
        }
        return null;
    }

    private RequestSpecification comToken() {
        RequestSpecification spec = given().baseUri(BASE_URL);
        return token == null ? spec : spec.auth().oauth2(token);
    }

    private Response enviar(String usuario, String metodo, String caminho, String corpo) {
        return given().baseUri(BASE_URL).auth().oauth2(tokenDe(usuario))
                .contentType(ContentType.JSON).body(corpo).request(metodo, caminho);
    }

    private String tokenDe(String usuario) {
        return TOKENS.computeIfAbsent(usuario, u -> {
            Response r = given().baseUri(BASE_URL).contentType(ContentType.JSON)
                    .body(Map.of("username", u, "password", SENHA_PADRAO))
                    .post("/api/auth/login");
            assertEquals(200, r.statusCode(), "Login de " + u + " falhou: " + r.asString());
            return r.jsonPath().getString("token");
        });
    }

    private String payloadPonto(int capacidade) {
        Map<String, String> v = new HashMap<>();
        v.put("nome", "Ecoponto BDD " + System.nanoTime());
        v.put("capacidade", String.valueOf(capacidade));
        return modelo("payloads/ponto-coleta.json", v);
    }

    private String payloadDescarte(String idPonto, int peso) {
        Map<String, String> v = new HashMap<>();
        v.put("pontoId", idPonto);
        v.put("peso", String.valueOf(peso));
        return modelo("payloads/descarte.json", v);
    }

    private String modelo(String recurso, Map<String, String> variaveis) {
        String texto = ler(recurso);
        for (var e : variaveis.entrySet()) {
            texto = texto.replace("${" + e.getKey() + "}", e.getValue());
        }
        return texto;
    }

    private String ler(String recurso) {
        try (var in = getClass().getClassLoader().getResourceAsStream(recurso)) {
            assertNotNull(in, "Arquivo não encontrado: " + recurso);
            return new String(in.readAllBytes(), StandardCharsets.UTF_8);
        } catch (IOException e) {
            throw new IllegalStateException(e);
        }
    }

    private JsonNode lerJson(Response r) {
        try {
            return JSON.readTree(r.asString());
        } catch (IOException e) {
            throw new IllegalStateException("Resposta não é JSON válido: " + r.asString(), e);
        }
    }

    private static boolean iguais(String atual, String esperado) {
        try {
            return new BigDecimal(atual).compareTo(new BigDecimal(esperado)) == 0;
        } catch (NumberFormatException e) {
            return atual.equalsIgnoreCase(esperado);
        }
    }

    private static String resolverBaseUrl() {
        String prop = System.getProperty("baseUrl");
        if (prop != null && !prop.isBlank() && !prop.startsWith("${")) {
            return prop;
        }
        String env = System.getenv("BASE_URL");
        return env != null && !env.isBlank() ? env : "http://localhost:8080";
    }
}
