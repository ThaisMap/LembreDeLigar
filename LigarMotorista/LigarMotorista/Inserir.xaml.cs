using System;
using System.Windows;
using System.Windows.Controls;

namespace LigarMotorista
{
    /// <summary>
    /// Interação lógica para Inserir.xam
    /// </summary>
    public partial class Inserir : UserControl
    {
        public Inserir()
        {
            InitializeComponent();
        }

        private void Incluir_Click(object sender, RoutedEventArgs e)
        {
            if (NomeMotorista.Text != "")
            {
                Dados d = new Dados();
                d.InserirMotorista(montarMotorista());
                NomeMotorista.Text = "";
                NF.Text = "";
                Fornecedor.Text = "";
                Cliente.Text = "";
                Observacao.Text = "";
                DataManifesto.SelectedDate = null;
                NomeMotorista.Focus();
            }
            else
                NomeMotorista.Focus();
        }

        private Motorista montarMotorista()
        {
            return new Motorista()
            {
                NomeMotorista = NomeMotorista.Text,
                NF = NF.Text,
                Fornecedor = Fornecedor.Text,
                Cliente = Cliente.Text,
                Observacao = Observacao.Text,
                DataManifesto = DataManifesto.SelectedDate == null ? DateTime.Today : (DateTime)DataManifesto.SelectedDate
            };
        }

        private void NomeMotorista_Maiusculo(object sender, EventArgs e)
        {
            NomeMotorista.Text = NomeMotorista.Text.ToUpper();
        }
    }
}