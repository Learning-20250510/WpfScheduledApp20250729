using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTaskSystem.Models.HowToLearn.FreePlane
{
    class FocusTheMMFileWithHavingAnFreedomIdeaSpeedlyOfFreePlane
    {
        public string MMFileName;
        public FocusTheMMFileWithHavingAnFreedomIdeaSpeedlyOfFreePlane(string MMFileName)
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
