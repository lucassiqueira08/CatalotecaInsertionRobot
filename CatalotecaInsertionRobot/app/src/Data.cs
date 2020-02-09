using System.Data.SqlClient;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace CatalotecaInsertionRobot.src.Data
{
  public class Data
  {
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