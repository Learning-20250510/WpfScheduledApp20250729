using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTaskSystem.Models.DBs.SpecificRangeTable
{
    class DataClass
    {
        public int ID { get; set; }

        public int TasksTableID{ get; set; }
        public string Category { get; set; }
        public string CreatedAt { get; set; }
    }
}
