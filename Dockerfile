# syntax=docker/dockerfile:1

# ---------- Stage 1: build (SDK .NET 8) ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# restore em camada separada para aproveitar o cache
COPY src/Fiap.Api.Reciclagem/Fiap.Api.Reciclagem.csproj Fiap.Api.Reciclagem/
RUN dotnet restore Fiap.Api.Reciclagem/Fiap.Api.Reciclagem.csproj
COPY src/Fiap.Api.Reciclagem/ Fiap.Api.Reciclagem/
RUN dotnet publish Fiap.Api.Reciclagem/Fiap.Api.Reciclagem.csproj \
    -c Release -o /app/publish --no-restore /p:UseAppHost=false

# ---------- Stage 2: runtime (ASP.NET 8, imagem leve) ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
# diretorio do SQLite (montado como volume) com permissao para o usuario nao-root
RUN mkdir /data && chown $APP_UID /data
USER $APP_UID
ENV ASPNETCORE_URLS=http://+:8080 \
    ConnectionStrings__DefaultConnection="Data Source=/data/reciclagem.db"
EXPOSE 8080
# a imagem aspnet nao tem curl/wget: healthcheck via bash (/dev/tcp)
HEALTHCHECK --interval=15s --timeout=5s --start-period=20s --retries=5 \
  CMD bash -c 'exec 3<>/dev/tcp/127.0.0.1/8080 && printf "GET /health HTTP/1.0\r\nHost: localhost\r\n\r\n" >&3 && head -n1 <&3 | grep -q " 200 "' || exit 1
ENTRYPOINT ["dotnet", "Fiap.Api.Reciclagem.dll"]
