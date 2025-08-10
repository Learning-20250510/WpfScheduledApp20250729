using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTaskSystem.Models.HowToLearn.Movie
{
    class FocusContextBetWeen10SecondsOfMovie
    {
        private string MovieFileName;
        private int StartTime;


        public FocusContextBetWeen10SecondsOfMovie(string MovieFileName, int StartTime)
        {
            this.MovieFileName = MovieFileName;
            this.StartTime = StartTime;
        }

        public void TaskAction(string KMTName, string KMN, string HTLName, int actionID)
        {
            //open movieFileWithSpecificStartTime
            Models.FilesOperation.MovieFileOperation pdfOperation = new FilesOperation.MovieFileOperation();
            pdfOperation.OpenMovieFileWithSpecificTime(this.MovieFileName, this.StartTime);

            //createMMFile
            Models.FilesOperation.FreePlaneFileOperation freePlaneFileOperation = new Models.FilesOperation.FreePlaneFileOperation();
            freePlaneFileOperation.newCreateXml(KMTName, KMN, HTLName, actionID);

            //taskクリアTrueを返す。
        }
    }
}
