using WpfScheduledApp20250729.Models.FilesOperation;

namespace WpfScheduledApp20250729.Models.HowToLearn.WebPage
{
    /// <summary>
    /// Webページの特定スクロール位置にフォーカスしてデザインを学習
    /// </summary>
    public class FocusDesignInScrollValueOfWebPage : IHowToLearnAction
    {
        private readonly string _webPage;
        private readonly double _scrollValue;
        private readonly WebPageOperation _webPageOperation;
        private readonly FreePlaneFileOperation _freePlaneOperation;

        public FocusDesignInScrollValueOfWebPage(string webPage, double scrollValue)
        {
            _webPage = webPage;
            _scrollValue = scrollValue;
            _webPageOperation = new WebPageOperation();
            _freePlaneOperation = new FreePlaneFileOperation();
        }

        public void TaskAction()
        {
            // Webページを特定スクロール位置で開く（デザインフォーカス）
            _webPageOperation.OpenWebPageWithScroll(_webPage, _scrollValue);
        }

        public void TaskAction(string kmtName, string kmn, string htlName, int taskId)
        {
            // Webページを特定スクロール位置で開く
            _webPageOperation.OpenWebPageWithScroll(_webPage, _scrollValue);

            // MMファイルを作成（デザイン分析用）
            _freePlaneOperation.CreateBasicMMFile(kmtName, kmn, htlName + "_Design", taskId);
        }

        public void TaskAction(string kmtName, string kmn, string htlName, int taskId, string relationalFile1, string relationalFile2)
        {
            TaskAction(kmtName, kmn, htlName, taskId);
        }
    }
}