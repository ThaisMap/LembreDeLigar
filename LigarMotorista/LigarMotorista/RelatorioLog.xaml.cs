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

namespace LigarMotorista
{
    /// <summary>
    /// Interação lógica para RelatorioLog.xam
    /// </summary>
    public partial class RelatorioLog : UserControl
    {
        private Dados d = new Dados();

        public RelatorioLog()
        {
            InitializeComponent();
          
            dataInicio.SelectedDate = DateTime.Today;
            dataFim.SelectedDate = DateTime.Today;
        }

        private ObservableCollection<LogModel> ListaDeLigacoes { get; set; }

        private void BtnExportar_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog salvar = new SaveFileDialog();
            salvar.AddExtension = true;
            salvar.DefaultExt = "xls";
            salvar.Title = "Salvar relatório em Excel";
            if (salvar.ShowDialog() == true)
            {
                if (d.ExcelLog(salvar.FileName, ListaDeLigacoes.ToList()))
                    MessageBox.Show("Arquivo salvo com sucesso.");
                else
                    MessageBox.Show("O arquivo não foi salvo.");
            }
        }

        private void Pesquisar_Click(object sender, RoutedEventArgs e)
        {
            string inicio = string.Format("{0:yyyy-MM-dd HH:mm:ss}", dataInicio.SelectedDate);
            string fim = string.Format("{0:yyyy-MM-dd HH:mm:ss}", dataFim.SelectedDate);

            string query = "Select * from LogLigacoes where Data >='" + inicio + "' and Data <= '" + fim + "'";

            if (nomePesquisa.Text != "")
            {
                query += " and Nome like '" + nomePesquisa.Text + "%'";
            }

            if (nomeMoto.Text != "")
            {
                query += " and Motorista like '" + nomeMoto.Text + "%'";
            }
            if (textoOcorrencia.Text != "")
            {
                query += " and ObsContato like '%" + textoOcorrencia.Text + "%'";
            }
            ListaDeLigacoes = d.LeDadosLog<SQLiteConnection, SQLiteDataAdapter>(query);
            dgLista.ItemsSource = ListaDeLigacoes;
        }
    }
}