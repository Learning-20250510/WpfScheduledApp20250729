using WpfScheduledApp20250729.Models.FilesOperation;
using System.Windows;

namespace WpfScheduledApp20250729.Models.HowToLearn.WebPage
{
    /// <summary>
    /// Webページを自動スクレイピングしてMMファイルを生成する
    /// </summary>
    public class CreateMMFileAutomaticallyScrapingByAutoMMFileGenerationOfWebPage : IHowToLearnAction
    {
        private readonly string _webPage;
        private readonly WebPageOperation _webPageOperation;
        private readonly FreePlaneFileOperation _freePlaneOperation;

        public CreateMMFileAutomaticallyScrapingByAutoMMFileGenerationOfWebPage(string webPage)
        {
            _webPage = webPage;
            _webPageOperation = new WebPageOperation();
            _freePlaneOperation = new FreePlaneFileOperation();
        }

        public void TaskAction()
        {
            // WebページをMMファイル生成用に準備
            _webPageOperation.PrepareWebPageForMMGeneration(_webPage);

            MessageBox.Show("🎮 AUTO SCRAPING MODE!\nWebページの内容を自動取得してMMファイルを生成します。", "SCRAPING START");

            // TODO: 実際のスクレイピング処理を実装
            // 現在は仮実装で、サンプルテキストでMMファイルを作成
            var scrapedText = $"スクレイピングされたWebページの内容: {_webPage}\n" +
                            "タイトル: サンプルページ\n" +
                            "メインコンテンツ: ここに抽出されたテキストが入ります。\n" +
                            "キーワード: テクノロジー、学習、自動化";

            _freePlaneOperation.CreateMMFileWithTextAnalysis(0, scrapedText, $"Scraped_{GetDomainFromUrl(_webPage)}");
        }

        public void TaskAction(string kmtName, string kmn, string htlName, int taskId)
        {
            TaskAction();
        }

        public void TaskAction(string kmtName, string kmn, string htlName, int taskId, string relationalFile1, string relationalFile2)
        {
            TaskAction();
        }

        /// <summary>
        /// URLからドメイン名を抽出するヘルパーメソッド
        /// </summary>
        private string GetDomainFromUrl(string url)
        {
            try
            {
                if (url.StartsWith("http://") || url.StartsWith("https://"))
                {
                    var uri = new System.Uri(url);
                    return uri.Host.Replace("www.", "");
                }
                else if (url.StartsWith("www."))
                {
                    return url.Substring(4).Split('/')[0];
                }
                else
                {
                    return url.Split('/')[0].Replace("www.", "");
                }
            }
            catch
            {
                return "UnknownSite";
            }
        }
    }
}