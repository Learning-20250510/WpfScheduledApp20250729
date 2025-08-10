using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTaskSystem.Models.DBs.DummyTasksTable
{


    class DataClass
    {
        public int ID { get; set; }
        public string KMN { get; set; }
        public int KMT { get; set; }
        public int HTL { get; set; }
        public int PCP_ID { get; set; }
        public string Description { get; set; }
        public int EstimatedTime { get; set; }
        public string DueDate { get; set; }
        public string DueTime { get; set; }
        public int Priority { get; set; }
        public int RepeatTimesPerDay { get; set; }
        public int RepeatTimesPerDayDummy { get; set; }
        public string CreatedAt { get; set; }
        public string LastClearedAt { get; set; }
        public int ClearTimesInTime { get; set; }
        public int ClearTimesOutOfTime { get; set; }
        public bool Archived { get; set; }
        public bool Postpone { get; set; }
        public int RepeatDuration { get; set; }
        public int PeriodicallyCycles { get; set; }
        public string SpecifiedDay { get; set; }
        public int SpecificPageAsPDF { get; set; }
        public int specificScrollValueAsWebPage { get; set; }
        public int TenSecondsIncrementAsSoundsAndMovie { get; set; }
        public int TwoSecondsIncrementAsMovie { get; set; }
        public string RelationalFile1 { get; set; }
        public string RelationalFile2 { get; set; }
        public bool AutoCreateFirstVariableBranchesMMF { get; set; }
        public int RepeatPatterns { get; set; }
        public int TemporaryRepeatTaskCount { get; set; }


        //別クラスのカラム
        public string ParentProjectName { get; set; }
        public string ChildProjectName { get; set; }



        //KMNのカラム
        public int KMN_ID { get; set; }
        public string KMN_KMN { get; set; }

        //HTLTableのカラム
        public int HTL_ID { get; set; }
        public string HTL_HTL { get; set; }

        //KMTTableのカラム
        public int KMT_ID { get; set; }
        public string KMT_KMT { get; set; }

        //PeriodicallyCyclesのカラム
        public int PC_ID { get; set; }
        public string PC_Name { get; set; }

        //Prioritiesのカラム
        public int PR_ID { get; set; }
        public string PR_Name { get; set; }
    


    }

}
