using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTaskSystem.Models.DBs.URLForScrapingTable
{
    class Insert
    {
        private readonly string connectionString;

        public Insert()
        {
            // MySQLへの接続情報
            InitialSettingsInformation initialSettingsInformation = new InitialSettingsInformation();
            this.connectionString = initialSettingsInformation.ConnectionString;
        }
        public void InsertRecords()
        {



            try
            {



                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        connection.Open();


                        command.CommandText = "INSERT INTO simpletasksystem.urlforscraping (id, url) VALUES (@ID, @URL)";
                        //command.ExecuteNonQuery();

                        var list = new List<string>()
                        {
                            "https://www.google.com/",
                            "https://www.yahoo.co.jp/",
                            "https://teratail.com/",
                            "https://qiita.com/",
                            "https://chiebukuro.yahoo.co.jp/",
                            "https://stackoverflow.com/",
                            "https://www.bing.com/",
                            "https://www.youtube.com/",
                            "https://www.pinterest.jp/",
                            "https://vimeo.com/jp/",
                            "https://www.dailymotion.com/jp",
                        };

                        for (int i = 1; i <= list.Count; i++)
                        {
                            command.Parameters.Add(new MySqlParameter("@ID", i));
                            command.Parameters.Add(new MySqlParameter("@URL", list[i - 1]));
                            command.ExecuteNonQuery();
                            command.Parameters.Clear();
                        }




                        command.Parameters.Clear();



                    }

                    connection.Close();

                }



            }
            catch (MySqlException me)
            {
                Debug.WriteLine("ERROR: " + me.Message);
                //MessageBox.Show(me.Message);
            }

        }

    }
}
