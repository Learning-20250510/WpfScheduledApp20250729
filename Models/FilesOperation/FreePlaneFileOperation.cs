using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml;

namespace WpfScheduledApp20250729.Models.FilesOperation
{
    public class FreePlaneFileOperation
    {
        private readonly string _mmfsFolderPath;

        public FreePlaneFileOperation()
        {
            // MMFs フォルダのパスを設定（アプリケーションフォルダ/MMFs/）
            _mmfsFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MMFs");
            
            // フォルダが存在しない場合は作成
            if (!Directory.Exists(_mmfsFolderPath))
            {
                Directory.CreateDirectory(_mmfsFolderPath);
            }
            
            Debug.WriteLine($"MMFsFolderPath: {_mmfsFolderPath}");
        }

        /// <summary>
        /// 指定されたFreePlaneファイルを開く
        /// </summary>
        public void OpenSpecificFreePlaneFile(string freePlaneFileName)
        {
            try
            {
                string filePath = Path.Combine(_mmfsFolderPath, freePlaneFileName);
                
                if (File.Exists(filePath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = filePath,
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show($"{filePath} がMMFsFolderにありません。", "🎮 FILE NOT FOUND");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ファイルを開くときにエラーが発生しました: {ex.Message}", "🎮 ERROR");
            }
        }

        /// <summary>
        /// 基本的なMMファイルを作成
        /// </summary>
        public string CreateBasicMMFile(string kmtName, string kmn, string htlName, int actionId)
        {
            try
            {
                var fileName = GenerateUniqueFileName(kmtName, kmn, htlName);
                CreateXmlFile(fileName, fileName); // ファイル名をルートノードのテキストとして使用
                
                // ファイルを開く
                var fullPath = Path.Combine(_mmfsFolderPath, fileName);
                Process.Start(new ProcessStartInfo
                {
                    FileName = fullPath,
                    UseShellExecute = true
                });

                return fileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"MMファイル作成エラー: {ex.Message}", "🎮 MM FILE ERROR");
                return null;
            }
        }

        /// <summary>
        /// テキストを形態素解析してブランチを自動生成するMMファイルを作成
        /// </summary>
        public string CreateMMFileWithTextAnalysis(int actionId, string objectText, string baseFileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(objectText))
                {
                    MessageBox.Show("分析するテキストが空です。", "🎮 TEXT ANALYSIS ERROR");
                    return null;
                }

                var fileName = GenerateUniqueFileName(baseFileName, "AutoCreatedBranchesByTextAnalysis", "");
                
                // 基本的なXMLファイルを作成（今回はテキスト解析はシンプルに実装）
                CreateXmlFileWithBranches(fileName, objectText);

                MessageBox.Show($"🎮 MM FILE GENERATED!\n{fileName}", "SUCCESS");
                return fileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"テキスト解析MMファイル作成エラー: {ex.Message}", "🎮 ANALYSIS ERROR");
                return null;
            }
        }

        /// <summary>
        /// 重複しないファイル名を生成
        /// </summary>
        private string GenerateUniqueFileName(string kmtName, string kmn, string htlName)
        {
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var baseFileName = $"{SanitizeFileName(kmtName)}_{SanitizeFileName(kmn)}_{SanitizeFileName(htlName)}_{timestamp}";
            
            var counter = 1;
            var fileName = $"{baseFileName}_{counter}.mm";
            
            while (File.Exists(Path.Combine(_mmfsFolderPath, fileName)))
            {
                counter++;
                fileName = $"{baseFileName}_{counter}.mm";
            }
            
            return fileName;
        }

        /// <summary>
        /// ファイル名として無効な文字を置換
        /// </summary>
        private string SanitizeFileName(string fileName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            var sanitized = string.Concat(fileName.Select(c => invalidChars.Contains(c) ? '_' : c));
            
            // 長すぎる場合は短縮
            if (sanitized.Length > 50)
            {
                sanitized = sanitized.Substring(0, 50);
            }
            
            return sanitized;
        }

        /// <summary>
        /// 基本的なXMLファイル（MMファイル）を作成
        /// </summary>
        private void CreateXmlFile(string fileName, string rootNodeText)
        {
            var doc = new XmlDocument();
            var docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            var mapNode = doc.CreateElement("map");
            doc.AppendChild(mapNode);

            var nodeNode = doc.CreateElement("node");
            
            var random = new Random();
            var nodeId = random.Next(100000, 999999999);
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond;

            // ノード属性を設定
            SetNodeAttribute(doc, nodeNode, "CREATED", timestamp);
            SetNodeAttribute(doc, nodeNode, "ID", nodeId.ToString());
            SetNodeAttribute(doc, nodeNode, "MODIFIED", timestamp);
            SetNodeAttribute(doc, nodeNode, "TEXT", rootNodeText);
            SetNodeAttribute(doc, nodeNode, "STYLE", "oval");

            mapNode.AppendChild(nodeNode);

            var filePath = Path.Combine(_mmfsFolderPath, fileName);
            doc.Save(filePath);
        }

        /// <summary>
        /// テキストを元にブランチ付きのXMLファイルを作成
        /// </summary>
        private void CreateXmlFileWithBranches(string fileName, string objectText)
        {
            var doc = new XmlDocument();
            var docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            var mapNode = doc.CreateElement("map");
            doc.AppendChild(mapNode);

            var rootNode = doc.CreateElement("node");
            
            var random = new Random();
            var nodeId = random.Next(100000, 999999999);
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond;

            // ルートノード設定
            SetNodeAttribute(doc, rootNode, "CREATED", timestamp);
            SetNodeAttribute(doc, rootNode, "ID", nodeId.ToString());
            SetNodeAttribute(doc, rootNode, "MODIFIED", timestamp);
            SetNodeAttribute(doc, rootNode, "TEXT", fileName);
            SetNodeAttribute(doc, rootNode, "STYLE", "oval");

            mapNode.AppendChild(rootNode);

            // シンプルなテキスト解析（句読点で分割）
            var sentences = objectText.Split(new char[] { '。', '.', '!', '?', '！', '？' }, 
                                           StringSplitOptions.RemoveEmptyEntries);

            foreach (var sentence in sentences.Take(10)) // 最初の10文のみ
            {
                if (!string.IsNullOrWhiteSpace(sentence))
                {
                    var childNode = doc.CreateElement("node");
                    var childId = random.Next(100000, 999999999);
                    
                    SetNodeAttribute(doc, childNode, "CREATED", timestamp);
                    SetNodeAttribute(doc, childNode, "ID", childId.ToString());
                    SetNodeAttribute(doc, childNode, "MODIFIED", timestamp);
                    SetNodeAttribute(doc, childNode, "TEXT", sentence.Trim());

                    rootNode.AppendChild(childNode);
                    
                    // さらに単語レベルで分割（スペースと句読点で）
                    var words = sentence.Split(new char[] { ' ', '　', ',', '、' }, 
                                             StringSplitOptions.RemoveEmptyEntries);
                    
                    foreach (var word in words.Take(5)) // 各文から最初の5単語のみ
                    {
                        if (!string.IsNullOrWhiteSpace(word))
                        {
                            var wordNode = doc.CreateElement("node");
                            var wordId = random.Next(100000, 999999999);
                            
                            SetNodeAttribute(doc, wordNode, "CREATED", timestamp);
                            SetNodeAttribute(doc, wordNode, "ID", wordId.ToString());
                            SetNodeAttribute(doc, wordNode, "MODIFIED", timestamp);
                            SetNodeAttribute(doc, wordNode, "TEXT", word.Trim());

                            childNode.AppendChild(wordNode);
                        }
                    }
                }
            }

            var filePath = Path.Combine(_mmfsFolderPath, fileName);
            doc.Save(filePath);
        }

        /// <summary>
        /// XMLノードに属性を設定するヘルパーメソッド
        /// </summary>
        private void SetNodeAttribute(XmlDocument doc, XmlNode node, string attributeName, string attributeValue)
        {
            var attribute = doc.CreateAttribute(attributeName);
            attribute.Value = attributeValue;
            node.Attributes.Append(attribute);
        }

        /// <summary>
        /// MMFsフォルダ内のファイル数を取得
        /// </summary>
        public int GetMMFsDirectoryFilesCount()
        {
            try
            {
                return Directory.GetFiles(_mmfsFolderPath, "*.mm", SearchOption.AllDirectories).Length;
            }
            catch
            {
                return 0;
            }
        }
    }
}