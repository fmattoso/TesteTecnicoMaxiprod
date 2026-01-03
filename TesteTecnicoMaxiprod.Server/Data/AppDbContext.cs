using Microsoft.EntityFrameworkCore;
using TesteTecnicoMaxiprod.WebApi.Models;

namespace TesteTecnicoMaxiprod.WebApi.Data
{
    /// <summary>
    /// Contexto do banco de dados
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options) 
        { 
        }

        /// <summary>
        /// Tabela de Pessoa
        /// </summary>
        public DbSet<Pessoa> Pessoas {get; set;}

        /// <summary>
        /// Tabela de Categorias
        /// </summary>
        public DbSet<Categoria> Categorias {get; set;}

        /// <summary>
        /// Tabela de Transacoes
        /// </summary>
        public DbSet<Transacao> Transacoes {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações para a tabela Pessoas
            modelBuilder.Entity<Pessoa>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Idade).IsRequired();

                // Delete Cascade
                entity.HasMany(e => e.Transacoes)
                .WithOne(e => e.Pessoa)
                .HasForeignKey(e => e.PessoaId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            // Configurações para a tabela Categoria
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Descricao).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Finalidade).IsRequired().HasConversion<string>();

                // Delete Restrict
                entity.HasMany(e => e.Transacoes)
                .WithOne(e => e.Categoria)
                .HasForeignKey(e => e.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            // Configurações para a tabela Transacao
            modelBuilder.Entity<Transacao>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Descricao).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Valor).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.Tipo).IsRequired().HasConversion<string>();
                entity.Property(e => e.Data).IsRequired();

                // Relaçao com Pessoa
                entity.HasOne(e => e.Pessoa)
                .WithMany(e => e.Transacoes)
                .HasForeignKey(e => e.PessoaId)
                .OnDelete(DeleteBehavior.Cascade);

                // Relaçao com Categoria
                entity.HasOne(e => e.Categoria)
                .WithMany(e => e.Transacoes)
                .HasForeignKey(e => e.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
            });


        }
    }
}
