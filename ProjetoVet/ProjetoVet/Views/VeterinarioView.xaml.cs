using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ProjetoVet.Models;
using ProjetoVet.Repositories;
using System.Globalization; // Para converter o Salário

namespace ProjetoVet.Views
{
    public partial class VeterinarioView : UserControl
    {
        private readonly VeterinarioRepository _veterinarioRepository;
        private List<Veterinario> _listaCompletaVeterinarios;
        private Veterinario _veterinarioEmEdicao = null;

        public VeterinarioView()
        {
            InitializeComponent();
            _veterinarioRepository = new VeterinarioRepository();
            CarregarVeterinariosDoBanco();
        }

        private void CarregarVeterinariosDoBanco()
        {
            _listaCompletaVeterinarios = _veterinarioRepository.Consultar();
            AplicarFiltro();
        }

        private void AplicarFiltro()
        {
            var veterinariosFiltrados = _listaCompletaVeterinarios;

            if (TxtPesquisa != null && !string.IsNullOrWhiteSpace(TxtPesquisa.Text))
            {
                string filtro = TxtPesquisa.Text.ToLower();

                // Filtra por Nome ou CRMV
                veterinariosFiltrados = _listaCompletaVeterinarios.Where(
                    v => v.Nome.ToLower().Contains(filtro) ||
                         v.CRMV.Contains(filtro)
                ).ToList();
            }

            VeterinariosDataGrid.ItemsSource = veterinariosFiltrados;
        }

        private void TxtPesquisa_TextChanged(object sender, TextChangedEventArgs e)
        {
            AplicarFiltro();
        }

        private void ButtonSalvar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validação (adaptada)
                if (string.IsNullOrWhiteSpace(TxtNome.Text) ||
                    string.IsNullOrWhiteSpace(TxtCpf.Text) ||
                    string.IsNullOrWhiteSpace(TxtEmail.Text) ||
                    string.IsNullOrWhiteSpace(TxtCRMV.Text) ||
                    string.IsNullOrWhiteSpace(TxtSalario.Text) ||
                    DpDataNascimento.SelectedDate == null)
                {
                    MessageBox.Show("Por favor, preencha todos os campos obrigatórios.");
                    return;
                }

                // Validação do Salário
                if (!decimal.TryParse(TxtSalario.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal salario))
                {
                    if (!decimal.TryParse(TxtSalario.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out salario))
                    {
                        MessageBox.Show("Valor do Salário inválido. Use um número (ex: 3500.50).");
                        return;
                    }
                }


                if (_veterinarioEmEdicao == null)
                {
                    // 1. CRIAR NOVO
                    Veterinario novoVeterinario = new Veterinario();

                    // Preenche dados da Pessoa
                    novoVeterinario.Nome = TxtNome.Text;
                    novoVeterinario.CPF = TxtCpf.Text;
                    novoVeterinario.Email = TxtEmail.Text;
                    novoVeterinario.Telefone = TxtTelefone.Text;
                    novoVeterinario.DataNascimento = DpDataNascimento.SelectedDate.Value;

                    // Preenche dados do Veterinário
                    novoVeterinario.CRMV = TxtCRMV.Text;
                    novoVeterinario.Especialidade = TxtEspecialidade.Text;
                    novoVeterinario.Salario = salario;
                    novoVeterinario.DataAdmissao = DateTime.Now; // Pega a data atual
                    novoVeterinario.Situacao = "Ativo"; // Define um padrão

                    _veterinarioRepository.Adicionar(novoVeterinario);
                    MessageBox.Show("Veterinário salvo com sucesso!");
                }
                else
                {
                    // 2. ATUALIZAR EXISTENTE
                    _veterinarioEmEdicao.Nome = TxtNome.Text;
                    _veterinarioEmEdicao.CPF = TxtCpf.Text;
                    _veterinarioEmEdicao.Email = TxtEmail.Text;
                    _veterinarioEmEdicao.Telefone = TxtTelefone.Text;
                    _veterinarioEmEdicao.DataNascimento = DpDataNascimento.SelectedDate.Value;

                    _veterinarioEmEdicao.CRMV = TxtCRMV.Text;
                    _veterinarioEmEdicao.Especialidade = TxtEspecialidade.Text;
                    _veterinarioEmEdicao.Salario = salario;

                    _veterinarioRepository.Alterar(_veterinarioEmEdicao);
                    MessageBox.Show("Veterinário atualizado com sucesso!");
                }

                CarregarVeterinariosDoBanco();
                LimparCampos();
                FormExpander.IsExpanded = false;
            }
            catch (System.Exception ex)
            {
                Exception inner = ex;
                while (inner.InnerException != null)
                {
                    inner = inner.InnerException;
                }
                MessageBox.Show($"Erro ao salvar: {inner.Message}");
            }
        }

        private void ButtonEditar_Click(object sender, RoutedEventArgs e)
        {
            Veterinario veterinarioParaEditar = (Veterinario)((Button)sender).DataContext;
            _veterinarioEmEdicao = veterinarioParaEditar;

            // Carrega dados da Pessoa
            TxtNome.Text = veterinarioParaEditar.Nome;
            TxtCpf.Text = veterinarioParaEditar.CPF;
            TxtEmail.Text = veterinarioParaEditar.Email;
            TxtTelefone.Text = veterinarioParaEditar.Telefone;
            DpDataNascimento.SelectedDate = veterinarioParaEditar.DataNascimento;

            // Carrega dados do Veterinário
            TxtCRMV.Text = veterinarioParaEditar.CRMV;
            TxtEspecialidade.Text = veterinarioParaEditar.Especialidade;
            TxtSalario.Text = veterinarioParaEditar.Salario.ToString("F2"); // Formata para 2 casas

            BtnSalvar.Content = "Atualizar";
            FormExpander.Header = $"Editando: {veterinarioParaEditar.Nome}";
            FormExpander.IsExpanded = true;
        }

        private void ButtonExcluir_Click(object sender, RoutedEventArgs e)
        {
            Veterinario veterinarioParaExcluir = (Veterinario)((Button)sender).DataContext;

            MessageBoxResult resposta = MessageBox.Show(
                $"Tem certeza que deseja excluir o veterinário '{veterinarioParaExcluir.Nome}'?",
                "Confirmar Exclusão",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (resposta == MessageBoxResult.Yes)
            {
                try
                {
                    _veterinarioRepository.Excluir(veterinarioParaExcluir);
                    MessageBox.Show("Veterinário excluído com sucesso!");
                    CarregarVeterinariosDoBanco();
                }
                catch (System.Exception ex)
                {
                    Exception inner = ex;
                    while (inner.InnerException != null) { inner = inner.InnerException; }
                    MessageBox.Show($"Erro ao excluir: {inner.Message}");
                }
            }
        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimparCampos();
            FormExpander.IsExpanded = false;
        }

        private void LimparCampos()
        {
            // Limpa dados da Pessoa
            TxtNome.Clear();
            TxtCpf.Clear();
            TxtEmail.Clear();
            TxtTelefone.Clear();
            DpDataNascimento.SelectedDate = null;

            // Limpa dados do Veterinário
            TxtCRMV.Clear();
            TxtEspecialidade.Clear();
            TxtSalario.Clear();

            _veterinarioEmEdicao = null;

            BtnSalvar.Content = "Salvar";
            FormExpander.Header = "[+] Adicionar Novo Veterinário";

            if (TxtPesquisa != null && TxtPesquisa.Text != "")
            {
                TxtPesquisa.Clear();
            }
        }
    }
}