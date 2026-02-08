namespace Pdv.Models;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
    public string CodigoBarras { get; set; } = string.Empty;
    public bool Ativo { get; set; }
    public decimal PrecoAtual { get; set; }
}
