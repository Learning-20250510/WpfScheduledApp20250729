using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTaskSystem.Models.DBs.HTLTable
{
    class InitialSettingsInformation
    {
        private readonly string Server = "localhost";
        private readonly string Database = "mysql";
        private readonly string User = "root";
        private readonly string Pass = "shafkkd7";
        private readonly string Charset = "utf8";

        private string _connectionString;
        public string ConnectionString
        {
            get
            {

                return this._connectionString = string.Format("Server={0};Database={1};Uid={2};Pwd={3};Charset={4}", Server, Database, User, Pass, Charset);

            }

        }

        public void CreateHTLTable()
        {
            CommonDBsUtility cDBu = new CommonDBsUtility();
            string createTableSql = " create table if not exists simpletasksystem.htl(id INT AUTO_INCREMENT NOT NULL PRIMARY KEY, htl VARCHAR(200) NOT NULL UNIQUE, created_at DATETIME NOT NULL DEFAULT current_timestamp)";
            cDBu.CreateTable(createTableSql, this.ConnectionString);
        }

        
    }
}
