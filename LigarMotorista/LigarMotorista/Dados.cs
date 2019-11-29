using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Windows.Data;
using Excel = Microsoft.Office.Interop.Excel;

namespace LigarMotorista
{
    internal class Dados
    {
        private readonly String conexao = @"Data Source=LembreLigarBD.db";

        private void LiberarObjetos(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception)
            {
            }
            finally
            {
                GC.Collect();
            }
        }
 
        #region entrega

        public void AlterarEntrega(string id, string campo, string valor)
        {
            using (SQLiteConnection conn = new SQLiteConnection(conexao))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "UPDATE Entregas SET " + campo + "=@param WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@param", valor);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    { }
                    finally
                    { conn.Close(); }
                }
            }
        }

        public void DeletarEntrega(string id)
        {
            using (SQLiteConnection conn = new SQLiteConnection(conexao))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "DELETE FROM Entregas WHERE Id = @id";
                    try
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        public bool ExcelEntrega(string nomeArquivo, List<Motorista> lista)
        {
            try
            {
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;

                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);

                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlWorkSheet.Cells[1, 1] = "Data do manifesto";
                xlWorkSheet.Cells[1, 2] = "Motorista";
                xlWorkSheet.Cells[1, 3] = "NF";
                xlWorkSheet.Cells[1, 4] = "Fornecedor";
                xlWorkSheet.Cells[1, 5] = "Cliente";
                xlWorkSheet.Cells[1, 6] = "Observação";
                xlWorkSheet.Cells[1, 7] = "Ação";
                xlWorkSheet.Cells[1, 8] = "Ultima ligação";
                xlWorkSheet.Cells[1, 9] = "Quem ligou";
                int linha = 2;

                string[] divergencias;

                foreach (Motorista i in lista)
                {
                    xlWorkSheet.Cells[linha, 1] = i.DataManifesto.ToShortDateString();
                    xlWorkSheet.Cells[linha, 2] = i.NomeMotorista;
                    xlWorkSheet.Cells[linha, 3] = i.NF;
                    xlWorkSheet.Cells[linha, 4] = i.Fornecedor;
                    xlWorkSheet.Cells[linha, 5] = i.Cliente;
                    xlWorkSheet.Cells[linha, 6] = i.Observacao;
                    xlWorkSheet.Cells[linha, 7] = i.Acao;
                    xlWorkSheet.Cells[linha, 8] = i.UltimaLigacao.ToString("HH:mm");
                    xlWorkSheet.Cells[linha, 9] = i.QuemLigou;
                    linha++;
                }

                xlWorkBook.SaveAs(nomeArquivo, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue,
 Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                LiberarObjetos(xlWorkSheet);
                LiberarObjetos(xlWorkBook);
                LiberarObjetos(xlApp);
                return true;
            }
            catch (Exception ex)
            {
                var erro = ex;
                return false;
            }
        }
             
        public ObservableCollection<Motorista> LeDadosEntrega<S, T>(string query) where S : IDbConnection, new()
                                            where T : IDbDataAdapter, IDisposable, new()
        {
            var ListaDeMotoristas = new ObservableCollection<Motorista>();
            using (var conn = new S())
            {
                using (var da = new T())
                {
                    using (da.SelectCommand = conn.CreateCommand())
                    {
                        Motorista aux;
                        da.SelectCommand.CommandText = query;
                        da.SelectCommand.Connection.ConnectionString = conexao;
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            DateTime.TryParse(row["LastCall"].ToString(), out DateTime Ult);
                            DateTime.TryParse(row["DateM"].ToString(), out DateTime Manif);
                            aux = new Motorista(row["Id"].ToString(), row["Name"].ToString(), row["Obs"].ToString())
                            {
                                QuemLigou = row["Caller"].ToString(),
                                Intervalo = int.Parse(row["Interval"].ToString()),
                                UltimaLigacao = Ult,
                                NF = row["NF"].ToString(),
                                Fornecedor = row["Supplier"].ToString(),
                                Cliente = row["Client"].ToString(),
                                Acao = row["Action"].ToString(),
                                DataManifesto = Manif
                            };
                            aux.CalculaProxima();
                            ListaDeMotoristas.Add(aux);
                        }
                    }
                }
            }
            return ListaDeMotoristas;
        }

        public void FinalizarEntrega(string id)
        {
            using (SQLiteConnection conn = new SQLiteConnection(conexao))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "UPDATE Entregas SET Free=1 WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    { }
                    finally
                    { conn.Close(); }
                }
            }
        }

        public void Ligacao(string id, string nome)
        {
            using (SQLiteConnection conn = new SQLiteConnection(conexao))
            {
                conn.Open();

                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "UPDATE Entregas SET LastCall=@ultima, Caller=@quem WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@ultima", DateTime.Now);
                    cmd.Parameters.AddWithValue("@quem", nome);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    { }
                    finally
                    { conn.Close(); }
                }
            }
        }

        public void InserirMotorista(Motorista novo)
        {
            using (SQLiteConnection conn = new SQLiteConnection(conexao))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "INSERT INTO Entregas(Name,LastCall,Interval,Obs, NF, Supplier, Client, DateM) " +
                        "VALUES (@nome,@ultima,@intervalo,@obs,@nf,@sup,@cli,@datem)";
                    cmd.Parameters.AddWithValue("@nome", novo.NomeMotorista);
                    cmd.Parameters.AddWithValue("@ultima", DateTime.Today);
                    cmd.Parameters.AddWithValue("@intervalo", 0);
                    cmd.Parameters.AddWithValue("@obs", novo.Observacao);
                    cmd.Parameters.AddWithValue("@nf", novo.NF);
                    cmd.Parameters.AddWithValue("@cli", novo.Cliente);
                    cmd.Parameters.AddWithValue("@datem", novo.DataManifesto);
                    cmd.Parameters.AddWithValue("@sup", novo.Fornecedor);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    { }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        #endregion

        #region Diaria

        public void AlterarDiaria(string id, string campo, string valor)
        {
            using (SQLiteConnection conn = new SQLiteConnection(conexao))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "UPDATE Diarias SET " + campo + "=@param WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@param", valor);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    { }
                    finally
                    { conn.Close(); }
                }
            }
        }
       
        public void DeletarDiaria(string id)
        {
            using (SQLiteConnection conn = new SQLiteConnection(conexao))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "DELETE FROM Diarias WHERE Id = @id";
                    try
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception EX)
                    {
                        var aux = EX.Message;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        public bool ExcelDiaria(string nomeArquivo, List<DiariaModel> lista)
        {
            try
            {
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;

                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);

                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlWorkSheet.Cells[1, 1] = "Motorista";
                xlWorkSheet.Cells[1, 2] = "NF";
                xlWorkSheet.Cells[1, 3] = "Fornecedor";
                xlWorkSheet.Cells[1, 4] = "Cliente";
                xlWorkSheet.Cells[1, 5] = "Observação";
                int linha = 2;

                string[] divergencias;

                foreach (DiariaModel i in lista)
                {
                    xlWorkSheet.Cells[linha, 1] = i.Name;
                    xlWorkSheet.Cells[linha, 2] = i.NF;
                    xlWorkSheet.Cells[linha, 3] = i.Fornecedor;
                    xlWorkSheet.Cells[linha, 4] = i.Client;
                    xlWorkSheet.Cells[linha, 5] = i.Obs;
                    linha++;
                }

                xlWorkBook.SaveAs(nomeArquivo, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue,
 Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                LiberarObjetos(xlWorkSheet);
                LiberarObjetos(xlWorkBook);
                LiberarObjetos(xlApp);

                return true;
            }
            catch (Exception ex)
            {
                var erro = ex;
                return false;
            }
        }

        public void NovaDiaria(string idEntrega)
        {
            using (SQLiteConnection conn = new SQLiteConnection(conexao))
            {
                Motorista MotoDiaria = LeDadosEntrega<SQLiteConnection, SQLiteDataAdapter>("Select * from Entregas where Id=" + idEntrega)[0];

                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "INSERT INTO Diarias (Name,Client,Supplier,Obs,NF) VALUES (@nome,@cliente,@fornecedor,@obs,@nf)";
                    cmd.Parameters.AddWithValue("@nome", MotoDiaria.NomeMotorista);
                    cmd.Parameters.AddWithValue("@obs", MotoDiaria.Observacao);
                    cmd.Parameters.AddWithValue("@cliente", MotoDiaria.Cliente);
                    cmd.Parameters.AddWithValue("@fornecedor", MotoDiaria.Fornecedor);
                    cmd.Parameters.AddWithValue("@nf", MotoDiaria.NF);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    { }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        public ObservableCollection<DiariaModel> LeDadosDiaria<S, T>(string query) where S : IDbConnection, new()
                                           where T : IDbDataAdapter, IDisposable, new()
        {
            var ListaDeDiarias = new ObservableCollection<DiariaModel>();
            using (var conn = new S())
            {
                using (var da = new T())
                {
                    using (da.SelectCommand = conn.CreateCommand())
                    {
                        DiariaModel aux;
                        da.SelectCommand.CommandText = query;
                        da.SelectCommand.Connection.ConnectionString = conexao;
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            aux = new DiariaModel()
                            {
                                Id = row["Id"].ToString(),
                                Name = row["Name"].ToString(),
                                Client = row["Client"].ToString(),
                                Fornecedor = row["Supplier"].ToString(),
                                Obs = row["Obs"].ToString(),
                                NF = row["NF"].ToString()
                            };
                            ListaDeDiarias.Add(aux);
                        }
                    }
                }
            }

            return ListaDeDiarias;
        }
        
        #endregion

        #region Devolucao

        public void AlterarDevolucao(string id, string campo, string valor)
        {
            using (SQLiteConnection conn = new SQLiteConnection(conexao))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "UPDATE Devolucoes SET " + campo + "=@param WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@param", valor);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    { }
                    finally
                    { conn.Close(); }
                }
            }
        }
 
        public void DeletarDevolucao(string id)
        {
            using (SQLiteConnection conn = new SQLiteConnection(conexao))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "DELETE FROM Devolucoes WHERE Id = @id";
                    try
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception EX)
                    {
                        var aux = EX.Message;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }


        #endregion

        #region Log

        public bool ExcelLog(string nomeArquivo, List<LogModel> lista)
        {
            try
            {
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;

                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);

                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlWorkSheet.Cells[1, 1] = "Data";
                xlWorkSheet.Cells[1, 2] = "Hora";
                xlWorkSheet.Cells[1, 3] = "Quem Ligou";
                xlWorkSheet.Cells[1, 4] = "Motorista";
                xlWorkSheet.Cells[1, 5] = "Observação";
                int linha = 2;

                string[] divergencias;

                foreach (LogModel i in lista)
                {
                    xlWorkSheet.Cells[linha, 1] = i.Data.ToShortDateString();
                    xlWorkSheet.Cells[linha, 2] = i.Hora.ToShortTimeString();
                    xlWorkSheet.Cells[linha, 3] = i.QuemLigou;
                    xlWorkSheet.Cells[linha, 4] = i.NomeMotorista;
                    xlWorkSheet.Cells[linha, 5] = i.Obs;
                    linha++;
                }

                xlWorkBook.SaveAs(nomeArquivo, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue,
 Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                LiberarObjetos(xlWorkSheet);
                LiberarObjetos(xlWorkBook);
                LiberarObjetos(xlApp);

                return true;
            }
            catch (Exception ex)
            {
                var erro = ex;
                return false;
            }
        }

        public ObservableCollection<LogModel> LeDadosLog<S, T>(string query) where S : IDbConnection, new()
                                            where T : IDbDataAdapter, IDisposable, new()
        {
            var ListaDeLigacoes = new ObservableCollection<LogModel>();
            using (var conn = new S())
            {
                using (var da = new T())
                {
                    using (da.SelectCommand = conn.CreateCommand())
                    {
                        LogModel aux;
                        da.SelectCommand.CommandText = query;
                        da.SelectCommand.Connection.ConnectionString = conexao;
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            DateTime.TryParse(row["Horario"].ToString(), out DateTime hora);
                            DateTime.TryParse(row["Data"].ToString(), out DateTime data);
                            aux = new LogModel()
                            {
                                QuemLigou = row["Nome"].ToString(),
                                NomeMotorista = row["Motorista"].ToString(),
                                Hora = hora,
                                Data = data,
                                Obs = row["obsContato"].ToString()
                            };
                            ListaDeLigacoes.Add(aux);
                        }
                    }
                }
            }
            return ListaDeLigacoes;
        }
 
        public void SalvarLog(string idLigacao)
        {
            Motorista Ligacao = LeDadosEntrega<SQLiteConnection, SQLiteDataAdapter>("Select * from Entregas where Id=" + idLigacao)[0];

            using (SQLiteConnection conn = new SQLiteConnection(conexao))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "INSERT INTO LogLigacoes(Nome, Motorista, Horario, Data, ObsContato) " +
                         "VALUES (@nome,@motorista,@hora,@data,@obs)";
                    cmd.Parameters.AddWithValue("@nome", Ligacao.QuemLigou);
                    cmd.Parameters.AddWithValue("@motorista", Ligacao.NomeMotorista);
                    cmd.Parameters.AddWithValue("@hora", DateTime.Now.TimeOfDay);
                    cmd.Parameters.AddWithValue("@data", DateTime.Now.Date);
                    cmd.Parameters.AddWithValue("@obs", Ligacao.Observacao);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    { }
                    finally
                    { conn.Close(); }
                }
            }
        }

        #endregion
   }
}