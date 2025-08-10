using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimplyTaskSystem.Models.HowToLearn.WebPage
{
    class CreateMMFileDirectlyFromBrowserByAutoMMFileGenerationOfWebPage
    {
        public string WebPage;
        public CreateMMFileDirectlyFromBrowserByAutoMMFileGenerationOfWebPage(string webPage)
        {
            this.WebPage = webPage;
        }

        public void TaskAction()
        {
            //Open WebPage
            Models.FilesOperation.WebPageOperation webPageOperation = new FilesOperation.WebPageOperation();
            webPageOperation.openWebPageWithChrome(this.WebPage);

            //"AutoMMFileGeneratorMethod(重複ないファイル生成までしてくれる）で実行"

            //taskクリアTrueを返す。
        }
    }
}
