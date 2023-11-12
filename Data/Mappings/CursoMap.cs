using AppCurso.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCurso.Data.Mappings
{
    public class CursoMap : IEntityTypeConfiguration<Curso>
    {
        public void Configure(EntityTypeBuilder<Curso> builder)
        {
            // Tabela
            builder.ToTable("Curso");

            // Chave Primária
            builder.HasKey(x => x.Id);

            // Identity
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            // Propriedades            
            builder.Property(x => x.Titulo)
                 .IsRequired()  // NT NULL
                 .HasColumnName("Titulo")
                 .HasColumnType("TEXT")
                 .HasMaxLength(80);

            builder.Property(x => x.Sumario)
                 .IsRequired()  // NT NULL
                 .HasColumnName("Sumario")
                 .HasColumnType("TEXT")
                 .HasMaxLength(80);

            builder.Property(x => x.DuracaoEmMinutos)
                 .IsRequired()  // NT NULL
                 .HasColumnName("DuracaoEmMinutos")
                 .HasColumnType("INTEGER");

            builder.Property(x => x.DuracaoEmHoras)
                 .HasColumnName("DuracaoEmHoras")
                 .HasComputedColumnSql("[DuracaoEmMinutos] / 60");
        }
    }
}