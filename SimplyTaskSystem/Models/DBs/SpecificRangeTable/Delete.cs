using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimplyTaskSystem.Models.DBs.SpecificRangeTable
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
        public void DeleteRecordsUnderConditionsPeriodically()
        {



            try
            {



                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        connection.Open();

                        var specificRangeTableRead = new SpecificRangeTable.Read();
                        //3か月経過したタスクを全取得
                        var Collection = specificRangeTableRead.SelectSpecificRangeCollection("select * from simpletasksystem.SpecificRange where created_at < DATE_ADD(CURRENT_DATE(), INTERVAL - 90 day)");


                        //上記で取得した3か月経過後のレコードを全部削除
                        command.CommandText = "DELETE FROM simpletasksystem.SpecificRange where ID=@ID";

                        foreach (var ele in Collection)
                        {
                            command.Parameters.Add(new MySqlParameter("@ID", ele.ID));
                            command.ExecuteNonQuery();
                            command.Parameters.Clear();
                        }

                        //削除したレコード数（＞０）のとき、InsertMethodの実行（レコード数は削除数に関係なく追加する仕様にとりましておく問題あるなら、削除数に合わせて追加も検討。※同時にInsertすることしか基本的にしない予定なのでNP?）
                        if (Collection.Count > 0)
                        {
                            MessageBox.Show("SpecificTableの中で、期限切れレコードが  " + Collection.Count + " 数 削除されました。");
                            var specificRangeTableInsert = new SpecificRangeTable.Insert();
                            specificRangeTableInsert.InsertInitialRecords();



                        }
                        //初期化＝＞一番最初のStartUp: レコード数が０なのでInsert
                        Collection = new System.Collections.ObjectModel.ObservableCollection<DataClass>();
                        Collection = specificRangeTableRead.SelectSpecificRangeCollection("select * from simpletasksystem.SpecificRange");
                        if (Collection.Count == 0)
                        {
                            MessageBox.Show("SpecificRangeテーブルのレコード数が０なので、Insertします。");
                            var specificRangeTableInsert = new SpecificRangeTable.Insert();
                            specificRangeTableInsert.InsertInitialRecords();



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

