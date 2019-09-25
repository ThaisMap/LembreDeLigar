using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
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
using System.Windows.Shapes;

namespace LigarMotorista
{
    /// <summary>
    /// Lógica interna para MainWindowDevolucao.xaml
    /// </summary>
    public partial class MainWindowDevolucao : Window
    {
        public MainWindowDevolucao()
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            SeuNomeDialog.IsOpen = true;

        }

        private Dados d = new Dados();
        private string nome = "teste";
        private string nomeArquivo = "motoristas em entrega.xlsx";

      
        private void BtnDiarias_Click(object sender, RoutedEventArgs e)
        {
            Conteudo.Content = new Diarias();
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

        private void LerPlanilha()
        {
            if (MessageBox.Show("Importar dados da planilha?", "Importar", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                GetArquivo();

                if (File.Exists(nomeArquivo))
                {
                    OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + nomeArquivo + ";Extended Properties='Excel 12.0 Xml;HDR=Yes;'");
                    try
                    {
                        OleDbDataAdapter adapter = new OleDbDataAdapter("Select MOTORISTA, OBSERVAÇÃO, DATA from [Plan1$]", conn);
                        DataSet ds = new DataSet();
                        conn.Open();
                        adapter.Fill(ds);

                        foreach (DataRow linha in ds.Tables[0].Rows)
                        {
                            if (linha["MOTORISTA"].ToString() != "")
                            {
                                d.InserirMotorista(new Motorista()
                                {
                                    NomeMotorista = linha["MOTORISTA"].ToString(),
                                    Observacao = linha["OBSERVAÇÃO"].ToString(),
                                    DataManifesto = DateTime.Parse(linha["DATA"].ToString())
                                });
                            }
                            else
                                break;
                        }
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        conn.Close();
                        if (ex.HResult == -2147217904)
                            MessageBox.Show("Não foram encontrados na planilha os campos 'MOTORISTA', 'OBSERVAÇÃO' e/ou 'DATA'", "Erro");
                    }
                }
                else
                {
                    MessageBox.Show("Planilha não foi carregada.");
                }
            }
        }

        private void LiberarObjetos(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Ocorreu um erro durante a liberação do objeto " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void BtnDevolucao_Click(object sender, RoutedEventArgs e)
        {
            Conteudo.Content = new Devolucao();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (txtNome.Text != "")
            {
                nome = txtNome.Text;
                SeuNomeDialog.IsOpen = false;
                LerPlanilha();
                Conteudo.Content = new Pendentes(nome);
            }
            else
                txtNome.Focus();
        }
    }
}
