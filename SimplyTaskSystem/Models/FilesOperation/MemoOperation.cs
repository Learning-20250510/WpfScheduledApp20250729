using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimplyTaskSystem.Models.FilesOperation
{
    class MemoOperation
    {


        public List<string> ReadToEndFile(int priority, bool AllLinesDelete = false)
        {
            //ファイル名
            string InAFewDaysMemo = Utility.GetPathRelation.GetCurrentAppDir() + @"\Memo\" + "PeriodicallyCyclesFileName.txt";
            string Want1stMemo = Utility.GetPathRelation.GetCurrentAppDir() + @"\Memo\" + "Want1stMemo.txt";
            string Want2ndMemo = Utility.GetPathRelation.GetCurrentAppDir() + @"\Memo\" + "Want2ndMemo.txt";

            //読み込むテキストを保存する変数
            var text = "";

            string fileName = "";
            if ( priority == 3)
            {
                fileName = InAFewDaysMemo;

            }
            else if (priority == 4)
            {
                fileName = Want1stMemo;

            }
            else if ( priority == 5)
            {
                fileName = Want2ndMemo;

            }

            else
            {
                throw new Exception("正しいPriorityを選択してください。");
            }

            var list = new List<string>();

            try
            {
                //ファイルをオープンする
                using (StreamReader sr = new StreamReader(fileName, Encoding.GetEncoding("Shift_JIS")))
                {
             
                    var validateString = new Utility.ValidateString();


                    while ((text = sr.ReadLine()) != null)
                    {
                        // コンソールに読み込んだ文字列の内容を表示します。重複防止
                        Console.WriteLine("WEBPAGE?_TEXT: " + text);
                        if (!(list.Contains(text)))
                        {
                            if (validateString.IsUrl_1(text) == true || validateString.IsUrl_2(text) == true)
                            {
                                list.Add(text);
                                System.Diagnostics.Debug.WriteLine("URLでした" + text);
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("URLではなかったです。" + text);
                                //MessageBox.Show(text + " はＵＲＬではなかったです。");

                                /*
                                if (MessageBox.Show("どうしましょ？削除しますか？", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.No)
                                {
                                    string emptyLine = "";
                                    //sw.WriteLine(emptyLine);
                                }
                                else
                                {
                                    MessageBox.Show(text + " は削除されませんでした。手動で削除するかどうか検討してください");

                                }
                                */




                            }

                        }
                        else
                        {
                            //MessageBox.Show("既に、" + text + " が含まれています。");
                        }

                    }



                }
                if (AllLinesDelete == true)//FIle中身全削除（使ううえで注意）
                {
                    using (var fileStream = new FileStream(fileName, FileMode.Open))
                    {
                        fileStream.SetLength(0);
                        MessageBox.Show("Fileを削除しました。");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("File関係処理： " + ex.Message);
            }

            Debug.WriteLine(text);
            return list;
        }


        private void WriteLineToFile(string fileName)
        {
            using (var sw = new StreamWriter(fileName))
            {

            }
        }


    }
}
