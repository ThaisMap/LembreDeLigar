using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace LigarMotorista
{
    /// <summary>
    /// Interação lógica para Pendentes.xam
    /// </summary>
    public partial class Pendentes : UserControl
    {
        private List<Motorista> copiaAtrasos = new List<Motorista>();

        private Dados d = new Dados();
        private bool isEditing = false;
        private string nomePessoa = "";
        private DispatcherTimer timer = new DispatcherTimer();

        public Pendentes(string nome)
        {
            InitializeComponent();
            nomePessoa = nome;

            ListaDeMotoristas = new ObservableCollection<Motorista>();

            CarregaDados();

            timer.Interval = TimeSpan.FromMinutes(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private ObservableCollection<Motorista> ListaDeMotoristas { get; set; }

        private void Atualizar_Tick(object sender, EventArgs e)
        {
            CarregaDados();
        }

        private void CarregaDados()
        {
            ListaDeMotoristas = d.LeDadosEntrega<SQLiteConnection, SQLiteDataAdapter>("Select * from Entregas where Free=0");
            if (ListaDeMotoristas.Count > 0)
                dgLista.ItemsSource = ListaDeMotoristas;
            else
            {
                dgLista.ItemsSource = null;
            }
        }

        private void DgLista_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            isEditing = true;
        }

        private void DgLista_PreviewKeyDown(object sender, KeyEventArgs e)
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
            if (MessageBox.Show("Passar motorista para lista de finalizados? ", "Confirmação", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                d.AlterarEntrega(id, "Free", "1");
                CarregaDados();
            }
        }

        private void FinalizarTudo_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Passar todos os motoristas para lista de finalizados? ", "Confirmação", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                foreach (var linha in ListaDeMotoristas)
                {
                    d.AlterarEntrega(linha.Id, "Free", "1");
                }
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
            if (e.EditAction == DataGridEditAction.Commit) //caso a alteração seja confirmada
            {
                //para descobrir qual propriedade esta ligada com a coluna alterada
                if (e.Column is DataGridBoundColumn column)
                {
                    var bindingPath = (column.Binding as Binding).Path.Path; //esse é o nome da propriedade

                    Motorista teste = e.Row.Item as Motorista;  //o item da coleção correspondente a linha alterada
                    var el = e.EditingElement as TextBox;       //o campo alterado
                    string campo;
                    switch (bindingPath)
                    {
                        case "Intervalo":
                            campo = "Interval";
                            break;

                        case "Acao":
                            campo = "Action";
                            break;

                        case "NF":
                            campo = "NF";
                            break;

                        case "Fornecedor":
                            campo = "Supplier";
                            break;

                        case "Cliente":
                            campo = "Client";
                            break;

                        default:
                            campo = "Obs";
                            break;
                    }
                    d.AlterarEntrega(teste.Id, campo, el.Text);
                }
            }
            isEditing = false;
        }

        private void Registrar_Ligacao(object sender, RoutedEventArgs e)
        {
            var id = ((Button)sender).CommandParameter.ToString();
            if (id != null)
            {
                d.Ligacao(id, nomePessoa);

                d.SalvarLog(id);
            }
            System.Windows.MessageBox.Show("Ligação Registrada");
            CarregaDados();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            copiaAtrasos = ListaDeMotoristas.ToList();
            foreach (Motorista item in copiaAtrasos)
            {
                item.CalculaProxima();               
            }
        }
    }
}