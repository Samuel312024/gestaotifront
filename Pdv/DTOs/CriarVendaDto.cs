namespace Integracoes.Pdv.DTOs;

public class CriarVendaDto
{
    public string FormaPagamento { get; set; } = string.Empty;
    public List<CriarVendaItemDto> Itens { get; set; } = new();
}
