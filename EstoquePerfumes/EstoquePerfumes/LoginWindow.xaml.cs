// LoginWindow.xaml.cs
using EstoquePerfumes.Data;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Linq; // Importante para usar o .FirstOrDefault()

namespace EstoquePerfumes
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 1. Obter os dados da tela
            string email = txtEmail.Text;
            string senha = txtSenha.Password;

            // 2. Validação básica
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
            {
                MessageBox.Show("Por favor, preencha o email e a senha.");
                return;
            }

            // 3. Hashear a senha digitada para comparar com a do banco
            var sha = SHA256.Create();
            var asByteArray = Encoding.Default.GetBytes(senha);
            var hashedPassword = sha.ComputeHash(asByteArray);
            var senhaHashBase64 = Convert.ToBase64String(hashedPassword);

            // 4. Consultar o banco de dados
            using (var dbContext = new AppDbContext())
            {
                // Procuramos um usuário que tenha o mesmo email E a mesma senha hasheada
                var usuarioEncontrado = dbContext.Usuarios.FirstOrDefault(u =>
                    u.Email == email && u.SenhaHash == senhaHashBase64);

                // 5. Verificar o resultado
                if (usuarioEncontrado != null)
                {
                    // SUCESSO!
                    // Abre a tela principal, passando o nome do usuário
                    var mainWindow = new MainWindow(usuarioEncontrado.Nome);
                    mainWindow.Show();

                    // Fecha a tela de login
                    this.Close();
                }
                else
                {
                    // FALHA!
                    MessageBox.Show("Email ou senha inválidos.");
                }
            }
        }
    }
}
