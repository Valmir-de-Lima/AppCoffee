using AppCurso.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCurso.Data.Mappings
{
    public class ProdutoPedidoMap : IEntityTypeConfiguration<ProdutoPedido>
    {
        public void Configure(EntityTypeBuilder<ProdutoPedido> builder)
        {
            // Tabela
            builder.ToTable("ProdutoPedido");

            // Chave Primária
            builder.HasKey(x => x.Id);

            // Identity
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.ProdutoId)
                 .IsRequired()  // NT NULL
                 .HasColumnName("Produto")
                 .HasColumnType("INTEGER");

            // Propriedades            
            builder.Property(x => x.Descricao)
                 .IsRequired()  // NT NULL
                 .HasColumnName("Descricao")
                 .HasColumnType("TEXT")
                 .HasMaxLength(80);

            builder.Property(x => x.Preco)
                 .IsRequired()  // NT NULL
                 .HasColumnName("Preco")
                 .HasColumnType("TEXT");

            builder.Property(x => x.Quantidade)
                 .IsRequired()  // NT NULL
                 .HasColumnName("Quantidade")
                 .HasColumnType("INTEGER");

            builder.Property(x => x.Total)
                 .IsRequired()  // NT NULL
                 .HasColumnName("Total")
                 .HasColumnType("TEXT");

        }
    }
}