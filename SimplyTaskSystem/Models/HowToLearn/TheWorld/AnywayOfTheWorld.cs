using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTaskSystem.Models.HowToLearn.TheWorld
{
    class AnywayOfTheWorld
    {

        public AnywayOfTheWorld()
        {

        }

        public void TaskAction(string KMTName, string KMN, string HTLName, int actionID, string RF1, string RF2)
        {
            //create New MMFile
            Models.FilesOperation.FreePlaneFileOperation freePlaneFileOperation = new Models.FilesOperation.FreePlaneFileOperation();
            freePlaneFileOperation.newCreateXml(KMTName, KMN, HTLName,actionID);

            Models.FilesOperation.WebPageOperation webPageOperation = new FilesOperation.WebPageOperation();
            if (RF1 != null && RF1 != "")
            {
                //Open RF1
                webPageOperation.openWebPageWithChrome(RF1);
            }
            if (RF2 != null && RF2 != "")
            {
                //Open RF2
                webPageOperation.openWebPageWithChrome(RF2);
            }

            //taskクリアTrueを返す。
        }
    }
}
