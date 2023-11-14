using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AppCurso.ViewModels
{
    public class UsuarioViewModel
    {
        [DisplayName("Código")]
        public int Id { get; set; }

        [DisplayName("E-mail")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; } = "";

        [DisplayName("Senha")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(100, ErrorMessage = "A senha deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Senha { get; set; } = "";

        [DisplayName("Confirmar Senha")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Compare("Senha", ErrorMessage = "As senhas não coincidem")]
        public string ConfirmacaoSenha { get; set; } = "";
    }
}
