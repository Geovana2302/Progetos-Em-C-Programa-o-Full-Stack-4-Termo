using EstoquePerfumes.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography; // Importante para o Hash
using System.Text; // Importante para o Hash

namespace EstoquePerfumes.Data
{
    public class AppDbContext : DbContext
    {
        // Define as tabelas que teremos no banco de dados
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Perfume> Perfumes { get; set; }

        // Configura a conexão com o banco de dados
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // O nome do banco será EstoquePerfumesDB.db
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EstoquePerfumesDB;Integrated Security=True;");
        }

        // Este método é chamado quando o modelo do banco está sendo criado
        // É o lugar perfeito para inserir dados iniciais (seeding)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Criar o Hash da senha "123"
            var sha = SHA256.Create();
            var asByteArray = Encoding.Default.GetBytes("123");
            var hashedPassword = sha.ComputeHash(asByteArray);

            // Inserir o usuário admin inicial
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1, // Definimos o ID manualmente para o primeiro usuário
                    Nome = "Administrador",
                    Email = "admin@admin.com",
                    SenhaHash = Convert.ToBase64String(hashedPassword), // Guardamos o hash como string
                    NivelAcesso = "Admin"
                }
            );
        }
    }
}
