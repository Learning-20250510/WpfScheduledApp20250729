using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTaskSystem.Models.HowToLearn.WebPage
{
    class FocusDesignInScrollValueOfWebPage
    {
        public string WebPage;
        public double ScrollValue;
        public FocusDesignInScrollValueOfWebPage(string webPage, double scrollValue)
        {
            this.WebPage = webPage;
            this.ScrollValue = scrollValue;
        }

        public void TaskAction(string KMTName, string KMN, string HTLName, int actionID)
        {
            //open WebPage
            Models.FilesOperation.WebPageOperation webPageOperation = new FilesOperation.WebPageOperation();
            webPageOperation.openWebPageWithChrome(this.WebPage);

            //特定位置までスクロール(By JavaScript)

            //create New MMFile
            Models.FilesOperation.FreePlaneFileOperation freePlaneFileOperation = new Models.FilesOperation.FreePlaneFileOperation();
            freePlaneFileOperation.newCreateXml(KMTName, KMN, HTLName, actionID);

            //taskクリアTrueを返す。
        }
    }
}
