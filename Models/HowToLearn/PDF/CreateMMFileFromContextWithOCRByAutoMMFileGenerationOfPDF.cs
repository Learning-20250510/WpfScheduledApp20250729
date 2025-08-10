using WpfScheduledApp20250729.Models.FilesOperation;
using System.Windows;

namespace WpfScheduledApp20250729.Models.HowToLearn.PDF
{
    /// <summary>
    /// PDFからOCRでテキストを抽出してMMファイルを自動生成
    /// </summary>
    public class CreateMMFileFromContextWithOCRByAutoMMFileGenerationOfPDF : IHowToLearnAction
    {
        private readonly string _pdfFileName;
        private readonly int _specificPageNumber;
        private readonly PDFFileOperation _pdfOperation;
        private readonly FreePlaneFileOperation _freePlaneOperation;

        public CreateMMFileFromContextWithOCRByAutoMMFileGenerationOfPDF(string pdfFileName, int specificPageNumber)
        {
            _pdfFileName = pdfFileName;
            _specificPageNumber = specificPageNumber;
            _pdfOperation = new PDFFileOperation();
            _freePlaneOperation = new FreePlaneFileOperation();
        }

        public void TaskAction()
        {
            // PDFの特定ページを開いてOCR処理を促す
            _pdfOperation.OpenPDFSpecificPage(_pdfFileName, _specificPageNumber);
            
            MessageBox.Show("🎮 OCR MODE ACTIVATED!\nPDFの内容をテキスト化してMMファイルを生成します。\nページの内容を確認してください。", "OCR Processing");
            
            // TODO: 実際のOCR処理を実装
            // 現在は仮実装で、サンプルテキストでMMファイルを作成
            var sampleText = "PDFページからのサンプルテキスト。実際にはOCRで抽出されたテキストがここに入ります。";
            _freePlaneOperation.CreateMMFileWithTextAnalysis(0, sampleText, $"{_pdfFileName}_OCR_Page{_specificPageNumber}");
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