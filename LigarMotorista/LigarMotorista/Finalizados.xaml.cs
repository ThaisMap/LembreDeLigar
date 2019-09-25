using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LigarMotorista
{
    /// <summary>
    /// Interação lógica para Finalizados.xam
    /// </summary>
    public partial class Finalizados : UserControl
    {
        private Dados d = new Dados();

        public Finalizados()
        {
            InitializeComponent();
            
            CarregaDados();
        }

        public bool isEditing { get; private set; }
        private ObservableCollection<Motorista> ListaDeMotoristas { get; set; }

        private void ApagarTudo_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Apagar todos os motorista da lista? ", "Confirmação", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                foreach (var linha in ListaDeMotoristas)
                {
                    d.DeletarEntrega(linha.Id);
                }
                CarregaDados();
            }
        }

        private void CarregaDados()
        {
            ListaDeMotoristas = d.LeDadosEntrega<SQLiteConnection, SQLiteDataAdapter>("Select * from Entregas where Free=1");
            if (ListaDeMotoristas.Count > 0)
            {
                dgLista.ItemsSource = ListaDeMotoristas;
            }
            else
                dgLista.ItemsSource = null;
        }

        private void dgLista_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            isEditing = true;
        }

        private void dgLista_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Delete == e.Key && dgLista.SelectedItems.Count > 0 && !isEditing)
            {
                if (MessageBox.Show("Apagar motorista definitivamente? ", "Confirmação", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    var linha = dgLista.SelectedItem as Motorista;
                    d.DeletarEntrega(linha.Id);
                }
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
                if (d.ExcelEntrega(salvar.FileName, ListaDeMotoristas.ToList()))
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
                d.DeletarEntrega(id);
                CarregaDados();
            }
        }

        private void NovaDiaria(object sender, RoutedEventArgs e)
        {
            var id = ((Button)sender).CommandParameter.ToString();
            if (MessageBox.Show("Registrar diária para o motorista? ", "Confirmação", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                d.NovaDiaria(id);
                CarregaDados();
                MessageBox.Show("Diária registrada. ", "Confirmação", MessageBoxButton.OK);
            }
        }

        private void Registrar_Alteracao(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)  //caso a alteração seja confirmada
            {
                Motorista teste = e.Row.Item as Motorista;  //o item da coleção correspondente a linha alterada
                var el = e.EditingElement as TextBox;       //o campo alterado
                d.AlterarEntrega(teste.Id, "Obs", el.Text);        //el.text é o novo valor do campo
            }
            isEditing = false;
        }
    }
}