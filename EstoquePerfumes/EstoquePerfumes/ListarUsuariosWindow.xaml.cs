// ListarUsuariosWindow.xaml.cs
using EstoquePerfumes.Data;
using System.Linq;
using System.Windows;
namespace EstoquePerfumes
{
    public partial class ListarUsuariosWindow : Window
    {
        public ListarUsuariosWindow()
        {
            InitializeComponent();
            CarregarUsuarios(); // Chama o método para buscar os dados assim que a janela é criada
        }
        private void CarregarUsuarios()
        {
            using (var dbContext = new AppDbContext())
            {
                // Busca todos os usuários do banco e converte para uma lista
                var usuarios = dbContext.Usuarios.ToList();

                // Define a lista de usuários como a fonte de dados do nosso DataGrid
                dataGridUsuarios.ItemsSource = usuarios;
            }
        }
        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            // Simplesmente fecha a janela atual, voltando para a MainWindow que está atrás.
            this.Close();
        }
    }
}
