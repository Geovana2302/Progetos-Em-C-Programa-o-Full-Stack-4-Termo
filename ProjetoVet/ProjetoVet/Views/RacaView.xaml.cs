using ProjetoVet.Models;
using ProjetoVet.Repositories;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace ProjetoVet.Views
{
    public partial class RacaView : UserControl
    {
        private readonly RacaRepository _repository;
        private Raca racaSelecionada;

        public RacaView()
        {
            InitializeComponent();
            _repository = new RacaRepository();
            this.Loaded += RacaView_Loaded;
        }

        private void RacaView_Loaded(object sender, RoutedEventArgs e)
        {
            CarregarDataGrid();
            CarregarComboBox();
        }

        private void CarregarDataGrid(string pesquisa = "")
        {
            dataGridRacas.ItemsSource = _repository.GetAll(pesquisa);
        }

        private void CarregarComboBox()
        {
            // Busca as espécies do próprio repositório (não precisa do EspecieRepository)
            cmbEspecie.ItemsSource = _repository.GetEspecies();
            cmbEspecie.DisplayMemberPath = "Nome";
            cmbEspecie.SelectedValuePath = "EspecieID";
        }

        private void LimparCampos()
        {
            txtNome.Text = "";
            cmbEspecie.SelectedIndex = -1;
            racaSelecionada = null;
            dataGridRacas.SelectedItem = null;
        }

        private void btnLimpar_Click(object sender, RoutedEventArgs e)
        {
            LimparCampos();
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text) || cmbEspecie.SelectedValue == null)
            {
                MessageBox.Show("Os campos 'Nome' e 'Espécie' são obrigatórios.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Se racaSelecionada é nula, é um NOVO cadastro
                if (racaSelecionada == null)
                {
                    Raca novaRaca = new Raca
                    {
                        Nome = txtNome.Text,
                        EspecieID = (int)cmbEspecie.SelectedValue
                    };
                    _repository.Add(novaRaca);
                    MessageBox.Show("Raça salva com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                // Se NÃO é nula, é uma EDIÇÃO
                else
                {
                    racaSelecionada.Nome = txtNome.Text;
                    racaSelecionada.EspecieID = (int)cmbEspecie.SelectedValue;
                    _repository.Update(racaSelecionada);
                    MessageBox.Show("Raça atualizada com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
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
            if ((sender as FrameworkElement).DataContext is Raca raca)
            {
                racaSelecionada = raca;
                txtNome.Text = raca.Nome;
                cmbEspecie.SelectedValue = raca.EspecieID; // Seleciona a espécie correta
            }
        }

        private void ButtonExcluir_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement).DataContext is Raca raca)
            {
                MessageBoxResult result = MessageBox.Show($"Tem certeza que deseja excluir a raça '{raca.Nome}'?",
                                                           "Confirmação de Exclusão",
                                                           MessageBoxButton.YesNo,
                                                           MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _repository.Delete(raca.RacaID);
                        MessageBox.Show("Raça excluída com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
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