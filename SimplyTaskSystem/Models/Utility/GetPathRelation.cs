using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Reflection;

namespace SimplyTaskSystem.Models.Utility
{

    class GetPathRelation
    {
        public static string GetCurrentAppDir()
        {
            /*アプリケーションの実行FolderPathの取得
             * 
             */
            return System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location);
        }



        public bool GetFileNamesRecursive(string folderpath, string filename)


        /*
           * 【用途】
           * あるフォルダ内で再帰的にも拡張子含め同じファイル名が存在していないかCheck
           * 
           * 
          */

        {

            //"C:\test"以下のファイルをすべて取得する p.s. mm_fileの保管場所ジャンルごとにわけてるならその一番上のmmfolderのpathを記載

            IEnumerable<string> files =
                System.IO.Directory.EnumerateFiles(
                    @folderpath, "*", System.IO.SearchOption.AllDirectories);//後からprojectFolder名からの相対パスに変換したい

            //ファイルを列挙する
            List<string> list1 = new List<string>();
            list1 = new List<string>();
            List<string> list2 = new List<string>();



            foreach (string f in files)//folderpathから再帰的に全ファイルのpath取得
            {
                list1.Add(f);
                string gfn1 = Path.GetFileName(f);//拡張子付き
                Console.WriteLine(f);//再帰的にpath列挙される
                Console.WriteLine(gfn1);//filenameP.S. 再帰的対応してるつまり階層関係なくfile nameだけ取得できること確認済みあとドット記号filenameあってもＮＰ確認済み


                if (!list2.Contains(gfn1))
                {
                    list2.Add(gfn1);

                }
                else
                {
                    MessageBox.Show("追加前に既に同じファイル名が再帰的に" + folderpath + "にありました");
                }




            }

            if (!list2.Contains(filename))
            {
                //MessageBox.Show(folderpath + "内では再帰的ですらまだ使用されていないファイル名です。ご安心ください");
                return true;

            }
            else
            {
                MessageBox.Show("警告: " + folderpath + "内で再帰的のどこかで既にそのファイル名" + filename + "は使用されています。");//Review系、openFIle系ならこれが出てくれないと困るwhy?だって該当のファイル名があるってことだから
                return false;
            }


        }


        public List<string> GetFileNamesRecursiveList(string folderpath)


        /*
           * 【用途】
           * あるフォルダ内で再帰的にも拡張子含め同じファイル名が存在していないかCheckしてなかったら全ファイル取得
           * 
           * 
          */

        {

            //"C:\test"以下のファイルをすべて取得する p.s. mm_fileの保管場所ジャンルごとにわけてるならその一番上のmmfolderのpathを記載

            IEnumerable<string> files =
                System.IO.Directory.EnumerateFiles(
                    @folderpath, "*", System.IO.SearchOption.AllDirectories);//後からprojectFolder名からの相対パスに変換したい

            //ファイルを列挙する
            List<string> list1 = new List<string>();
            list1 = new List<string>();
            List<string> list2 = new List<string>();



            foreach (string f in files)//folderpathから再帰的に全ファイルのpath取得
            {
                list1.Add(f);
                string gfn1 = Path.GetFileName(f);//拡張子付き
                Debug.WriteLine("filePath: " + f);//再帰的にpath列挙される
                Debug.WriteLine("fileName: " + gfn1);//filenameP.S. 再帰的対応してるつまり階層関係なくfile nameだけ取得できること確認済みあとドット記号filenameあってもＮＰ確認済み


                if (!list2.Contains(gfn1))
                {
                    list2.Add(gfn1);

                }
                else
                {
                    //MessageBox.Show(gfn1 + " という同じファイル名が再帰的に" + folderpath + "にありました");
                    throw new Exception(gfn1 + " という同じファイル名が再帰的に" + folderpath + "にありました");
                }




            }
            return list2;



        }

        public string GetFilePathofFolder(string folderpath, string filename)


        /*when used?
         * Review系, Memo系などでMmFile作成されるときそのMm file名が再帰的にも重複して作られないように
         * 
         * 
         * 
        */

        {

            //"C:\test"以下のファイルをすべて取得する p.s. mm_fileの保管場所ジャンルごとにわけてるならその一番上のmmfolderのpathを記載

            IEnumerable<string> files =
                System.IO.Directory.EnumerateFiles(
                    @folderpath, "*", System.IO.SearchOption.AllDirectories);//後からprojectFolder名からの相対パスに変換したい

            //ファイルを列挙する
            List<string> list1 = new List<string>();
            //list1 = new List<string>();
            List<string> list2 = new List<string>();


            string returnString1 = "";

            foreach (string f in files)//folderpathから再帰的に全ファイルのpath取得
            {
                list1.Add(f);//拡張子付き絶対？Path
                string gfn1 = Path.GetFileName(f);//拡張子付き

                if (gfn1 == filename)
                {
                    returnString1 = f;

                }

            }
            return returnString1;

        }


        public int getMMFsDirectoryFilesCount()
        {
            var methodName = MethodBase.GetCurrentMethod().Name;
            Debug.WriteLine("Start: " + methodName);

            var MMFsFolderPath = GetCurrentAppDir() + @"\MMFs\";
            Debug.WriteLine("MMFsFolderPath: " + MMFsFolderPath);

            string[] files = Directory.GetFiles(MMFsFolderPath, "*", SearchOption.AllDirectories);
            Debug.WriteLine("files.Length: " + files.Length);

            return files.Length;









            Debug.WriteLine("End: " + methodName);
        }
















    }

}
