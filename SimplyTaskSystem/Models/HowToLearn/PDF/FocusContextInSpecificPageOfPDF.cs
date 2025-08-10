using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTaskSystem.Models.HowToLearn.PDF
{
    class FocusContextInSpecificPageOfPDF
    {
        public string PDFFileName;
        public int SpecificPageNumber;
        public FocusContextInSpecificPageOfPDF(string PDFFileName, int SpecificPageNumber)
        {
            this.PDFFileName = PDFFileName;
            this.SpecificPageNumber = SpecificPageNumber;
        }

        public void TaskAction(string KMTName, string KMN, string HTLName, int actionID)
        {
            //open pdfWithSpecificPage
            Models.FilesOperation.PDFFileOperation pdfOperation = new FilesOperation.PDFFileOperation();
            pdfOperation.OpenPDFFSpecificPage(this.PDFFileName, this.SpecificPageNumber);

            //create New MMFile
            Models.FilesOperation.FreePlaneFileOperation freePlaneFileOperation = new Models.FilesOperation.FreePlaneFileOperation();
            freePlaneFileOperation.newCreateXml(KMTName, KMN, HTLName, actionID);

            //taskクリアTrueを返す。
        }
    }
}
