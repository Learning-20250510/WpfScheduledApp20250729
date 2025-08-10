using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimplyTaskSystem.Models.FilesOperation
{
    class PDFFileOperation
    {
        private string PDFFileName;
        private string PDFFolderName;//PDFｆｉｌｅを置いてある箇所の一番上（再帰的に検索していく）
        private int GetTotalOfPagesOfSpecificPDFFile(string pDFFileName)
        {
            pDFFileName = this.PDFFileName;//外部から指定されたPDFFile名をMethod内部に渡す。
            return 0;
        }
        public void OpenPDFFSpecificPage(string PDFFileName, int specificPageNumber)
        {

            string exe_path = @"C:\Program Files (x86)\Adobe\Acrobat DC\Acrobat\Acrobat.exe";
            string sfile = @"C:\Users\tugar\Desktop\CACN2016-01.pdf";


            /*
            Process myProcess = new Process();
            myProcess.StartInfo.FileName = exe_path;
            myProcess.StartInfo.Arguments = "/A \"page=2\" \"C:\\Users\tugar\\Desktop\\Thesis_サイモン意志決定論の特質 1_KU-1100-19690425-02.pdf\"";
            myProcess.Start();

            ProcessStartInfo startInfo = new ProcessStartInfo(exe_path, "/A page=2 C:\\Users\tugar\\Desktop\\Thesis_サイモン意志決定論の特質 1_KU-1100-19690425-02.pdf");
            startInfo.WindowStyle = ProcessWindowStyle.Maximized;
            Process.Start(startInfo);
            */


            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = exe_path;
            //startInfo.Arguments = "/A \"page=1\" \"C:\\Users\\tugar\\Desktop\\Thesis_サイモン意思決定論の特質_2_KU-1100-19690625-02.pdf\"";
            startInfo.Arguments = "/A \"page=12\" \"C:\\Users\\tugar\\Desktop\\Thesis_サイモン意思決定論の特質_2_KU-1100-19690625-02.pdf\"";
            Process.Start(startInfo);
        }
    }
}
