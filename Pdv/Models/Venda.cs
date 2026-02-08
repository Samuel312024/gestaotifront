namespace Integracoes.Pdv.Models;

public class Venda
{
    public int Id { get; set; }
    public DateTime Data { get; set; }
    public decimal Total { get; set; }
    public string FormaPagamento { get; set; } = string.Empty;

    public List<VendaItem> Itens { get; set; } = new();
    public DateTime DataVenda { get; set; }
}
