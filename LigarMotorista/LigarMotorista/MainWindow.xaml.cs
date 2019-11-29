using Microsoft.Win32;
using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows;
using Ganss.Excel;

namespace LigarMotorista
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dados d = new Dados();
        private string nome = "teste";
        private string nomeArquivo = "motoristas em entrega.xlsx";
        Diarias diarias = new Diarias();


        public MainWindow()
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            SeuNomeDialog.IsOpen = true;
          
        }

        private void BtnDiarias_Click(object sender, RoutedEventArgs e)
        {
            Conteudo.Content = diarias;
        }

        private void BtnFinalizados_Click(object sender, RoutedEventArgs e)
        {
            Conteudo.Content = new Finalizados();
        }

        private void BtnIncluir_Click(object sender, RoutedEventArgs e)
        {
            Conteudo.Content = (new Inserir());
        }

        private void BtnPendentes_Click(object sender, RoutedEventArgs e)
        {
            Conteudo.Content = new Pendentes(nome);
        }

        private void BtnRelatorio_Click(object sender, RoutedEventArgs e)
        {
            Conteudo.Content = new RelatorioLog();
        }

        private void GetArquivo()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                DefaultExt = "xls",
                Filter = "Excel (*.xlsx)|*.xlsx|Excel (*.xls)|*.xls"
            };
            if (openFileDialog.ShowDialog() == true && (openFileDialog.FileName.EndsWith("xls") || openFileDialog.FileName.EndsWith("xlsx")))
                nomeArquivo = openFileDialog.FileName;
        }
        
        private void MapearPlanilha()
        {
            if (MessageBox.Show("Importar dados da planilha?", "Importar", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                GetArquivo();

                if (File.Exists(nomeArquivo))
                {
                    try
                    {
                        var motoristas = new ExcelMapper(nomeArquivo).Fetch<Motorista>();
                        foreach (var item in motoristas)
                        {
                            if(item.NomeMotorista != "")
                                d.InserirMotorista(item);
                        }
                   }
                    catch (Exception ex)
                    {                        
                            MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Planilha não foi carregada.");
                }
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (txtNome.Text != "")
            {
                nome = txtNome.Text;                
                SeuNomeDialog.IsOpen = false;
                MapearPlanilha();
                Conteudo.Content = new Pendentes(nome);
            }
            else
                txtNome.Focus();
        }
    }
}