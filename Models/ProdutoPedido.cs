using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppCurso.Models;

public class ProdutoPedido
{
    [DisplayName("Código")]
    [Required(ErrorMessage = "Campo obrigatório")]
    public int Id { get; set; }

    [DisplayName("Descrição")]
    [Required(ErrorMessage = "Campo obrigatório")]
    public string Descricao { get; set; } = "";

    [DisplayName("Preço")]
    [Required(ErrorMessage = "Campo obrigatório")]
    [PrecoValido(ErrorMessage = "Formato inválido. Use um número com até duas casas decimais.")]
    public decimal Preco { get; set; }

    [DisplayName("Quantidade")]
    [Required(ErrorMessage = "Campo obrigatório")]
    public int Quantidade { get; set; }

    [DisplayName("Preço")]
    [Required(ErrorMessage = "Campo obrigatório")]
    [PrecoValido(ErrorMessage = "Formato inválido. Use um número com até duas casas decimais.")]
    public decimal Total { get; set; }

    [DisplayName("Pedido")]
    [Required(ErrorMessage = "Campo obrigatório")]
    public int PedidoId { get; set; }
    public Pedido Pedido { get; set; } = new();
}
