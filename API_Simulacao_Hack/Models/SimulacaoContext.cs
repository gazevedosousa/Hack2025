using Microsoft.EntityFrameworkCore;

namespace API_Simulacao_Hack.Models
{

    public class SimulacaoContext : DbContext
    {
        public SimulacaoContext(DbContextOptions<SimulacaoContext> options) : base(options) { }

        public DbSet<Simulacao> Simulacoes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=simulacao.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Simulacao>(entity =>
            {
                entity.HasKey(e => e.IdSimulacao);
                entity.Property(e => e.IdSimulacao)
                    .IsRequired()
                    .HasColumnType("integer")
                    .HasColumnName("idSimulacao")
                    .ValueGeneratedOnAdd();
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
            });
        }
    }

}




