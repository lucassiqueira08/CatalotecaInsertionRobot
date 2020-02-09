using System.Data.SqlClient;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Diagnostics;
using CatalotecaInsertionRobot.src.Utils;
using CatalotecaInsertionRobot.src.Data;

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
      Console.Write("Digite qual o gerenciador de banco de dados (1-MySQL, 2-SQLServer):");
      string sgbd = Console.ReadLine();

      Console.Write("Digite host do banco de dados (Default => localhost):");
      string server = Console.ReadLine();

      Console.Write("Digite o nome do banco de dados (Default => cataloteca):");
      string dbName = Console.ReadLine();

      Console.WriteLine("Digite o caminho completo para o arquivo:");
      string filePath = @Console.ReadLine();

      string stringConnection = Utils.GetStringConnection(sgbd, server, dbName);

      // Iniciando count de tempo de processamento
      var stopwatch = new Stopwatch();
      stopwatch.Start();

      Console.WriteLine("-------------------------");
      Console.WriteLine("Iniciando leitura da planilha...");
      var dt = Utils.GetDataTableFromExcel(filePath);

      Console.WriteLine("Iniciando Inserção...");
      Data.InsertToSQLUsingSQLBulk(dt, stringConnection, tablename);


      Console.WriteLine("-------------------------");
      // Terminando count de tempo de processamento
      Console.WriteLine($"Tempo de processamento (Em segundos): {stopwatch.Elapsed.TotalSeconds}");
      stopwatch.Stop();
      Console.WriteLine("Fim!");

    }

  }
}
