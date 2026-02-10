namespace Pdv.DTOs;
public class CriarProdutoDto
{
    public string Nome { get; set; }
    public string Codigo { get; set; } // barcode ou QR
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
}

