using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimplyTaskSystem.Models.FilesOperation
{
    class WebPageOperation
    {
        public void openWebPageWithChrome(string WebPage)
        {
            if (WebPage != null && WebPage != "")
            {
                System.Diagnostics.Process.Start(@"C:\Program Files\Google\Chrome\Application\chrome.exe", WebPage); // Windows 8

                //ps.Start();
            }
            else
            {
                MessageBox.Show(WebPage + " がnull or 空文字になってしまっています。");
            }
        }


        
    }
}
