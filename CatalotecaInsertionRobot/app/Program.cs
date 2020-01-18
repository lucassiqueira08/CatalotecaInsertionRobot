using System.Data.SqlClient;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace CatalotecaInsertionRobot
{
  class Program
  {   
        private const string tablename = "rawdata";

        public static void Main()
        {
            Console.WriteLine("Robô de inserção Cataloteca");
            Console.WriteLine("-------------------------");

            // Input de dados
            Console.Write("Digite host do banco de dados (Default => localhost):");
            string server = Console.ReadLine();

            Console.Write("Digite o nome do banco de dados (Default => cataloteca):");
            string dbName = Console.ReadLine();

            Console.WriteLine("Digite o caminho completo para o arquivo:");
            string filePath = @Console.ReadLine();

            string stringConnection = GetStringConnection(server, dbName);
            
            // Iniciando count de tempo de processamento
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine("-------------------------");
            Console.WriteLine("Iniciando leitura da planilha...");
            var dt = GetDataTableFromExcel(filePath);

            Console.WriteLine("Iniciando Inserção...");
            InsertToSQLUsingSQLBulk(dt, stringConnection, tablename);


            Console.WriteLine("-------------------------");
            // Terminando count de tempo de processamento
            Console.WriteLine($"Tempo de processamento: {stopwatch.ElapsedTicks}");
            stopwatch.Stop();
            Console.WriteLine("Fim!");

        }

        public static string GetStringConnection(string server, string dbName)
        {
            server = server == "" ? "localhost" : server;
            dbName = dbName == "" ? "cataloteca" : dbName;
            string stringConnection = $"Server={server}; Initial Catalog = {dbName}; integrated security=true";
            return stringConnection;
        }

        public static DataTable GetDataTableFromExcel(string path, bool hasHeader = true)
        {
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                // Abre arquivo
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }

                // Pega primeira planilha
                var ws = pck.Workbook.Worksheets.First();

                // Define objeto de database
                DataTable database = new DataTable();


                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    database.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }

                // Se tiver Header a primeira linha será a 2
                var startRow = hasHeader ? 2 : 1;

                // For em cada linha da planilha a partir da primeira
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    try
                    {
                        var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                        DataRow row = database.Rows.Add();
                        foreach (var cell in wsRow)
                        {
                            row[cell.Start.Column - 1] = cell.Text;
                        }
                    } catch (Exception ex)
                    {
                        Console.WriteLine($"Erro na linha {rowNum}");
                        Console.WriteLine(ex);
                        
                    }

                }
                
                return database;
            }
        }

        public static void InsertToSQLUsingSQLBulk(DataTable dt, string connectionstring, string Tablename)
        {

            try
            {
                using (var bulkCopy = new SqlBulkCopy(connectionstring, SqlBulkCopyOptions.KeepIdentity))
                {

                    //bulkCopy.BulkCopyTimeout = 6000;
                    var name = new SqlBulkCopyColumnMapping("Name", "Name");
                    bulkCopy.ColumnMappings.Add(name);

                    var longDescription = new SqlBulkCopyColumnMapping("Description", "Description");
                    bulkCopy.ColumnMappings.Add(longDescription);

                    bulkCopy.DestinationTableName = Tablename;
                    bulkCopy.WriteToServer(dt);
                    Console.WriteLine("-------------------------");
                    Console.WriteLine("Linhas Inseridas:");
                    Console.WriteLine(dt.Rows.Count);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Não foi possível concluir a tarefa");
                Console.WriteLine(ex);
                throw ex;
            }
        }


    }
}
