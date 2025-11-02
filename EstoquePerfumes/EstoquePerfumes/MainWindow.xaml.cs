using EstoquePerfumes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace EstoquePerfumes
{
    public partial class MainWindow : Window
    {

        // Construtor que recebe o nome do usuário
        public MainWindow (String Nome)
        {
            InitializeComponent(); // Este método desenha a tela, sempre deve ser o primeiro

            // Atualiza o TextBlock da saudação com o nome do usuário que logou
            txtSaudacao.Text = $"Olá, {Nome}!";
        }

        

        private void btnLogoff_Click(object sender, RoutedEventArgs e)
        {
            // 1. Cria uma nova instância da tela de Login.
            //    É como se estivéssemos abrindo o programa pela primeira vez.
            var loginWindow = new LoginWindow();

            // 2. Mostra a nova tela de login.
            loginWindow.Show();

            // 3. Fecha a tela atual (a MainWindow).
            //    Isso "destrói" a sessão do usuário logado.
            this.Close();
        }

        private void btnListarUsuarios_Click(object sender, RoutedEventArgs e)
        {
            // Cria uma instância da nossa nova janela
            var janelaListarUsuarios = new ListarUsuariosWindow();

            // Mostra a janela. O código na MainWindow vai pausar aqui até a janela ser fechada se usarmos ShowDialog().
            // Usando Show() permite que ambas as janelas fiquem ativas. Para este caso, qualquer um serve.
            janelaListarUsuarios.Show();
        }

    }
}

