using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTaskSystem.Models.HowToLearn.TheWorld
{
    class ReadyForVariablesListLearningOfTheWorld
    {

        public ReadyForVariablesListLearningOfTheWorld()
        {

        }

        public void TaskAction(string KMTName, string KMN, string HTLName, int actionID)
        {
            //create New MMFile and Insert To DummyTasksTable
            Models.FilesOperation.FreePlaneFileOperation freePlaneFileOperation = new Models.FilesOperation.FreePlaneFileOperation();
            freePlaneFileOperation.newCreateXml(KMTName, KMN, HTLName, actionID);

            

            //taskクリアTrueを返す。
        }
    }
}
