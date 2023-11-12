using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppCurso.Models;

public class Pedido
{
    [DisplayName("Pedido")]
    [Required(ErrorMessage = "Campo obrigatório")]
    public int Id { get; set; }

    [DisplayName("Cliente")]
    [Required(ErrorMessage = "Campo obrigatório")]
    public string Cliente { get; set; } = "";

    [DisplayName("Status")]
    public string Status { get; set; } = "";

    [DisplayName("Avaliação")]
    public string Avaliacao { get; set; } = "";

    [DisplayName("Resposta")]
    public string Resposta { get; set; } = "";

    [DisplayName("Total")]
    [Required(ErrorMessage = "Selecione pelo menos um produto")]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Total { get; set; }

    [DisplayName("Recebido")]
    public bool Recebido { get; set; }

    public List<Produto> Produtos { get; set; } = new List<Produto>();

    public int PagamentoId { get; set; }
    public Pagamento? Pagamento { get; set; }
}
