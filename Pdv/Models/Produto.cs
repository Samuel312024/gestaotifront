namespace Pdv.Models;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
    public int EstoqueMinimo { get; set; } = 5;
    public string CodigoBarras { get; set; } = string.Empty;
    public bool Ativo { get; set; }
    public decimal PrecoAtual { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.Now;
    public ICollection<HistoricoPreco> HistoricoPrecos { get; set; }
}
