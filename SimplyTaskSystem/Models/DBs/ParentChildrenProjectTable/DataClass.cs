using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTaskSystem.Models.DBs.ParentChildrenProjectTable
{
    class DataClass
    {
        public int ID { get; set; }
        public string ParentProjectName { get; set; }
        public string ChildProjectName { get; set; }
        public string CreatedAt { get; set; }

        //ForWindow(ParentとChildの連結文字列
        public string ParentChildComboName { get; set; }
    }
}
