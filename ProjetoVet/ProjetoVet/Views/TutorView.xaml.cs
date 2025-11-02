using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ProjetoVet.Models;
using ProjetoVet.Repositories;

namespace ProjetoVet.Views
{
    public partial class TutorView : UserControl
    {
        // Alterado para Tutor
        private readonly TutorRepository _tutorRepository;
        private List<Tutor> _listaCompletaTutores;
        private Tutor _tutorEmEdicao = null;

        public TutorView()
        {
            InitializeComponent();
            _tutorRepository = new TutorRepository();
            CarregarTutoresDoBanco();
        }

        private void CarregarTutoresDoBanco()
        {
            _listaCompletaTutores = _tutorRepository.Consultar();
            AplicarFiltro();
        }

        private void AplicarFiltro()
        {
            var tutoresFiltrados = _listaCompletaTutores;

            if (TxtPesquisa != null && !string.IsNullOrWhiteSpace(TxtPesquisa.Text))
            {
                string filtro = TxtPesquisa.Text.ToLower();

                tutoresFiltrados = _listaCompletaTutores.Where(
                    t => t.Nome.ToLower().Contains(filtro) ||
                         t.CPF.Contains(filtro)
                ).ToList();
            }

            TutoresDataGrid.ItemsSource = tutoresFiltrados;
        }

        private void TxtPesquisa_TextChanged(object sender, TextChangedEventArgs e)
        {
            AplicarFiltro();
        }

        private void ButtonSalvar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validação adaptada para Tutor
                if (string.IsNullOrWhiteSpace(TxtNome.Text) ||
                    string.IsNullOrWhiteSpace(TxtCpf.Text) ||
                    string.IsNullOrWhiteSpace(TxtEmail.Text) ||
                    string.IsNullOrWhiteSpace(TxtTelefone.Text) ||
                    string.IsNullOrWhiteSpace(TxtStatus.Text) ||
                    DpDataNascimento.SelectedDate == null)
                {
                    MessageBox.Show("Por favor, preencha todos os campos obrigatórios.");
                    return;
                }

                if (_tutorEmEdicao == null)
                {
                    // 1. CRIAR NOVO TUTOR
                    Tutor novoTutor = new Tutor();

                    // Preenche dados da Pessoa
                    novoTutor.Nome = TxtNome.Text;
                    novoTutor.CPF = TxtCpf.Text;
                    novoTutor.Email = TxtEmail.Text;
                    novoTutor.Telefone = TxtTelefone.Text;
                    novoTutor.DataNascimento = DpDataNascimento.SelectedDate.Value;

                    // Preenche dados do Tutor
                    novoTutor.Status = TxtStatus.Text;

                    _tutorRepository.Adicionar(novoTutor);
                    MessageBox.Show("Tutor salvo com sucesso!");
                }
                else
                {
                    // 2. ATUALIZAR TUTOR EXISTENTE
                    _tutorEmEdicao.Nome = TxtNome.Text;
                    _tutorEmEdicao.CPF = TxtCpf.Text;
                    _tutorEmEdicao.Email = TxtEmail.Text;
                    _tutorEmEdicao.Telefone = TxtTelefone.Text;
                    _tutorEmEdicao.DataNascimento = DpDataNascimento.SelectedDate.Value;

                    _tutorEmEdicao.Status = TxtStatus.Text;

                    _tutorRepository.Alterar(_tutorEmEdicao);
                    MessageBox.Show("Tutor atualizado com sucesso!");
                }

                CarregarTutoresDoBanco();
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
            Tutor tutorParaEditar = (Tutor)((Button)sender).DataContext;
            _tutorEmEdicao = tutorParaEditar;

            // Carrega dados da Pessoa
            TxtNome.Text = tutorParaEditar.Nome;
            TxtCpf.Text = tutorParaEditar.CPF;
            TxtEmail.Text = tutorParaEditar.Email;
            TxtTelefone.Text = tutorParaEditar.Telefone;
            DpDataNascimento.SelectedDate = tutorParaEditar.DataNascimento;

            // Carrega dados do Tutor
            TxtStatus.Text = tutorParaEditar.Status;

            BtnSalvar.Content = "Atualizar";
            FormExpander.Header = $"Editando: {tutorParaEditar.Nome}";
            FormExpander.IsExpanded = true;
        }

        private void ButtonExcluir_Click(object sender, RoutedEventArgs e)
        {
            Tutor tutorParaExcluir = (Tutor)((Button)sender).DataContext;

            MessageBoxResult resposta = MessageBox.Show(
                $"Tem certeza que deseja excluir o tutor '{tutorParaExcluir.Nome}'?",
                "Confirmar Exclusão",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (resposta == MessageBoxResult.Yes)
            {
                try
                {
                    _tutorRepository.Excluir(tutorParaExcluir);
                    MessageBox.Show("Tutor excluído com sucesso!");
                    CarregarTutoresDoBanco();
                }
                catch (System.Exception ex)
                {
                    // Lógica para tratar exclusão de tutor com animais
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("REFERENCE constraint"))
                    {
                        MessageBox.Show("Não é possível excluir este tutor, pois ele possui animais ou endereços cadastrados.");
                    }
                    else
                    {
                        Exception inner = ex;
                        while (inner.InnerException != null) { inner = inner.InnerException; }
                        MessageBox.Show($"Erro ao excluir: {inner.Message}");
                    }
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

            // Limpa dados do Tutor
            TxtStatus.Clear();

            _tutorEmEdicao = null;

            BtnSalvar.Content = "Salvar";
            FormExpander.Header = "[+] Adicionar Novo Tutor";

            if (TxtPesquisa != null && TxtPesquisa.Text != "")
            {
                TxtPesquisa.Clear();
            }
        }
    }
}