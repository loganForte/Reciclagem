# Executa a suíte BDD (Java 17 + Maven) contra uma API já em execução.
FROM maven:3.9-eclipse-temurin-17
WORKDIR /tests
COPY pom.xml .
RUN mvn -B -q dependency:go-offline
COPY src ./src
ENV BASE_URL=http://host.docker.internal:8080
CMD ["mvn", "-B", "test"]
