using System.Data.SqlClient;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace CatalotecaInsertionRobot.src.Utils
{
  public class Utils
  {
    public static string GetStringConnection(string sgbd, string server, string dbName)
    {
      server = server == "" ? "localhost" : server;
      dbName = dbName == "" ? "cataloteca" : dbName;
      if (sgbd == "1") //Mysql
      {
        string stringConnection = $"Host={server};Database={dbName};";
      }
      else
      { // SQLSERVER
        string stringConnection = $"Server={server}; Initial Catalog = {dbName}";
      }
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
          }
          catch (Exception ex)
          {
            Console.WriteLine($"Erro na linha {rowNum}");
            Console.WriteLine(ex);

          }

        }
        return database;
      }
    }

  }
}