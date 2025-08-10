using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTaskSystem.Models.HowToLearn
{
    class CallFromOutside
    {

        public void CallFromOutsideMethod(string specificHowToLearnName)
        {

            switch (specificHowToLearnName)
            {
                case "FocusContextBetWeen10SecondsOfMovie":
                    DebugWriteLineOfHowToLearnName("FocusContextBetWeen10SecondsOfMovie");
                    break;
                case "FocusContextInStillImageOfMovie":
                    DebugWriteLineOfHowToLearnName("FocusContextInStillImageOfMovie");
                    break;
                case "FocusContextInSpecificPageOfPDF":
                    DebugWriteLineOfHowToLearnName("FocusContextInSpecificPageOfPDF");
                    break;
                case "FocusDesignInSpecificPageOfPDF":
                    DebugWriteLineOfHowToLearnName("FocusDesignInSpecificPageOfPDF");
                    break;
                case "AnywayOfTheWorld":
                    DebugWriteLineOfHowToLearnName("AnywayOfTheWorld");
                    break;
                case "FocusContextInScrollValueOfWebPage":
                    DebugWriteLineOfHowToLearnName("FocusContextInScrollValueOfWebPage");
                    break;
                case "FocusDesignInScrollValueOfWebPage":
                    DebugWriteLineOfHowToLearnName("FocusDesignInScrollValueOfWebPage");
                    break;

                default:
                    throw new Exception(specificHowToLearnName + " というspecficHowToLearnNameは存在しません。存在するHowToLearnNameを指定してください。");


            }
           
        }

        public readonly string FocusContextBetWeen10SecondsOfMovie = "FocusContextBetWeen10SecondsOfMovie";
        public readonly string FocusContextInStillImageOfMovie = "FocusContextInStillImageOfMovie";
        public readonly string FocusContextInSpecificPageOfPDF = "FocusContextInSpecificPageOfPDF";
        public readonly string FocusDesignInSpecificPageOfPDF = "FocusDesignInSpecificPageOfPDF";
        public readonly string AnywayOfTheWorld = "AnywayOfTheWorld";
        public readonly string FocusContextInScrollValueOfWebPage = "FocusContextInScrollValueOfWebPage";
        public readonly string FocusDesignInScrollValueOfWebPage = "FocusDesignInScrollValueOfWebPage";



        private void DebugWriteLineOfHowToLearnName(string howToLearnName)
        {
            System.Diagnostics.Debug.WriteLine("実行されたHowToLearnName:  " + howToLearnName + "  です。");
        }
    }
}
