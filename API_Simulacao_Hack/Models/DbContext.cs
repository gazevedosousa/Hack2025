using Microsoft.EntityFrameworkCore;

namespace API_Simulacao_Hack.Models
{

    public class DbHack : DbContextBase<DbHack>
    {
        public DbHack(DbContextOptions<DbHack> options) : base(options) { }
    }

    public abstract class DbContextBase<T> : DbContext
         where T : DbContext
    {
        public DbContextBase(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produto>(entity =>
            {
                entity.HasKey(e => e.CoProduto);
                entity.Property(e => e.CoProduto)
                    .IsRequired()
                    .HasColumnType("int")
                    .HasColumnName("CO_PRODUTO");
                entity.Property(e => e.NoProduto)
                    .IsRequired()
                    .HasColumnType("varchar(200)")
                    .HasColumnName("NO_PRODUTO");
                entity.Property(e => e.PcTaxaJuros)
                    .IsRequired()
                    .HasColumnType("decimal(10,9)")
                    .HasColumnName("PC_TAXA_JUROS");
                entity.Property(e => e.NuMinimoMeses)
                    .IsRequired()
                    .HasColumnType("smallint")
                    .HasColumnName("NU_MINIMO_MESES");
                entity.Property(e => e.NuMaximoMeses)
                    .IsRequired(false)
                    .HasColumnType("smallint")
                    .HasColumnName("NU_MAXIMO_MESES");
                entity.Property(e => e.VrMinimo)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)")
                    .HasColumnName("VR_MINIMO");
                entity.Property(e => e.VrMaximo)
                    .IsRequired(false)
                    .HasColumnType("decimal(18,2)")
                    .HasColumnName("VR_MAXIMO");
            });
        }
    }
}
