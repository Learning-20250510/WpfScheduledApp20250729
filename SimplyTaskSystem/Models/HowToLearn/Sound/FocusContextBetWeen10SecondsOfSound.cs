using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTaskSystem.Models.HowToLearn.Sound
{
    class FocusContextBetWeen10SecondsOfSound
    {
        public string SoundFileName;
        public int StartTime;
        public FocusContextBetWeen10SecondsOfSound(string SoundFileName, int StartTime)
        {
            this.SoundFileName = SoundFileName;
            this.StartTime = StartTime;
        }

        public void TaskAction(string KMTName, string KMN, string HTLName, int actionID)
        {
            //open soundFileWithSpecificStartTime
            Models.FilesOperation.MovieFileOperation pdfOperation = new FilesOperation.MovieFileOperation();
            pdfOperation.OpenMovieFileWithSpecificTime(this.SoundFileName, this.StartTime);
            //create New MMFile
            Models.FilesOperation.FreePlaneFileOperation freePlaneFileOperation = new Models.FilesOperation.FreePlaneFileOperation();
            freePlaneFileOperation.newCreateXml(KMTName, KMN, HTLName, actionID);

            //taskクリアTrueを返す。
        }
    }
}
