using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTaskSystem.Models.DBs.TasksTable
{
    class UtilityForCreatingCommandText
    {
        private DayOfWeek dateWeek = DateTime.Now.DayOfWeek;

        public int GetTodayIsValueOfPCCorrespondenceTable(int PCOfWeeks = 1)
        {
            int returnValue = 0;//初期値、else節で初期化しないと変数が使えないから
            if (dateWeek == DayOfWeek.Monday)
            {
                if (PCOfWeeks == 1)
                {
                    returnValue = 3;
                }
                else if (PCOfWeeks == 2)
                {
                    returnValue = 10;
                }
            }
            else if (dateWeek == DayOfWeek.Tuesday)
            {
                if (PCOfWeeks == 1)
                {
                    returnValue = 4;
                }
                else if (PCOfWeeks == 2)
                {
                    returnValue = 11;
                }


            }
            else if (dateWeek == DayOfWeek.Wednesday)
            {
                if (PCOfWeeks == 1)
                {
                    returnValue = 5;
                }
                else if (PCOfWeeks == 2)
                {
                    returnValue = 12;
                }
            }
            else if (dateWeek == DayOfWeek.Thursday)
            {
                if (PCOfWeeks == 1)
                {
                    returnValue = 6;
                }
                else if (PCOfWeeks == 2)
                {
                    returnValue = 13;
                }
            }
            else if (dateWeek == DayOfWeek.Friday)
            {
                if (PCOfWeeks == 1)
                {
                    returnValue = 7;
                }
                else if (PCOfWeeks == 2)
                {
                    returnValue = 14;
                }
            }
            else if (dateWeek == DayOfWeek.Saturday)
            {
                if (PCOfWeeks == 1)
                {
                    returnValue = 8;
                }
                else if (PCOfWeeks == 2)
                {
                    returnValue = 15;
                }
            }
            else if (dateWeek == DayOfWeek.Sunday)
            {
                if (PCOfWeeks == 1)
                {
                    returnValue = 9;
                }
                else if (PCOfWeeks == 2)
                {
                    returnValue = 16;
                }
            }
            else
            {
                throw new Exception("曜日の値として不適切な値が入っています。その値は： " + returnValue);
            }

            System.Diagnostics.Debug.WriteLine("returnValue: " + returnValue);
            return returnValue;
        }

        public int GetTodayIsWeekdayORWeekendFromCorrespondenceTable()
        {
            int returnValue;
            if (dateWeek == DayOfWeek.Saturday || dateWeek == DayOfWeek.Sunday)
            {
                returnValue = 18;
            }
            else
            {
                returnValue = 17;
            }

            Debug.WriteLine("Method: GetTodayIsWeekdayORWeekendFromCorrespondenceTable, returnValue: " + returnValue);
            return returnValue;
        }



    }
}
