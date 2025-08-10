using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTaskSystem.Models.DBs
{
    using MySql.Data.MySqlClient;
    class CommonDBsUtility
    {
        public void CreateTable(string createTableSql, string connectionString)
        {

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = new MySqlCommand(createTableSql, connection))
                    {
                        connection.Open();
                        System.Diagnostics.Debug.WriteLine(createTableSql + " のテーブルが存在しなかったので、作成しました。");
                        command.CommandText = createTableSql;
                        command.ExecuteNonQuery();

                    }

                    connection.Close();
                }



            }
            catch (MySqlException me)
            {
                System.Diagnostics.Debug.WriteLine("ERROR: " + me.Message);
            }

        }
    }
}
