using WpfScheduledApp20250729.Models.FilesOperation;
using System.Windows;

namespace WpfScheduledApp20250729.Models.HowToLearn.WebPage
{
    /// <summary>
    /// ブラウザから直接MMファイルを自動生成する
    /// </summary>
    public class CreateMMFileDirectlyFromBrowserByAutoMMFileGenerationOfWebPage : IHowToLearnAction
    {
        private readonly string _webPage;
        private readonly WebPageOperation _webPageOperation;
        private readonly FreePlaneFileOperation _freePlaneOperation;

        public CreateMMFileDirectlyFromBrowserByAutoMMFileGenerationOfWebPage(string webPage)
        {
            _webPage = webPage;
            _webPageOperation = new WebPageOperation();
            _freePlaneOperation = new FreePlaneFileOperation();
        }

        public void TaskAction()
        {
            // WebページをMMファイル生成用に準備
            _webPageOperation.PrepareWebPageForMMGeneration(_webPage);

            // 基本的なMMファイルを作成
            _freePlaneOperation.CreateBasicMMFile("WebPage", _webPage, "DirectGeneration", 0);
        }

        public void TaskAction(string kmtName, string kmn, string htlName, int taskId)
        {
            TaskAction();
        }

        public void TaskAction(string kmtName, string kmn, string htlName, int taskId, string relationalFile1, string relationalFile2)
        {
            TaskAction();
        }
    }
}