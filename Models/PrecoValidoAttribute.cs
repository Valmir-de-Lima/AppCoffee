using System;
using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class PrecoValidoAttribute : ValidationAttribute
{
#nullable disable
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return ValidationResult.Success!; // Valor vazio é aceito ou pode ser tratado de outra forma
        }

        // Use a expressão regular para verificar se o formato é válido (números com até duas casas decimais)
        if (System.Text.RegularExpressions.Regex.IsMatch(value.ToString()!, @"^\d+(\.\d{1,2})?$"))
        {
            return ValidationResult.Success!;
        }

        return new ValidationResult("Formato inválido. Use um número com até duas casas decimais.");
    }
}
