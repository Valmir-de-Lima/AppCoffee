using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AppCurso.Models;

public class Usuario
{
    [DisplayName("Código")]
    [Required(ErrorMessage = "Campo obrigatório")]
    public int Id { get; set; }

    [DisplayName("E-mail")]
    [Required(ErrorMessage = "Campo obrigatório")]
    public string Email { get; set; } = "";

    [DisplayName("Senha")]
    [Required(ErrorMessage = "Campo obrigatório")]
    public string Senha { get; set; } = "";
}
