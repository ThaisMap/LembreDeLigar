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
    /// Interação lógica para Devolucao.xam
    /// </summary>
    public partial class Devolucao : UserControl
    {
        public Devolucao()
        {
            InitializeComponent();

            CarregaDados();
        }

        private Dados d = new Dados();
        private bool isEditing;

        ObservableCollection<DevolucaoModel> ListaDeDevolucoes { get; set; }

        private void CarregaDados()
        {
            ListaDeDevolucoes = d.LeDadosDevolucao<SQLiteConnection, SQLiteDataAdapter>("Select * from Devolucoes");
            if (ListaDeDevolucoes.Count > 0)
            {
                dgLista.ItemsSource = ListaDeDevolucoes;
            }
            else
                dgLista.ItemsSource = null;
        }

        private void ApagarTudo_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Apagar todas as devolucoes da lista? ", "Confirmação", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                foreach (var linha in ListaDeDevolucoes)
                {
                    d.DeletarDevolucao(linha.Id);
                }
                CarregaDados();
            }
        }

       

        private void Apagar(object sender, RoutedEventArgs e)
        {
            var id = ((Button)sender).CommandParameter.ToString();
            Deletar(id);
        }

        private void Deletar(string id)
        {
            if (MessageBox.Show("Apagar devolução da lista? ", "Confirmação", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                d.DeletarDevolucao(id);
                CarregaDados();
            }
        }

        private void Registrar_Alteracao(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit) //caso a alteração seja confirmada
            {
                //para descobrir qual propriedade esta ligada com a coluna alterada
                if (e.Column is DataGridBoundColumn column)
                {
                    var bindingPath = (column.Binding as Binding).Path.Path; //esse é o nome da propriedade

                    DevolucaoModel teste = e.Row.Item as DevolucaoModel;  //o item da coleção correspondente a linha alterada
                    var el = e.EditingElement as TextBox;       //o campo alterado

                    d.AlterarDevolucao(teste.Id, bindingPath, el.Text);
                }
            }
            isEditing = false;
        }

        private void DgLista_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Delete == e.Key && dgLista.SelectedItems.Count > 0)
            {
                var linha = dgLista.SelectedItem as Motorista;
                Deletar(linha.Id);
            }
        }
    }
}
