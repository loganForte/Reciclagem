using Xunit;

// Os testes compartilham o mesmo arquivo SQLite (reciclagem.db); executa em sequencia
// para evitar disputa na criacao do schema.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
