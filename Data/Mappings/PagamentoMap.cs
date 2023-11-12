using AppCurso.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCurso.Data.Mappings
{
    public class PagamentoMap : IEntityTypeConfiguration<Pagamento>
    {
        public void Configure(EntityTypeBuilder<Pagamento> builder)
        {
            // Tabela
            builder.ToTable("Pagamento");

            // Chave Primária
            builder.HasKey(x => x.Id);

            // Identity
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            // Propriedades            
            builder.Property(x => x.Cliente)
                 .IsRequired()  // NT NULL
                 .HasColumnName("Cliente")
                 .HasColumnType("TEXT")
                 .HasMaxLength(80);

            // Propriedades            
            builder.Property(x => x.FormaPagamento)
                 .IsRequired()  // NT NULL
                 .HasColumnName("FormaPagamento")
                 .HasColumnType("TEXT")
                 .HasMaxLength(80);

            builder.Property(x => x.Total)
                 .IsRequired()  // NT NULL
                .HasColumnName("Total")
                .HasColumnType("TEXT");

            builder.HasOne(x => x.Pedido)
                .WithOne(p => p.Pagamento) // Um para um
                .HasForeignKey<Pagamento>(x => x.PedidoId);

            // Indice auxiliar
            builder
                .HasIndex(x => x.Cliente, "IX_Pedido_Cliente");
        }
    }
}