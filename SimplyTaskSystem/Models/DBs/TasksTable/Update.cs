using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimplyTaskSystem.Models.DBs.TasksTable
{

    

    class Update
    {
        private readonly string connectionString;
        public Update()
        {
            // MySQLへの接続情報
            InitialSettingsInformation initialSettingsInformation = new InitialSettingsInformation();
            this.connectionString = initialSettingsInformation.ConnectionString;
        }

        public void UpdateTasksRecord(string commandText)
        {



            try
            {



                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = commandText;



                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
                    }

                    connection.Close();

                }



            }
            catch (MySqlException me)
            {
                Debug.WriteLine("ERROR: " + me.Message);
                MessageBox.Show(me.Message);
            }

        }

        public void UpdateTasksRecordOfResult(string dd, int ctit, int ctoot, string lca, int id)
        {
            try
            {



                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = "UPDATE simpletasksystem.tasks SET due_date=@DD, clear_times_intime=@CTIT, clear_times_outoftime=@CTOOT, lastcleared_at=@LCA where id=@ID";
                        command.CommandTimeout = 99999;

                        command.Parameters.Add(new MySqlParameter("@DD", dd));
                        command.Parameters.Add(new MySqlParameter("@CTIT", ctit));
                        command.Parameters.Add(new MySqlParameter("@CTOOT", ctoot));
                        command.Parameters.Add(new MySqlParameter("@LCA", lca));
                        command.Parameters.Add(new MySqlParameter("@ID", id));
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
                    }

                    connection.Close();

                }



            }
            catch (MySqlException me)
            {
                Debug.WriteLine("ERROR: " + me.Message);
                MessageBox.Show(me.Message);
            }

        }

    }
}
