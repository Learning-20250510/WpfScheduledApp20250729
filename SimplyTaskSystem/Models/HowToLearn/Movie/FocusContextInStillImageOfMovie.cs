using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTaskSystem.Models.HowToLearn.Movie
{
    class FocusContextInStillImageOfMovie//2s間（1sだとあまりにもレコード数が多いのと、1sと2sそんなに変わらないと思っている
    {
        public string MovieFileName;
        public int StartTime;
        public FocusContextInStillImageOfMovie(string MovieFileName, int StartTime)
        {
            this.MovieFileName = MovieFileName;
            this.StartTime = StartTime;
        }

        public void TaskAction(string KMTName, string KMN, string HTLName, int actionID)
        {
            //open pdfWithSpecificPage
            Models.FilesOperation.MovieFileOperation pdfOperation = new FilesOperation.MovieFileOperation();
            pdfOperation.OpenMovieFileWithSpecificTime(this.MovieFileName, this.StartTime);

            //createMMFile
            Models.FilesOperation.FreePlaneFileOperation freePlaneFileOperation = new Models.FilesOperation.FreePlaneFileOperation();
            freePlaneFileOperation.newCreateXml(KMTName, KMN, HTLName, actionID);

            //taskクリアTrueを返す。
        }
    }
}
