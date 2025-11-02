using System;
using System.Windows;
using System.Windows.Controls;
using ProjetoVet.Models;
using ProjetoVet.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ProjetoVet.Views
{
    public partial class FuncionarioView : UserControl
    {
        private FuncionarioRepository funcionarioRepository;
        private List<Funcionario> _listaCompletaFuncionarios;
        private Funcionario _funcionarioEmEdicao = null;

        public FuncionarioView()
        {
            InitializeComponent();
            funcionarioRepository = new FuncionarioRepository();
            LoadGrid();
        }

        // REMOVIDO: O método 'BtnToggleForm_Click' não é mais necessário,
        // pois o Expander cuida disso sozinho.

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtNome.Text) ||
                    string.IsNullOrWhiteSpace(TxtCpf.Text) ||
                    string.IsNullOrWhiteSpace(TxtCargo.Text) ||
                    !DpDataNasc.SelectedDate.HasValue)
                {
                    MessageBox.Show("Nome, CPF, Cargo e Data de Nascimento são obrigatórios.", "Erro de Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!decimal.TryParse(TxtSalario.Text, out decimal salario))
                {
                    salario = 0;
                }

                if (_funcionarioEmEdicao == null)
                {
                    Funcionario funcionario = new Funcionario();

                    funcionario.Nome = TxtNome.Text;
                    funcionario.Telefone = TxtTelefone.Text;
                    funcionario.Email = TxtEmail.Text;
                    funcionario.DataNascimento = DpDataNasc.SelectedDate.Value;
                    funcionario.CPF = TxtCpf.Text;
                    funcionario.Cargo = TxtCargo.Text;
                    funcionario.DataAdmissao = DpDataAdmissao.SelectedDate ?? DateTime.Now;
                    funcionario.Salario = salario;

                    funcionarioRepository.Adicionar(funcionario);
                    MessageBox.Show("Funcionário salvo com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _funcionarioEmEdicao.Nome = TxtNome.Text;
                    _funcionarioEmEdicao.Telefone = TxtTelefone.Text;
                    _funcionarioEmEdicao.Email = TxtEmail.Text;
                    _funcionarioEmEdicao.DataNascimento = DpDataNasc.SelectedDate.Value;
                    _funcionarioEmEdicao.CPF = TxtCpf.Text;
                    _funcionarioEmEdicao.Cargo = TxtCargo.Text;
                    _funcionarioEmEdicao.DataAdmissao = DpDataAdmissao.SelectedDate ?? DateTime.Now;
                    _funcionarioEmEdicao.Salario = salario;

                    funcionarioRepository.Alterar(_funcionarioEmEdicao);
                    MessageBox.Show("Funcionário atualizado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                LoadGrid();
                ClearForm();

                // ATUALIZADO: Fecha o Expander
                FormExpander.IsExpanded = false;
            }
            catch (Exception ex)
            {
                Exception inner = ex;
                while (inner.InnerException != null) { inner = inner.InnerException; }
                MessageBox.Show($"Ocorreu um erro ao salvar: {inner.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadGrid()
        {
            try
            {
                _listaCompletaFuncionarios = funcionarioRepository.Consultar();
                AplicarFiltro();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar os funcionários: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearForm()
        {
            TxtNome.Text = "";
            TxtTelefone.Text = "";
            TxtCpf.Text = "";
            TxtEmail.Text = "";
            DpDataNasc.SelectedDate = null;
            TxtCargo.Text = "";
            TxtSalario.Text = "";
            DpDataAdmissao.SelectedDate = null;

            _funcionarioEmEdicao = null;
            BtnSalvar.Content = "Salvar";

            // ATUALIZADO: Reseta o Header do Expander
            FormExpander.Header = "[+] Adicionar Novo Funcionário";
        }

        private void AplicarFiltro()
        {
            var funcionariosFiltrados = _listaCompletaFuncionarios;

            if (TxtPesquisar != null && !string.IsNullOrWhiteSpace(TxtPesquisar.Text))
            {
                string filtro = TxtPesquisar.Text.ToLower();
                funcionariosFiltrados = _listaCompletaFuncionarios.Where(
                    f => f.Nome.ToLower().Contains(filtro) ||
                         f.CPF.Contains(filtro)
                ).ToList();
            }
            DataGridFuncionarios.ItemsSource = funcionariosFiltrados;
        }

        private void TxtPesquisar_TextChanged(object sender, TextChangedEventArgs e)
        {
            AplicarFiltro();
        }

        private void ButtonEditar_Click(object sender, RoutedEventArgs e)
        {
            Funcionario funcionarioParaEditar = (Funcionario)((Button)sender).DataContext;
            _funcionarioEmEdicao = funcionarioParaEditar;

            TxtNome.Text = funcionarioParaEditar.Nome;
            TxtCpf.Text = funcionarioParaEditar.CPF;
            TxtEmail.Text = funcionarioParaEditar.Email;
            TxtTelefone.Text = funcionarioParaEditar.Telefone;
            DpDataNasc.SelectedDate = funcionarioParaEditar.DataNascimento;
            TxtCargo.Text = funcionarioParaEditar.Cargo;
            TxtSalario.Text = funcionarioParaEditar.Salario.ToString("F2");
            DpDataAdmissao.SelectedDate = funcionarioParaEditar.DataAdmissao;

            BtnSalvar.Content = "Atualizar";

            // ATUALIZADO: Abre o Expander e muda o título
            FormExpander.Header = $"Editando: {funcionarioParaEditar.Nome}";
            FormExpander.IsExpanded = true;
        }

        private void ButtonExcluir_Click(object sender, RoutedEventArgs e)
        {
            Funcionario funcionarioParaExcluir = (Funcionario)((Button)sender).DataContext;

            MessageBoxResult resposta = MessageBox.Show(
                $"Tem certeza que deseja excluir o funcionário '{funcionarioParaExcluir.Nome}'?",
                "Confirmar Exclusão",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (resposta == MessageBoxResult.Yes)
            {
                try
                {
                    funcionarioRepository.Excluir(funcionarioParaExcluir);
                    MessageBox.Show("Funcionário excluído com sucesso!");
                    LoadGrid();
                }
                catch (Exception ex)
                {
                    Exception inner = ex;
                    while (inner.InnerException != null) { inner = inner.InnerException; }
                    MessageBox.Show($"Erro ao excluir: {inner.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // ADICIONADO: Método para o botão Cancelar
        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
            FormExpander.IsExpanded = false;
        }
    }
}