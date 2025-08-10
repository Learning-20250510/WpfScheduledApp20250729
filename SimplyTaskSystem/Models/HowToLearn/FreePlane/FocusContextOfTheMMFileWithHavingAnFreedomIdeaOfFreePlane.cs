using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTaskSystem.Models.HowToLearn.FreePlane
{
    class FocusContextOfTheMMFileWithHavingAnFreedomIdeaOfFreePlane
    {
        public string MMFileName;
        public FocusContextOfTheMMFileWithHavingAnFreedomIdeaOfFreePlane(string MMFileName)
        {
            this.MMFileName = MMFileName;
        }

        public void TaskAction()
        {
            //open MMFile

            Models.FilesOperation.FreePlaneFileOperation freePlaneFileOperation = new FilesOperation.FreePlaneFileOperation();
            freePlaneFileOperation.OpenSpecificFreePlaneFile(this.MMFileName);
            //taskクリアTrueを返す。
        }
    }
}
