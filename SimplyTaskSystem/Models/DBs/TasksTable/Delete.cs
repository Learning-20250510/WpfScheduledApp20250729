using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimplyTaskSystem.Models.DBs.TasksTable
{
    class Delete
    {
        private readonly string connectionString;
        public Delete()
        {
            // MySQLへの接続情報
            InitialSettingsInformation initialSettingsInformation = new InitialSettingsInformation();
            this.connectionString = initialSettingsInformation.ConnectionString;
        }
        public void DeleteRecordFromTasksTableByID(int ID)
        {


            // MySQLへの接続

            try
            {



                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        connection.Open();


                        command.CommandText = "DELETE FROM simpletasksystem.tasks where ID=@ID";
                        command.Parameters.Add(new MySqlParameter("@ID", ID));
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();




                        command.Parameters.Clear();



                    }

                    connection.Close();

                }



            }
            catch (MySqlException me)
            {
                Debug.WriteLine("ERROR: " + MethodBase.GetCurrentMethod().Name + me.Message);
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + me.Message);
            }

        }

    }
}
