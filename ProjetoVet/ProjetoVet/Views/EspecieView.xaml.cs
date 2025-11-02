using ProjetoVet.Models;
using ProjetoVet.Repositories;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace ProjetoVet.Views
{
    public partial class EspecieView : UserControl
    {
        private readonly EspecieRepository _repository;
        private Especie especieSelecionada;

        public EspecieView()
        {
            InitializeComponent();
            _repository = new EspecieRepository();
            this.Loaded += EspecieView_Loaded;
        }

        private void EspecieView_Loaded(object sender, RoutedEventArgs e)
        {
            CarregarDataGrid();
        }

        private void CarregarDataGrid(string pesquisa = "")
        {
            dataGridEspecies.ItemsSource = _repository.GetAll(pesquisa);
        }

        private void LimparCampos()
        {
            txtNome.Text = "";
            especieSelecionada = null;
            dataGridEspecies.SelectedItem = null;
        }

        private void btnLimpar_Click(object sender, RoutedEventArgs e)
        {
            LimparCampos();
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("O campo 'Nome' é obrigatório.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Se especieSelecionada é nula, é um NOVO cadastro
                if (especieSelecionada == null)
                {
                    Especie novaEspecie = new Especie
                    {
                        Nome = txtNome.Text
                    };
                    _repository.Add(novaEspecie);
                    MessageBox.Show("Espécie salva com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                // Se NÃO é nula, é uma EDIÇÃO
                else
                {
                    especieSelecionada.Nome = txtNome.Text;
                    _repository.Update(especieSelecionada);
                    MessageBox.Show("Espécie atualizada com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Erro ao salvar: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                CarregarDataGrid(txtPesquisa.Text);
                LimparCampos();
            }
        }

        private void ButtonEditar_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement).DataContext is Especie especie)
            {
                especieSelecionada = especie;
                txtNome.Text = especie.Nome;
            }
        }

        private void ButtonExcluir_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement).DataContext is Especie especie)
            {
                MessageBoxResult result = MessageBox.Show($"Tem certeza que deseja excluir a espécie '{especie.Nome}'?",
                                                           "Confirmação de Exclusão",
                                                           MessageBoxButton.YesNo,
                                                           MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _repository.Delete(especie.EspecieID);
                        MessageBox.Show("Espécie excluída com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (System.Exception ex)
                    {
                        // Captura a exceção que jogamos no repositório
                        MessageBox.Show($"Erro ao excluir: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        CarregarDataGrid(txtPesquisa.Text);
                        LimparCampos();
                    }
                }
            }
        }

        private void txtPesquisa_TextChanged(object sender, TextChangedEventArgs e)
        {
            CarregarDataGrid(txtPesquisa.Text);
        }
    }
}