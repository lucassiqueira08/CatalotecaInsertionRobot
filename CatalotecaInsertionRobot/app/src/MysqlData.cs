using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace CatalotecaInsertionRobot.app.src
{
    public class MysqlData
    {
        public static void InsertMySQL(string ConnectionString, List<ProductEntity> dt, string table)
        {
            int insertLines = 0;
            string query = $"INSERT INTO {table} (Id, Name, ShortDescription, LongDescription ) VALUES (@Id, @Name, @ShortDescription, @LongDescription);";
            using MySqlConnection mConnection = new MySqlConnection(ConnectionString);
            mConnection.Open();
            using MySqlTransaction trans = mConnection.BeginTransaction();
            using (MySqlCommand myCmd = new MySqlCommand(query, mConnection, trans))
            {
                myCmd.CommandType = CommandType.Text;
                for (int i = 0; i < dt.ToArray().Length; i++)
                {
                    var row = dt[i];
                    Console.WriteLine("---------");
                    Console.WriteLine($"Inserindo => {i} => {row.LongDescription}");
                    myCmd.Parameters.Clear();
                    myCmd.Parameters.AddWithValue("@Id", Guid.NewGuid());
                    myCmd.Parameters.AddWithValue("@Name", row.Name.ToString());
                    myCmd.Parameters.AddWithValue("@ShortDescription", row.ShortDescription.ToString());
                    myCmd.Parameters.AddWithValue("@LongDescription", row.LongDescription.ToString());
                    myCmd.ExecuteNonQuery();
                    insertLines++;
                }
                trans.Commit();
                Console.WriteLine("-------------------------");
                Console.WriteLine("Linhas Inseridas:");
                Console.WriteLine(insertLines + 1);
            }
        }
    }
}