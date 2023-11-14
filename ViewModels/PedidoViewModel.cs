using System.Collections.Generic;

namespace AppCurso.ViewModels
{
    public class PedidoViewModel
    {
        public string Cliente { get; set; } = "";
        public decimal TotalPedido { get; set; }
        public List<ProdutoViewModel> Produtos { get; set; } = new();
    }
}
