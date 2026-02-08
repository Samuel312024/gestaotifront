namespace Pdv.Models;
public class HistoricoPreco
{
    public int Id { get; set; }

    public int ProdutoId { get; set; }
    //public Produto Produto { get; set; }

    public decimal PrecoAnterior { get; set; }
    public decimal PrecoNovo { get; set; }

    public DateTime AlteradoEm { get; set; } = DateTime.Now;
    public DateTime DataAlteracao { get; set; }
}
