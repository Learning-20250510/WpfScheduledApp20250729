using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace WpfScheduledApp20250729.Models.FilesOperation
{
    public class PDFFileOperation
    {
        /// <summary>
        /// PDFファイルの指定ページを開く
        /// </summary>
        /// <param name="pdfFileName">PDFファイル名</param>
        /// <param name="specificPageNumber">開くページ番号</param>
        public void OpenPDFSpecificPage(string pdfFileName, int specificPageNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(pdfFileName))
                {
                    MessageBox.Show("PDFファイル名が指定されていません。", "🎮 PDF ERROR");
                    return;
                }

                // PDFファイルを探す
                string filePath = FindPDFFile(pdfFileName);
                
                if (string.IsNullOrEmpty(filePath))
                {
                    MessageBox.Show($"PDFファイル '{pdfFileName}' が見つかりません。", "🎮 PDF NOT FOUND");
                    return;
                }

                // Adobe Acrobat Reader/Proで特定ページから開く
                if (TryOpenWithAcrobat(filePath, specificPageNumber))
                {
                    return;
                }

                // SumatraPDFで開く
                if (TryOpenWithSumatra(filePath, specificPageNumber))
                {
                    return;
                }

                // デフォルトPDFビューアーで開く
                OpenWithDefaultViewer(filePath, specificPageNumber);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"PDFファイルを開くときにエラーが発生しました: {ex.Message}", "🎮 PDF ERROR");
            }
        }

        /// <summary>
        /// PDFファイルを探す
        /// </summary>
        private string FindPDFFile(string fileName)
        {
            // 検索するフォルダ
            string[] searchFolders = {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PDFs"),
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Desktop"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"),
                AppDomain.CurrentDomain.BaseDirectory
            };

            foreach (var folder in searchFolders)
            {
                if (!Directory.Exists(folder)) continue;

                // 拡張子なしの場合は.pdfを追加
                string searchFileName = Path.HasExtension(fileName) ? fileName : fileName + ".pdf";
                string fullPath = Path.Combine(folder, searchFileName);
                
                if (File.Exists(fullPath))
                {
                    return fullPath;
                }

                // サブフォルダも検索
                try
                {
                    var files = Directory.GetFiles(folder, searchFileName, SearchOption.AllDirectories);
                    if (files.Length > 0)
                    {
                        return files[0];
                    }
                }
                catch
                {
                    // 権限エラーなどは無視
                }
            }

            return null;
        }

        /// <summary>
        /// Adobe Acrobat Reader/Proで特定ページから開く
        /// </summary>
        private bool TryOpenWithAcrobat(string filePath, int pageNumber)
        {
            try
            {
                // Adobe Acrobatの一般的なインストールパス
                string[] acrobatPaths = {
                    @"C:\Program Files\Adobe\Acrobat DC\Acrobat\Acrobat.exe",
                    @"C:\Program Files (x86)\Adobe\Acrobat DC\Acrobat\Acrobat.exe",
                    @"C:\Program Files\Adobe\Reader DC\Reader\AcroRd32.exe",
                    @"C:\Program Files (x86)\Adobe\Reader DC\Reader\AcroRd32.exe"
                };

                foreach (var acrobatPath in acrobatPaths)
                {
                    if (File.Exists(acrobatPath))
                    {
                        var startInfo = new ProcessStartInfo
                        {
                            FileName = acrobatPath,
                            Arguments = $"/A \"page={pageNumber}\" \"{filePath}\"",
                            UseShellExecute = false
                        };

                        Process.Start(startInfo);
                        MessageBox.Show($"🎮 Adobe Acrobat起動: ページ{pageNumber}から表示", "PDF VIEWER");
                        return true;
                    }
                }
            }
            catch
            {
                // Acrobatでの起動に失敗した場合は次の方法を試す
            }

            return false;
        }

        /// <summary>
        /// SumatraPDFで特定ページから開く
        /// </summary>
        private bool TryOpenWithSumatra(string filePath, int pageNumber)
        {
            try
            {
                // SumatraPDFの一般的なインストールパス
                string[] sumatraPaths = {
                    @"C:\Program Files\SumatraPDF\SumatraPDF.exe",
                    @"C:\Program Files (x86)\SumatraPDF\SumatraPDF.exe"
                };

                foreach (var sumatraPath in sumatraPaths)
                {
                    if (File.Exists(sumatraPath))
                    {
                        var startInfo = new ProcessStartInfo
                        {
                            FileName = sumatraPath,
                            Arguments = $"-page {pageNumber} \"{filePath}\"",
                            UseShellExecute = false
                        };

                        Process.Start(startInfo);
                        MessageBox.Show($"🎮 SumatraPDF起動: ページ{pageNumber}から表示", "PDF VIEWER");
                        return true;
                    }
                }
            }
            catch
            {
                // SumatraPDFでの起動に失敗した場合は次の方法を試す
            }

            return false;
        }

        /// <summary>
        /// デフォルトPDFビューアーで開く
        /// </summary>
        private void OpenWithDefaultViewer(string filePath, int pageNumber)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });

                MessageBox.Show($"🎮 PDFを開きました。\n手動でページ{pageNumber}に移動してください。", "PDF VIEWER");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"デフォルトビューアーでPDFを開けませんでした: {ex.Message}", "🎮 VIEWER ERROR");
            }
        }

        /// <summary>
        /// PDFファイルの総ページ数を取得（将来的な機能拡張用）
        /// </summary>
        public int GetTotalPagesOfPDF(string pdfFileName)
        {
            // TODO: PDFライブラリを使用してページ数を取得する実装
            // 現在は仮実装
            return 100; // 仮の値
        }
    }
}