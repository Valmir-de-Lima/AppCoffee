using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AppCurso.Models;

public class Curso
{
    [DisplayName("Código")]
    [Required(ErrorMessage = "Campo obrigatório")]
    public int Id { get; set; }

    [DisplayName("Curso")]
    [Required(ErrorMessage = "Campo obrigatório")]
    public string Titulo { get; set; } = "";

    [Required(ErrorMessage = "Campo obrigatório")]
    public string Tag { get; set; } = "";

    [DisplayName("Sumário")]
    [Required(ErrorMessage = "Campo obrigatório")]
    public string Sumario { get; set; } = "";

    [DisplayName("Duração em minutos")]
    [Required(ErrorMessage = "Campo obrigatório")]
    public int DuracaoEmMinutos { get; set; }

    [DisplayName("horas")]
    public int DuracaoEmHoras { get; set; }

    //public IList<Modulo>? Modulos { get; set; }
}
