using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTaskSystem.Models.HowToLearn.MyBrain
{
    class FocusTheKMNWithHavingAnIdeaMyBrain
    { 
        public FocusTheKMNWithHavingAnIdeaMyBrain()
        {

        }

        public void TaskAction(string KMTName, string KMN, string HTLName, int actionID)
        {
            //create New MMFile
            Models.FilesOperation.FreePlaneFileOperation freePlaneFileOperation = new Models.FilesOperation.FreePlaneFileOperation();
            freePlaneFileOperation.newCreateXml(KMTName, KMN, HTLName, actionID);


            //taskクリアTrueを返す。
        }
    }
}
