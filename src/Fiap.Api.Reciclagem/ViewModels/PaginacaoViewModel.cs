namespace Fiap.Api.Reciclagem.ViewModels
{
    public class PaginacaoViewModel<T>
    {
        public IEnumerable<T> Itens { get; set; } = new List<T>();
        public int Pagina { get; set; }
        public int Tamanho { get; set; }
        public int TotalItens { get; set; }
        public int TotalPaginas => Tamanho == 0 ? 0 : (int)Math.Ceiling((double)TotalItens / Tamanho);
    }
}
