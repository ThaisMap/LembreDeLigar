using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LigarMotorista
{
    /// <summary>
    /// Interação lógica para Diarias.xam
    /// </summary>
    public partial class Diarias : UserControl
    {
        private Dados d = new Dados();

        public Diarias()
        {
            InitializeComponent();            
        }

        private ObservableCollection<DiariaModel> ListaDeDiarias { get; set; }

        private void ApagarTudo_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Apagar todas as diárias da lista? ", "Confirmação", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                foreach (var linha in ListaDeDiarias)
                {
                    d.DeletarDiaria(linha.Id);
                }
                CarregaDados();
            }
        }

        private void CarregaDados()
        {
            ListaDeDiarias = d.LeDadosDiaria<SQLiteConnection, SQLiteDataAdapter>("Select * from Diarias");
            if (ListaDeDiarias.Count > 0)
            {
                dgLista.ItemsSource = ListaDeDiarias;
            }
            else
                dgLista.ItemsSource = null;
        }

        private void DgLista_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Delete == e.Key && dgLista.SelectedItems.Count > 0)
            {
                var linha = dgLista.SelectedItem as Motorista;
                d.DeletarDiaria(linha.Id);
                CarregaDados();
            }
        }

        private void Exportar_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog salvar = new SaveFileDialog
            {
                AddExtension = true,
                DefaultExt = "xls",
                Title = "Salvar lista em Excel"
            };
            if (salvar.ShowDialog() == true)
            {
                if (d.ExcelDiaria(salvar.FileName, ListaDeDiarias.ToList()))
                    MessageBox.Show("Arquivo salvo com sucesso.");
                else
                    MessageBox.Show("O arquivo não foi salvo.");
            }
        }

        private void Finalizar(object sender, RoutedEventArgs e)
        {
            var id = ((Button)sender).CommandParameter.ToString();
            if (MessageBox.Show("Apagar motorista da lista? ", "Confirmação", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                d.DeletarDiaria(id);
                CarregaDados();
            }
        }

        private void Registrar_Alteracao(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)  //caso a alteração seja confirmada
            {
                DiariaModel teste = e.Row.Item as DiariaModel;        //o item da coleção correspondente a linha alterada
                var el = e.EditingElement as TextBox;       //o campo alterado
                d.AlterarDiaria(teste.Id, "Obs", el.Text);  //el.text é o novo valor do campo
            }
        }

        private void TelaDiaria_Loaded(object sender, RoutedEventArgs e)
        {
            CarregaDados();

        }
    }
}