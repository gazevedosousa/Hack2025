using Microsoft.EntityFrameworkCore;

namespace API_Simulacao_Hack.Models
{

    public class SimulacaoContext : DbContext
    {
        public SimulacaoContext(DbContextOptions<SimulacaoContext> options) : base(options) { }

        public DbSet<Simulacao> Simulacoes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlite("Data Source=simulacao.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Simulacao>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasColumnType("integer")
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.IdSimulacao)
                    .IsRequired()
                    .HasColumnType("integer")
                    .HasColumnName("idSimulacao");
                entity.Property(e => e.ValorDesejado)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)")
                    .HasColumnName("valorDesejado");
                entity.Property(e => e.Prazo)
                    .IsRequired()
                    .HasColumnType("int")
                    .HasColumnName("prazo");
                entity.Property(e => e.ValorTotalParcelas)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)")
                    .HasColumnName("valorTotalParcelas");
                entity.Property(e => e.CodigoProduto)
                    .IsRequired()
                    .HasColumnType("int")
                    .HasColumnName("codigoProduto");
                entity.Property(e => e.DescricaoProduto)
                    .IsRequired()
                    .HasColumnType("varchar(200)")
                    .HasColumnName("descricaoProduto");
                entity.Property(e => e.DataReferencia)
                    .HasColumnName("dataReferencia");
                entity.Property(e => e.TaxaJuros)
                    .IsRequired()
                    .HasColumnType("decimal(10,9)")
                    .HasColumnName("taxaJuros");
                entity.Property(e => e.ValorMediaPrestacoes)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)")
                    .HasColumnName("valorMediaPrestacoes");
                entity.Property(e => e.TipoSimulacao)
                    .IsRequired()
                    .HasColumnType("varchar(5)")
                    .HasColumnName("tipoSimulacao");
            });
        }
    }

}




