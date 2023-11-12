using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppCurso.Models;

public class Pagamento
{
    [DisplayName("Código")]
    [Required(ErrorMessage = "Campo obrigatório")]
    public int Id { get; set; }

    [DisplayName("Cliente")]
    [Required(ErrorMessage = "Campo obrigatório")]
    public string Cliente { get; set; } = "";

    [DisplayName("Forma de Pagamento")]
    [Required(ErrorMessage = "Campo obrigatório")]
    public string FormaPagamento { get; set; } = "";

    [DisplayName("Total")]
    [Required(ErrorMessage = "Campo obrigatório")]
    [PrecoValido(ErrorMessage = "Formato inválido. Use um número com até duas casas decimais.")]
    public decimal Total { get; set; }

    [DisplayName("Pedido")]
    public int PedidoId { get; set; }
    public Pedido? Pedido { get; set; }
}
