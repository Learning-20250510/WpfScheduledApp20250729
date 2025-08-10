using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimplyTaskSystem.Models.DBs.KMTTable
{
    class Read
    {
        private readonly string connectionString;
        public Read()
        {
            // MySQLへの接続情報
            InitialSettingsInformation initialSettingsInformation = new InitialSettingsInformation();
            this.connectionString = initialSettingsInformation.ConnectionString;
        }
        public ObservableCollection<DataClass> SelectKMTCollection(string commandtext)
        {


            // MySQLへの接続

            ObservableCollection<DataClass> KMTCollection = new ObservableCollection<DataClass>();

            try
            {



                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        connection.Open();


                        command.CommandText = commandtext;
                        command.ExecuteNonQuery();




                        using (var reader = command.ExecuteReader())
                        {
                            // コンソールに表示
                            while (reader.Read())
                            {
                                KMTCollection.Add(new DataClass { ID = int.Parse(reader["id"].ToString()), KMTName = reader["kmt"].ToString(), CreatedAt = reader["created_at"].ToString() });
                            }
                        }

                        command.Parameters.Clear();



                    }

                    connection.Close();
                    return KMTCollection;

                }



            }
            catch (MySqlException me)
            {
                Debug.WriteLine("ERROR: " + me.Message);
                MessageBox.Show(me.Message);
                return KMTCollection;
            }

        }

    }
}
