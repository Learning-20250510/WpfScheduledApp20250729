using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTaskSystem.Models.DBs
{
    class DBsSetup
    {
        public void createMustTables()
        {
            TaskCommonElementsTable.InitialSettingsInformation tcet = new TaskCommonElementsTable.InitialSettingsInformation();
            tcet.CreateTaskCommonElementsTable();

            PrioritiesTable.InitialSettingsInformation p = new PrioritiesTable.InitialSettingsInformation();
            p.CreatePRTable();

            var pc = new PeriodicallyCyclesTable.InitialSettingsInformation();
            pc.CreatePCTable();

            var kmt = new KMTTable.InitialSettingsInformation();
            kmt.CreateKMTTable();

            var htl = new HTLTable.InitialSettingsInformation();
            htl.CreateHTLTable();

            var kmn = new KMNTable.InitialSettingsInformation();
            kmn.CreateKMNTable();

            var pcp = new ParentChildrenProjectTable.InitialSettingsInformation();
            pcp.CreatePCPTable();

            var dummyTasksTable = new DummyTasksTable.InitialSettingsInformation();
            dummyTasksTable.CreateDummyTasksTable();

            var motivationsTable = new MotivationsTable.InitialSettingsInformation();
            motivationsTable.CreateMotivationsTable();

            var specicRangeTable = new SpecificRangeTable.InitialSettingsInformation();
            specicRangeTable.CreateSpecificRangeTable();
        }

        public void insertInitialRecords()
        {
            var kmt = new KMTTable.Insert();
            kmt.InsertInitialRecords();

            var htl = new HTLTable.Insert();
            htl.InsertInitialRecords();

            var pr = new PrioritiesTable.Insert();
            pr.InsertInitialRecords();

            var pc = new PeriodicallyCyclesTable.Insert();
            pc.InsertInitialRecords();

            var pcp = new ParentChildrenProjectTable.Insert();
            pcp.InsertInitialRecords();

            //DeleteとInsertがセット。
            var sr = new SpecificRangeTable.Delete();
            sr.DeleteRecordsUnderConditionsPeriodically();


        }
    }
}
