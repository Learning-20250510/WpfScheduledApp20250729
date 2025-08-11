using WpfScheduledApp20250729.Models.FilesOperation;

namespace WpfScheduledApp20250729.Models.HowToLearn.PDF
{
    /// <summary>
    /// PDFの特定ページにフォーカスしてデザインを学習
    /// </summary>
    public class FocusDesignInSpecificPageOfPDF : IHowToLearnAction
    {
        private readonly string _pdfFileName;
        private readonly int _specificPageNumber;
        private readonly PDFFileOperation _pdfOperation;
        private readonly FreePlaneFileOperation _freePlaneOperation;

        public FocusDesignInSpecificPageOfPDF(string pdfFileName, int specificPageNumber)
        {
            _pdfFileName = pdfFileName;
            _specificPageNumber = specificPageNumber;
            _pdfOperation = new PDFFileOperation();
            _freePlaneOperation = new FreePlaneFileOperation();
        }

        public void TaskAction()
        {
            // PDFの特定ページを開く（デザインフォーカス）
            _pdfOperation.OpenPDFSpecificPage(_pdfFileName, _specificPageNumber);
        }

        public void TaskAction(string kmtName, string kmn, string htlName, int taskId)
        {
            // PDFの特定ページを開く
            _pdfOperation.OpenPDFSpecificPage(_pdfFileName, _specificPageNumber);

            // MMファイルを作成（デザイン分析用）
            _freePlaneOperation.CreateBasicMMFile(kmtName, kmn, htlName + "_Design", taskId);
        }

        public void TaskAction(string kmtName, string kmn, string htlName, int taskId, string relationalFile1, string relationalFile2)
        {
            TaskAction(kmtName, kmn, htlName, taskId);
        }
    }
}