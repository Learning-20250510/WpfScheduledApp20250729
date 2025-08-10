using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTaskSystem.Models.DBs.TaskCommonElementsTable
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

        public void CreateTaskCommonElementsTable()
        {
            CommonDBsUtility cDBu = new CommonDBsUtility();
            string createTableSql = "create table if not exists simpletasksystem.taskcommonelements(id INT AUTO_INCREMENT NOT NULL PRIMARY KEY, kmn LONGTEXT NOT NULL, kmt int NOT NULL DEFAULT 1, htl INT NOT NULL DEFAULT 1, created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP)";
            cDBu.CreateTable(createTableSql, this.ConnectionString);
        }
    }
}
