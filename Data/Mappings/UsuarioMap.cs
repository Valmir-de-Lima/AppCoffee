using AppCurso.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCurso.Data.Mappings
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            // Tabela
            builder.ToTable("Usuario");

            // Chave Primária
            builder.HasKey(x => x.Id);

            // Identity
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            // Propriedades            
            builder.Property(x => x.Email)
                 .IsRequired()  // NT NULL
                 .HasColumnName("Email")
                 .HasColumnType("TEXT")
                 .HasMaxLength(80);

            builder.Property(x => x.Senha)
                 .IsRequired()  // NT NULL
                 .HasColumnName("Senha")
                 .HasColumnType("TEXT")
                 .HasMaxLength(80);

            builder
                .HasIndex(x => x.Senha, "IX_Usuario_Senha");
        }
    }
}