using System.Windows;
// 1. ADICIONE O USING PARA SUA PASTA DE VIEWS
using ProjetoVet.Views; // <-- Verifique se o namespace está correto

namespace ProjetoVet
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // --- SEUS MÉTODOS EXISTENTES (PROVAVELMENTE SÃO ASSIM) ---

        private void ButtonTutores_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new TutorView());
        }

        private void ButtonAnimais_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new AnimalView());
        }

        private void ButtonVeterinarios_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new VeterinarioView());
        }

        private void ButtonFuncionarios_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new FuncionarioView()); 
        }


        // --- 2. ADICIONE OS NOVOS MÉTODOS DE CLIQUE AQUI ---

        private void ButtonEspecies_Click(object sender, RoutedEventArgs e)
        {
            // Carrega a tela de Espécies dentro do Frame
            MainContentFrame.Navigate(new EspecieView());
        }

        private void ButtonRacas_Click(object sender, RoutedEventArgs e)
        {
            // Carrega a tela de Raças dentro do Frame
            MainContentFrame.Navigate(new RacaView());
        }
    }
}