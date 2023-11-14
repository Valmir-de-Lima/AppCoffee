using AppCurso.Data.Mappings;
using AppCurso.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppCurso.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Pedido>? Pedidos { get; set; }
    public DbSet<ProdutoPedido>? ProdutoPedidos { get; set; }
    public DbSet<Produto>? Produtos { get; set; }
    public DbSet<Pagamento>? Pagamentos { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ProdutoMap());
        modelBuilder.ApplyConfiguration(new PedidoMap());
        modelBuilder.ApplyConfiguration(new ProdutoPedidoMap());
        modelBuilder.ApplyConfiguration(new PagamentoMap());
    }

}
