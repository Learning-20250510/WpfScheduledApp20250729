using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTaskSystem.Models.HowToLearn.PDF
{
    class CreateMMFileFromContextWithOCRByAutoMMFileGenerationOfPDF
    {
        public string PDFFileName;
        public int SpecificPageNumber;
        public CreateMMFileFromContextWithOCRByAutoMMFileGenerationOfPDF(string PDFFileName, int SpecificPageNumber)
        {
            this.PDFFileName = PDFFileName;
            this.SpecificPageNumber = SpecificPageNumber;
        }

        public void TaskAction()
        {
            //open pdfWithSpecificPage
            Models.FilesOperation.PDFFileOperation pdfOperation = new FilesOperation.PDFFileOperation();
            pdfOperation.OpenPDFFSpecificPage(this.PDFFileName, this.SpecificPageNumber);
            

            //taskクリアTrueを返す。
        }
    }
}
