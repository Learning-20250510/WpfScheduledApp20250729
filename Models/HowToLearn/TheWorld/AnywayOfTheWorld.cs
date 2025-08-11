using WpfScheduledApp20250729.Models.FilesOperation;

namespace WpfScheduledApp20250729.Models.HowToLearn.TheWorld
{
    /// <summary>
    /// 世界の様々なリソース（WebページやFile）を使った学習
    /// </summary>
    public class AnywayOfTheWorld : IHowToLearnAction
    {
        private readonly WebPageOperation _webPageOperation;
        private readonly FreePlaneFileOperation _freePlaneOperation;

        public AnywayOfTheWorld()
        {
            _webPageOperation = new WebPageOperation();
            _freePlaneOperation = new FreePlaneFileOperation();
        }

        public void TaskAction()
        {
            // 基本的なMMファイルを作成
            _freePlaneOperation.CreateBasicMMFile("World", "AnyResource", "TheWorld", 0);
        }

        public void TaskAction(string kmtName, string kmn, string htlName, int taskId)
        {
            // MMファイルを作成
            _freePlaneOperation.CreateBasicMMFile(kmtName, kmn, htlName, taskId);
        }

        public void TaskAction(string kmtName, string kmn, string htlName, int taskId, string relationalFile1, string relationalFile2)
        {
            // MMファイルを作成
            _freePlaneOperation.CreateBasicMMFile(kmtName, kmn, htlName, taskId);

            // Relational File 1を開く（WebページまたはFile）
            if (!string.IsNullOrWhiteSpace(relationalFile1))
            {
                if (IsWebUrl(relationalFile1))
                {
                    _webPageOperation.OpenWebPageWithChrome(relationalFile1);
                }
                else
                {
                    // ファイルの場合は拡張子に応じて適切なアプリで開く
                    OpenFile(relationalFile1);
                }
            }

            // Relational File 2を開く
            if (!string.IsNullOrWhiteSpace(relationalFile2))
            {
                if (IsWebUrl(relationalFile2))
                {
                    _webPageOperation.OpenWebPageWithChrome(relationalFile2);
                }
                else
                {
                    OpenFile(relationalFile2);
                }
            }
        }

        /// <summary>
        /// URLかどうかを判定
        /// </summary>
        private bool IsWebUrl(string input)
        {
            return input.StartsWith("http://") || 
                   input.StartsWith("https://") || 
                   input.StartsWith("www.") ||
                   input.Contains(".com") ||
                   input.Contains(".net") ||
                   input.Contains(".org");
        }

        /// <summary>
        /// ファイルを適切なアプリケーションで開く
        /// </summary>
        private void OpenFile(string filePath)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show($"ファイルを開けませんでした: {ex.Message}", "🎮 FILE ERROR");
            }
        }
    }
}