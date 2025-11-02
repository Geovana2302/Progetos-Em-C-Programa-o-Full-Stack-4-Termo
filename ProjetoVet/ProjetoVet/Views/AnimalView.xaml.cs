using ProjetoVet.Models;
using ProjetoVet.Repositories;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic; // Para List
using System; // Para Exception

namespace ProjetoVet.Views
{
    public partial class AnimalView : UserControl
    {
        private readonly AnimalRepository _repository;
        private Animal animalSelecionado;
        private List<Animal> _listaCompletaAnimais; // Cache para a pesquisa

        public AnimalView()
        {
            InitializeComponent();
            _repository = new AnimalRepository();
            this.Loaded += AnimalView_Loaded;
        }

        private void AnimalView_Loaded(object sender, RoutedEventArgs e)
        {
            CarregarComboBoxes();
            CarregarDataGrid();
        }

        /// <summary>
        /// Carrega (ou atualiza) os dados no DataGrid
        /// </summary>
        private void CarregarDataGrid(string pesquisa = "")
        {
            try
            {
                // Se a lista de cache estiver vazia (primeira carga), busca do banco
                if (_listaCompletaAnimais == null)
                {
                    // CORREÇÃO 1: Chamando 'Consultar' (Português)
                    _listaCompletaAnimais = _repository.Consultar();
                }

                // Filtra a lista em memória
                if (string.IsNullOrWhiteSpace(pesquisa))
                {
                    dataGridAnimais.ItemsSource = _listaCompletaAnimais;
                }
                else
                {
                    // Pesquisa por Nome do Animal ou Nome do Tutor
                    string lowerPesquisa = pesquisa.ToLower();
                    dataGridAnimais.ItemsSource = _listaCompletaAnimais
                        .Where(a => a.Nome.ToLower().Contains(lowerPesquisa) ||
                                    (a.Tutor != null && a.Tutor.Nome.ToLower().Contains(lowerPesquisa)))
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar os animais: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Carrega os ComboBoxes de Tutor e Espécie
        /// </summary>
        private void CarregarComboBoxes()
        {
            // Estes nomes de métodos estão corretos pois são do AnimalRepository
            cmbTutor.ItemsSource = _repository.GetTutores();
            cmbTutor.DisplayMemberPath = "Nome";
            cmbTutor.SelectedValuePath = "PessoaID";

            cmbEspecie.ItemsSource = _repository.GetEspecies();
            cmbEspecie.DisplayMemberPath = "Nome";
            cmbEspecie.SelectedValuePath = "EspecieID";
        }

        /// <summary>
        /// Filtra o ComboBox de Raças quando a Espécie muda
        /// </summary>
        private void cmbEspecie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbEspecie.SelectedItem is Especie especieSelecionada)
            {
                cmbRaca.ItemsSource = _repository.GetRacasPorEspecie(especieSelecionada.EspecieID);
                cmbRaca.DisplayMemberPath = "Nome";
                cmbRaca.SelectedValuePath = "RacaID";
                cmbRaca.IsEnabled = true;
            }
            else
            {
                cmbRaca.ItemsSource = null;
                cmbRaca.IsEnabled = false;
            }
        }

        /// <summary>
        /// Limpa o formulário e fecha o Expander
        /// </summary>
        private void LimparCampos()
        {
            txtNome.Text = "";
            txtSexo.Text = "";
            dpDataNascimento.SelectedDate = null;
            cmbTutor.SelectedIndex = -1;
            cmbEspecie.SelectedIndex = -1;
            cmbRaca.SelectedIndex = -1;
            cmbRaca.ItemsSource = null;
            cmbRaca.IsEnabled = false;

            animalSelecionado = null;
            FormExpander.IsExpanded = false;
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimparCampos();
        }

        /// <summary>
        /// Salva (Novo ou Edição)
        /// </summary>
        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text) ||
                cmbTutor.SelectedValue == null ||
                cmbEspecie.SelectedValue == null ||
                cmbRaca.SelectedValue == null)
            {
                MessageBox.Show("Preencha todos os campos obrigatórios (Nome, Tutor, Espécie e Raça).", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Se animalSelecionado é nulo, é um NOVO cadastro
                if (animalSelecionado == null)
                {
                    Animal novoAnimal = new Animal
                    {
                        Nome = txtNome.Text,
                        DataNascimento = dpDataNascimento.SelectedDate,
                        Sexo = txtSexo.Text,
                        TutorID = (int)cmbTutor.SelectedValue,
                        EspecieID = (int)cmbEspecie.SelectedValue,
                        RacaID = (int)cmbRaca.SelectedValue
                    };

                    // CORREÇÃO 2: Chamando 'Adicionar' (Português)
                    _repository.Adicionar(novoAnimal);
                    _listaCompletaAnimais = null; // Força recarregar do banco
                    MessageBox.Show("Animal salvo com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                // Se NÃO é nulo, é uma EDIÇÃO
                else
                {
                    animalSelecionado.Nome = txtNome.Text;
                    animalSelecionado.DataNascimento = dpDataNascimento.SelectedDate;
                    animalSelecionado.Sexo = txtSexo.Text;
                    animalSelecionado.TutorID = (int)cmbTutor.SelectedValue;
                    animalSelecionado.EspecieID = (int)cmbEspecie.SelectedValue;
                    animalSelecionado.RacaID = (int)cmbRaca.SelectedValue;

                    // CORREÇÃO 3: Chamando 'Alterar' (Português)
                    _repository.Alterar(animalSelecionado);
                    _listaCompletaAnimais = null; // Força recarregar do banco
                    MessageBox.Show("Animal atualizado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Erro ao salvar: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                CarregarDataGrid(txtPesquisa.Text); // Recarrega o grid (com o filtro)
                LimparCampos();
            }
        }

        /// <summary>
        /// Chamado pelo botão 'Editar' (lápis) no DataGrid
        /// </summary>
        private void ButtonEditar_Click(object sender, RoutedEventArgs e)
        {
            // Pega o animal da linha do botão que foi clicado
            if ((sender as FrameworkElement).DataContext is Animal animal)
            {
                animalSelecionado = animal;

                // Preenche os campos do formulário
                txtNome.Text = animal.Nome;
                dpDataNascimento.SelectedDate = animal.DataNascimento;
                txtSexo.Text = animal.Sexo;
                cmbTutor.SelectedValue = animal.TutorID;
                cmbEspecie.SelectedValue = animal.EspecieID;

                // Carrega as raças da espécie ANTES de selecionar a raça
                cmbRaca.ItemsSource = _repository.GetRacasPorEspecie(animal.EspecieID);
                cmbRaca.IsEnabled = true;
                cmbRaca.SelectedValue = animal.RacaID;

                // Abre o expander
                FormExpander.IsExpanded = true;
            }
        }

        /// <summary>
        /// Chamado pelo botão 'Excluir' (lixeira) no DataGrid
        /// </summary>
        private void ButtonExcluir_Click(object sender, RoutedEventArgs e)
        {
            // Pega o animal da linha do botão que foi clicado
            if ((sender as FrameworkElement).DataContext is Animal animal)
            {
                MessageBoxResult result = MessageBox.Show($"Tem certeza que deseja excluir o animal '{animal.Nome}'?",
                                                        "Confirmação de Exclusão",
                                                        MessageBoxButton.YesNo,
                                                        MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // CORREÇÃO 4: Chamando 'Excluir(animal)' (Português)
                        _repository.Excluir(animal);
                        _listaCompletaAnimais = null; // Força recarregar do banco
                        MessageBox.Show("Animal excluído com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (System.Exception ex)
                    {
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

        /// <summary>
        /// Filtra o DataGrid em tempo real
        /// </summary>
        private void txtPesquisa_TextChanged(object sender, TextChangedEventArgs e)
        {
            CarregarDataGrid(txtPesquisa.Text);
        }
    }
}