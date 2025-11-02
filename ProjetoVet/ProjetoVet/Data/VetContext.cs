using Microsoft.EntityFrameworkCore;
using ProjetoVet.Models;

namespace ProjetoVet.Data
{
    public class VetContext : DbContext
    {
        // 1. Liste todas as suas classes "base" e "derivadas"
        //    O EF Core vai entender a herança (TPH)
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Tutor> Tutores { get; set; }
        public DbSet<Veterinario> Veterinarios { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Animal> Animais { get; set; }
        public DbSet<Especie> Especies { get; set; }
        public DbSet<Raca> Racas { get; set; }

        // (Adicione os outros DbSets quando criar as classes)
        // public DbSet<Animal> Animais { get; set; }
        // public DbSet<Especie> Especies { get; set; }
        // public DbSet<Raca> Racas { get; set; }
        // public DbSet<Endereco> Enderecos { get; set; }

        // 2. Configure a Conexão com o Banco
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Altere esta string de conexão para o seu banco
                // Este exemplo usa o SQL Server LocalDB (padrão do Visual Studio)
                // O banco "VetDB" será criado automaticamente
                string connectionString = "Server=(localdb)\\mssqllocaldb;Database=VetDB;Trusted_Connection=True;";

                optionsBuilder.UseSqlServer(connectionString);

                // (Alternativa se estivesse usando SQLite)
                // optionsBuilder.UseSqlite("Data Source=vet.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Mantenha esta linha

            // Configuração Herança TPH (Já deve estar implícito, mas bom garantir)
            modelBuilder.Entity<Pessoa>()
                .HasDiscriminator<string>("TipoPessoa") // Cria a coluna "Discriminator"
                .HasValue<Funcionario>("Funcionario")
                .HasValue<Tutor>("Tutor")
                .HasValue<Veterinario>("Veterinario");

            // --- Relacionamentos do Tutor ---

            // 1 Tutor -> Muitos Animais
            modelBuilder.Entity<Tutor>()
                .HasMany(t => t.Animais) // (Precisa adicionar 'ICollection<Animal> Animais' no Tutor.cs)
                .WithOne(a => a.Tutor)
                .HasForeignKey(a => a.TutorID)
                .OnDelete(DeleteBehavior.Cascade); // Se deletar o Tutor, deleta os animais

            // 1 Tutor -> Muitos Endereços
            modelBuilder.Entity<Tutor>()
                .HasMany(t => t.Enderecos) // (Precisa adicionar 'ICollection<Endereco> Enderecos' no Tutor.cs)
                .WithOne(e => e.Tutor)
                .HasForeignKey(e => e.TutorID)
                .OnDelete(DeleteBehavior.Cascade); // Se deletar o Tutor, deleta os endereços

            // --- Relacionamentos de Animal/Especie/Raca ---

            // 1 Espécie -> Muitas Raças
            modelBuilder.Entity<Especie>()
                .HasMany(e => e.Racas)
                .WithOne(r => r.Especie)
                .HasForeignKey(r => r.EspecieID);

            // 1 Raça -> Muitos Animais
            modelBuilder.Entity<Raca>()
                .HasMany(r => r.Animais)
                .WithOne(a => a.Raca)
                .HasForeignKey(a => a.RacaID)
                .OnDelete(DeleteBehavior.Restrict); // Não deixa deletar a Raça se tiver animal cadastrado

            // 1 Espécie -> Muitos Animais
            modelBuilder.Entity<Especie>()
                .HasMany(e => e.Animais)
                .WithOne(a => a.Especie)
                .HasForeignKey(a => a.EspecieID)
                .OnDelete(DeleteBehavior.Restrict); // Não deixa deletar a Espécie se tiver animal cadastrado
        }
    }
}