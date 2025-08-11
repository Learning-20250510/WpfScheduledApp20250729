using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace WpfScheduledApp20250729.Models.FilesOperation
{
    public class WebPageOperation
    {
        /// <summary>
        /// Chromeで指定したWebページを開く
        /// </summary>
        /// <param name="webPageUrl">開くWebページのURL</param>
        public void OpenWebPageWithChrome(string webPageUrl)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(webPageUrl))
                {
                    MessageBox.Show("WebページのURLが指定されていません。", "🎮 WEBPAGE ERROR");
                    return;
                }

                // URLの前処理（httpプロトコルがない場合は追加）
                if (!webPageUrl.StartsWith("http://") && !webPageUrl.StartsWith("https://"))
                {
                    webPageUrl = "https://" + webPageUrl;
                }

                // Chromeでページを開く
                if (TryOpenWithChrome(webPageUrl))
                {
                    return;
                }

                // デフォルトブラウザで開く
                OpenWithDefaultBrowser(webPageUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Webページを開くときにエラーが発生しました: {ex.Message}", "🎮 WEBPAGE ERROR");
            }
        }

        /// <summary>
        /// Webページを指定の位置にスクロールして開く
        /// </summary>
        /// <param name="webPageUrl">WebページのURL</param>
        /// <param name="scrollValue">スクロール値（ピクセル）</param>
        public void OpenWebPageWithScroll(string webPageUrl, double scrollValue)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(webPageUrl))
                {
                    MessageBox.Show("WebページのURLが指定されていません。", "🎮 WEBPAGE ERROR");
                    return;
                }

                // URLにスクロール情報を追加（JavaScript実行用）
                var scrollScript = $"javascript:window.scrollTo(0,{scrollValue});";
                
                // まずページを開く
                OpenWebPageWithChrome(webPageUrl);
                
                // スクロール情報を表示
                if (scrollValue > 0)
                {
                    MessageBox.Show($"🎮 WEBPAGE OPENED!\n手動で {scrollValue}px の位置までスクロールしてください。", 
                                  "SCROLL INSTRUCTION");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"スクロール付きWebページを開くときにエラーが発生しました: {ex.Message}", "🎮 SCROLL ERROR");
            }
        }

        /// <summary>
        /// Chromeブラウザで開く
        /// </summary>
        private bool TryOpenWithChrome(string webPageUrl)
        {
            try
            {
                // Chromeの一般的なインストールパス
                string[] chromePaths = {
                    @"C:\Program Files\Google\Chrome\Application\chrome.exe",
                    @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"
                };

                foreach (var chromePath in chromePaths)
                {
                    if (File.Exists(chromePath))
                    {
                        var startInfo = new ProcessStartInfo
                        {
                            FileName = chromePath,
                            Arguments = webPageUrl,
                            UseShellExecute = false
                        };

                        Process.Start(startInfo);
                        MessageBox.Show($"🎮 Chrome起動: {webPageUrl}", "WEBPAGE OPENED");
                        return true;
                    }
                }
            }
            catch
            {
                // Chromeでの起動に失敗した場合は次の方法を試す
            }

            return false;
        }

        /// <summary>
        /// デフォルトブラウザで開く
        /// </summary>
        private void OpenWithDefaultBrowser(string webPageUrl)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = webPageUrl,
                    UseShellExecute = true
                });

                MessageBox.Show($"🎮 デフォルトブラウザで開きました: {webPageUrl}", "WEBPAGE OPENED");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"デフォルトブラウザでWebページを開けませんでした: {ex.Message}", "🎮 BROWSER ERROR");
            }
        }

        /// <summary>
        /// 複数のWebページを一度に開く
        /// </summary>
        /// <param name="webPageUrls">WebページのURL配列</param>
        public void OpenMultipleWebPages(params string[] webPageUrls)
        {
            foreach (var url in webPageUrls)
            {
                if (!string.IsNullOrWhiteSpace(url))
                {
                    OpenWebPageWithChrome(url);
                }
            }
        }

        /// <summary>
        /// WebページからMMファイル生成用のスクレイピング準備
        /// </summary>
        /// <param name="webPageUrl">対象のWebページURL</param>
        public void PrepareWebPageForMMGeneration(string webPageUrl)
        {
            OpenWebPageWithChrome(webPageUrl);
            
            MessageBox.Show("🎮 MM FILE GENERATION MODE!\nWebページの内容を確認して、\n重要な情報をコピーしてください。", 
                          "SCRAPING PREPARATION");
        }
    }
}