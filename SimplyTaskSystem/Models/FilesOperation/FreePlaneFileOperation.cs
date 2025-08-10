using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTaskSystem.Models.FilesOperation
{
    using MeCab;
    /*Concept=>
*  外部から、該当タスクレコードの型をコンストラクタで取得し、加工しFileを作成=>開く、 までのプロセスを実行する。
* 
* 
*/
    using SimplyTaskSystem.Models;
    using System.Diagnostics;
    using System.IO;
    using System.Windows;
    using System.Xml;

    class FreePlaneFileOperation
    {
        private readonly string MMFsFolderPath;//固定対象フォルダー
        private readonly Models.DBs.TasksTable.DataClass tasksDataClass;

        public FreePlaneFileOperation(Models.DBs.TasksTable.DataClass dataClass)
        {

        }

        public FreePlaneFileOperation()
        {
            MMFsFolderPath = Utility.GetPathRelation.GetCurrentAppDir() + @"\MMFs\";
            Debug.WriteLine("MMFsFolderPath: " + MMFsFolderPath);
        }



        private string CreateFreePlaneFileWithNoDuplicateInSpecificFolder(Models.DBs.TasksTable.DataClass dataClass, bool autoGenerationBranch=false)
        {
            /*第2引数の対象レコードにより、ファイル名の基礎を決定し、
             * 第1引数Folder内で再帰的に評価し、重複ファイル名が存在しないように、適当な文字列を添えて戻り値で返す。
             * 
             * 
             */

            //MMfsFolder内にまず再帰的にも重複ファイル名がないかCheckし、あれば、エラーを返す。
            Utility.GetPathRelation getPathRelation = new Utility.GetPathRelation();
            getPathRelation.GetFileNamesRecursiveList(MMFsFolderPath);

            //ここまでこれたなら、上記の検問は無事にpassできたということ。
            //ここから、xmlFileを生成していく。
            return "";
        }

        private Models.DBs.TasksTable.DataClass dataClass;

        public void OpenSpecificFreePlaneFile(string freePlaneFile)
        {
            //該当のファイルを開くとき、そのファイルが万が一なかったら、おかしいので、Try-Catchの記述かな
            string str = "";
            try
            {
                //File Open With FreePlane.exe
                Utility.GetPathRelation getPathRelation = new Utility.GetPathRelation();
                str = getPathRelation.GetFilePathofFolder(MMFsFolderPath, freePlaneFile);
                Process.Start(str); //

            }
            catch
            {
                MessageBox.Show(str + " がMMFsFolderにありません。");
            }
        }

        private void CreateAutoVariablesBaseOfVariablesFreePlaneFile()
        {
            /*VariablesTableへアクセスして、全レコードのID, Nameを取得？
             * =>その文字列値をもとに、MMFILENAMEを選定して、作成していく。
             * Tips: VariableTableのInsertされる値は、"TaskName", "FreePlaneの全ブランチなど）”そのあたりは、VariablesTableの問題だからそっちで記述しようかな。
             *          既に作成したものに関しては、どうしようかな。一意性にするのか、そうしないのか。ファイル数が過多になるのを防ぎたいなら、んー、ただ毎回その変数で
             *          自分が発想するとは限らないということを考えると、定期的にしてもいいのかなと思っているところはある。
             *          
             *          when ? :
             *              定期的にやればいいと思う。それこそ1か月おきとかでもあと、一意性を担保するかどうかだけど、どうだろ。しなくていいんじゃないかな。とりま。ある程度変数がたまってから
             *              1か月おきとかでいいかもしれないね。そういうタスクレコード追加しておくのもいいかもしれない。
             */

            //VariablesTableから全レコードを取得して、その値を対象テーブルのDataClass等に格納して、取得。

            //上記の値群を、もとに、組み合わせ漏れがないように、mmFileを作成していく。(for, While, contains etcを必要あらば使用して）

        }

        private void CreateAutoVariablesBaseOfTasksRecordFreePlaneFile()
        {
            /*Record情報をもとに、一意性のファイルを生成していく。
             * when?: dummyTasksTableからSelectしてというのを、UIのBtnCommandなどからやればいいかな。
             * 
             * 
             */
        }

        private void CreateBranchesFromFilledInGenerator(string text)
        {
            //textの文字列を形態素解析などをして、自動的にブランチを割り振る。あと、先にMMFileを名前決めてあるフォルダー内で再帰的に重複しないのをCheckしてから。
        }


        public void newCreateXml(string KMTName, string HTLName, string KMNName,int actionID,int rootNode = 1, bool instantlyAddToDB = false, int priority = 3, int periodically_cycles = 1)
        {
            int count = 1;

            //事前にＭＭＦＩＬＥ作成前のファイル数所得のためwhileLoopの外で宣言して初期化している。
            Utility.GetPathRelation getPathRelation = new Utility.GetPathRelation();

            //ＭＭＦＩＬＥ作成前のファイル数を取得しておく。
            var MMFsFilesCount = getPathRelation.getMMFsDirectoryFilesCount();

            while (true)
            {
                //MMFsFolderPathをGet string
                string folderPath = MMFsFolderPath;
                Debug.WriteLine("実行パス＋MMFsFolderのパスは： " + folderPath);

                //File生成日時の取得（For: 一意性）
                DateTime dt = DateTime.Now;
                string created = dt.ToString("yyyyMMddJJmmss") + dt.Millisecond.ToString();

                string fileName = KMTName + "_" + KMNName + "_" + HTLName + "_" + count + "_" + created + "_"  + ".mm";
                //MessageBox.Show(fileName);


                //windowsFileNameとして不適当な文字列をアンダースコアに変換
                var invalidChars = Path.GetInvalidFileNameChars();
                var converted = string.Concat(
                  fileName.Select(c => invalidChars.Contains(c) ? '_' : c));


                //widnowsFileNameとして不適当な文字数を削除(特に、webPage系は長くなりやすそう）
                if (converted.Length >= 200)
                {
                    Debug.WriteLine(converted + " が200文字以上のため、文字列をカットします。");
                    converted = converted.Substring(0, 150) + created + "_" + count + ".mm";
                    Debug.WriteLine("カット後の文字列： " + converted);

                }

                Debug.WriteLine("生成予定ファイル名： " + converted);

                //生成予定ファイル名が、MMfsFolderに再帰的にないかCheck
                var flag = getPathRelation.GetFileNamesRecursive(folderPath, converted);

                if (flag == true)//converted＝＞NodeCreateのプロセスに進む
                {

                    nodeCreateOnXml(converted, rootNode);
                    var convertedWithPath = getPathRelation.GetFilePathofFolder(folderPath, converted);
                    Debug.WriteLine("convertedWithPath: " + convertedWithPath);

                    System.Diagnostics.Process p =
                    System.Diagnostics.Process.Start(convertedWithPath);

                    if (instantlyAddToDB == false)
                    {
                        //DummyTasksTableにInsert
                        var dummyTasksTable = new Models.DBs.DummyTasksTable.Insert();
                        dummyTasksTable.InsertRecordFromAction(actionID, converted);

              
               
                    }

                    //break直前つまりは、MMFILEが作成されてから評価している。
                    if (MMFsFilesCount + 1 == getPathRelation.getMMFsDirectoryFilesCount())
                    {
                        Debug.WriteLine("上書きされることなくMMFsFolderにMMFileがnewCreateされました。");
                    }
                    else
                    {
                        Debug.WriteLine("大変です。既存のＭＭＦＩＬＥに新しいMMFileが上書きされ消失した可能性があります。");
                        MessageBox.Show("大変です。既存のＭＭＦＩＬＥに新しいMMFileが上書きされ消失した可能性があります。");
                    }
                    break;

                }
                else
                {
                    MessageBox.Show(fileName + " :というファイル名再帰的にありますよ！ミリ秒込みで生成しているので重複するなんて珍しい！！１％だろ！引き続きwhileLoopを繰り返し、重複ファイル名がなくなるまで、生成を行います。");
                }
                count++;
            }




        }


        private void nodeCreateOnXml(string fileName, int rootNode = 1, string optionalFolderPath = "")
        {

            /*【用途】
             * mmFileをRootNodeのみで作るだけ
             * File名の重複防止は、第一引数の文字列外部でMUSTで行うように！！！
             * 
             * 
             * 
             */
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode mapNode = doc.CreateElement("map");
            doc.AppendChild(mapNode);



            XmlNode nodeNode = doc.CreateElement("node");

            var rand = new Random();
            int value = rand.Next(minValue: 100000, maxValue: 999999999);

            DateTime dt = DateTime.Now;
            string created = dt.ToString("yyyyMMddJJmmss") + dt.Millisecond.ToString();
            Console.WriteLine(created);



            XmlAttribute nodeAttribute = doc.CreateAttribute("CREATED");
            nodeAttribute.Value = created;
            nodeNode.Attributes.Append(nodeAttribute);

            nodeAttribute = doc.CreateAttribute("ID");
            nodeAttribute.Value = value.ToString();
            nodeNode.Attributes.Append(nodeAttribute);

            nodeAttribute = doc.CreateAttribute("MODIFIED");
            nodeAttribute.Value = created;
            nodeNode.Attributes.Append(nodeAttribute);

            nodeAttribute = doc.CreateAttribute("TEXT");
            nodeAttribute.Value = fileName;//RootNodeなので、ファイル名をRootNodeTextに代入している
            nodeNode.Attributes.Append(nodeAttribute);

            if (rootNode == 1)
            {
                //RootNodeなので、円形にしておくcentral Image
                nodeAttribute = doc.CreateAttribute("STYLE");
                nodeAttribute.Value = "oval";
                nodeNode.Attributes.Append(nodeAttribute);
            }

            mapNode.AppendChild(nodeNode);

            string folderPath = MMFsFolderPath + optionalFolderPath;








            doc.Save(Console.Out);

            //MessageBox.Show(fileName);




            doc.Save(folderPath + fileName);


            Console.ReadLine();
        }


        public string createXmlFileWithContextAnylysis(int actionID, string objectText = "", string fileName = "MiscellaneousNotes", int rootNode = 1, int afterChildNode = 1)
        {
            /*when used?:
             * OCROfPDF, DirectlyOfWebPage,etc taskAction画面
             * fileName: "KMT_KMN_HTL_"末尾アンダースコアなし？
             */

            string folderPath = MMFsFolderPath;
            fileName += "_AutoCreatedBranchesByMorphologicalAnalysis";
            string buckUpFileNmae = fileName;

            Utility.GetPathRelation getPathRelation = new Utility.GetPathRelation();



            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode mapNode = doc.CreateElement("map");
            doc.AppendChild(mapNode);



            XmlNode nodeNode = doc.CreateElement("node");

            var rand = new Random();
            int value = rand.Next(minValue: 100000, maxValue: 999999999);

            DateTime dt = DateTime.Now;
            string created = dt.ToString("yyyyMMddJJmmss") + dt.Millisecond.ToString();
            Console.WriteLine(created);



            XmlAttribute nodeAttribute = doc.CreateAttribute("CREATED");
            nodeAttribute.Value = created;
            nodeNode.Attributes.Append(nodeAttribute);

            nodeAttribute = doc.CreateAttribute("ID");
            nodeAttribute.Value = value.ToString();
            nodeNode.Attributes.Append(nodeAttribute);

            nodeAttribute = doc.CreateAttribute("MODIFIED");
            nodeAttribute.Value = created;
            nodeNode.Attributes.Append(nodeAttribute);

            nodeAttribute = doc.CreateAttribute("TEXT");
            nodeAttribute.Value = fileName;//RootNodeなので、ファイル名をRootNodeTextに代入している
            nodeNode.Attributes.Append(nodeAttribute);

            if (rootNode == 1)
            {
                //RootNodeなので、円形にしておくcentral Image
                nodeAttribute = doc.CreateAttribute("STYLE");
                nodeAttribute.Value = "oval";
                nodeNode.Attributes.Append(nodeAttribute);
            }

            mapNode.AppendChild(nodeNode);

            if (afterChildNode == 1)
            {


                //多次元リストを受け取る
                var list = this.ExtractionNoun(objectText);

                for (int i = 0; i < list.Count; i++)//多次元リスト（1文ずつ）Repeat
                {


                    /////ここまでがRootNodeに関する処理/////////////

                    XmlNode[] xmlNodes = new XmlNode[list[i].Count];//単語の数だけノード作成

                    for (int k = 0; k < list[i].Count; k++)//1文リスト内の単語repeat(parent-child)
                    {
                        xmlNodes[k] = doc.CreateElement("node");
                        //XmlNode nodeNode2 = doc.CreateElement("node");


                        nodeAttribute = doc.CreateAttribute("CREATED");
                        nodeAttribute.Value = created;
                        xmlNodes[k].Attributes.Append(nodeAttribute);

                        nodeAttribute = doc.CreateAttribute("ID");
                        nodeAttribute.Value = value.ToString();
                        xmlNodes[k].Attributes.Append(nodeAttribute);

                        nodeAttribute = doc.CreateAttribute("MODIFIED");
                        nodeAttribute.Value = created;
                        xmlNodes[k].Attributes.Append(nodeAttribute);

                        /*
                        nodeAttribute = doc.CreateAttribute("POSITION");
                        if (k == 0 && i % 2 == 0)
                        {
                            nodeAttribute.Value = "right";

                        }
                        else if (k == 0 && i % 2 == 1)
                        {
                            nodeAttribute.Value = "left";

                        }
                        xmlNodes[k].Attributes.Append(nodeAttribute);
                        */

                        nodeAttribute = doc.CreateAttribute("TEXT");
                        nodeAttribute.Value = list[i][k];
                        xmlNodes[k].Attributes.Append(nodeAttribute);

                        if (k == 0)
                        {
                            nodeNode.AppendChild(xmlNodes[k]);

                        }
                        else
                        {
                            xmlNodes[k - 1].AppendChild(xmlNodes[k]);

                        }
                    }


                    ////ここまでが子ブランチに関する処理////

                    if (i % 5 == 0)
                    {
                        //初期化
                        fileName = buckUpFileNmae;


                        //windowsFileNameとして不適当な文字列をアンダースコアに変換
                        var invalidChars2 = Path.GetInvalidFileNameChars();
                        fileName = string.Concat(
                          fileName.Select(c => invalidChars2.Contains(c) ? '_' : c));


                        //widnowsFileNameとして不適当な文字数を削除(特に、webPage系は長くなりやすそう）
                        if (fileName.Length >= 200)
                        {
                            Debug.WriteLine(fileName + " が200文字以上のため、文字列をカットします。");
                            fileName = fileName.Substring(0, 150) + created + ".mm";
                            Debug.WriteLine("カット後の文字列： " + fileName);

                        }


                        //FileName重複検知

                        dt = DateTime.Now;
                        created = dt.ToString("yyyyMMddJJmmss") + dt.Millisecond.ToString();
                        fileName += "_" + created + "_";

                        int counter1 = 1;
                        //while内において毎回初期化するために用意している
                        string trashFileName = fileName;
                        do
                        {
                            //初期化
                            trashFileName = fileName;

                            trashFileName += "_" + counter1 + ".mm";
                            counter1++;
                        }

                        while (!(getPathRelation.GetFileNamesRecursive(folderPath, trashFileName)));


                        fileName = trashFileName;


                        Debug.WriteLine("fileName: " + fileName);
                        Debug.WriteLine(" ; " + folderPath + fileName);
                        
                        doc.Save(Console.Out);
                        doc.Save(folderPath + fileName);


                        Console.ReadLine();

                        ////////////RootNode再び生成////////////////
                        doc = new XmlDocument();
                        docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                        doc.AppendChild(docNode);

                        mapNode = doc.CreateElement("map");
                        doc.AppendChild(mapNode);



                        nodeNode = doc.CreateElement("node");

                        rand = new Random();
                        value = rand.Next(minValue: 100000, maxValue: 999999999);

                        dt = DateTime.Now;
                        created = dt.ToString("yyyyMMddJJmmss") + dt.Millisecond.ToString();
                        Console.WriteLine(created);



                        nodeAttribute = doc.CreateAttribute("CREATED");
                        nodeAttribute.Value = created;
                        nodeNode.Attributes.Append(nodeAttribute);

                        nodeAttribute = doc.CreateAttribute("ID");
                        nodeAttribute.Value = value.ToString();
                        nodeNode.Attributes.Append(nodeAttribute);

                        nodeAttribute = doc.CreateAttribute("MODIFIED");
                        nodeAttribute.Value = created;
                        nodeNode.Attributes.Append(nodeAttribute);

                        nodeAttribute = doc.CreateAttribute("TEXT");
                        nodeAttribute.Value = fileName;//RootNodeなので、ファイル名をRootNodeTextに代入している
                        nodeNode.Attributes.Append(nodeAttribute);

                        if (rootNode == 1)
                        {
                            //RootNodeなので、円形にしておくcentral Image
                            nodeAttribute = doc.CreateAttribute("STYLE");
                            nodeAttribute.Value = "oval";
                            nodeNode.Attributes.Append(nodeAttribute);
                        }

                        mapNode.AppendChild(nodeNode);


                    }





                }









            }





            //初期化
            fileName = buckUpFileNmae;

            //FileName重複検知
            fileName += "_" + created + "_";

            int counter2 = 1;
            //while内において毎回初期化するために用意している
            string trashFileName2 = fileName;
            do
            {
                //初期化
                trashFileName2 = fileName;

                trashFileName2 += counter2 + ".mm";
                counter2++;
            }
            while (!(getPathRelation.GetFileNamesRecursive(folderPath, trashFileName2)));


            fileName = trashFileName2;

            //windowsFileNameとして不適当な文字列をアンダースコアに変換
            var invalidChars = Path.GetInvalidFileNameChars();
            fileName = string.Concat(
              fileName.Select(c => invalidChars.Contains(c) ? '_' : c));


            //widnowsFileNameとして不適当な文字数を削除(特に、webPage系は長くなりやすそう）
            if (fileName.Length >= 200)
            {
                Debug.WriteLine(fileName + " が200文字以上のため、文字列をカットします。");
                fileName = fileName.Substring(0, 150) + created + ".mm";
                Debug.WriteLine("カット後の文字列： " + fileName);

            }


            doc.Save(Console.Out);
            doc.Save(folderPath + fileName);

            //DummyTasksTableにInsert
            var dummyTasksTable = new Models.DBs.DummyTasksTable.Insert();
            dummyTasksTable.InsertRecordFromAction(actionID, fileName);

            Console.ReadLine();

            return fileName;    



        }


        private List<List<string>> ExtractionNoun(string sentence)
        {
            //第1引数の文字列を形態素解析して、1血統ごとにList<string>である程度の区切りで分けている

            //var sentence = "行く川のながれは絶えずして、しかももとの水にあらず。";

            /*
            var macroList = new List<string>();
            
            
            int stringLength = sentence.Length;

            if (stringLength > 2000)
            {
                for (int i = 1; i < 100; i++)
                {


                }
                macroList.Add(sentence.Substring(2001));

                int i = 0;
                while (true)
                {
                    int strCounter = i * 1000;
                    if (strCounter > stringLength)
                    {
                        break;
                    }
                    
                    if (stringLength > 2000)
                    {
                        macroList.Add(sentence.Substring(, ));

                    }
                    else

                    {
                        macroList.Add(sentence.Substring(1, stringLength));

                    }
                    i++;
                }
            }
            }*/
            var parameter = new MeCabParam();
            var tagger = MeCabTagger.Create(parameter);
            var list = new List<List<string>>();

            var splitSymbols = new List<string>() { "!", "?", "！", "？", "！？", "!?", ",", ".", "、", "。", ",", "，" };
            foreach (var split in splitSymbols)
            {
                Console.WriteLine(split);
            }

            int counter = 0;
            foreach (var node in tagger.ParseToNodes(sentence))
            {
                if (node.CharType > 0)
                {
                    var features = node.Feature.Split(',');
                    var displayFeatures = string.Join(", ", features);

                    if (splitSymbols.Contains(node.Surface))
                    {
                        counter++;
                    }
                    list.Add(new List<string>());
                    list[counter].Add(node.Surface);
                    Debug.WriteLine($"{node.Surface}\t{displayFeatures}");

                }
            }



            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine("list: " + i);
                foreach (var li in list[i])
                {
                    /*
                    if (list[i].Count == 0)
                    {
                        list.Remove(new List<string>());
                        list.Remove(list[i]);
                        list.RemoveAll(l => l.Count == 0);
                        

                    }
                    */
                    Debug.WriteLine("リスト内要素：" + li);

                }

            }

            list.RemoveAll(l => l.Count == 0);


            Console.WriteLine("Listの数は： " + list.Count);

            return list;
        }


    }
}
