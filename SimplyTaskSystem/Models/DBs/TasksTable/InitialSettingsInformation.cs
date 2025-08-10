using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTaskSystem.Models.DBs.TasksTable
{
    using MySql.Data.MySqlClient;
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

        public void CreateTasksTable()
        {
            CommonDBsUtility cDBu = new CommonDBsUtility();
            System.Diagnostics.Debug.WriteLine("さいご");
            //string createTableSql = "create table if not exists simpletasksystem.tasks(id INT AUTO_INCREMENT NOT NULL PRIMARY KEY, kmn_kmt_htl_id INT NOT NULL CHECK(kmn_kmt_htl_id > 0), childrenproject_id INT NOT NULL DEFAULT 1 CHECK(childrenproject_id > 0), description TEXT DEFAULT NULL, estimated_time INT NOT NULL DEFAULT 1 CHECK(estimated_time > 0), due_date DATETIME NOT NULL DEFAULT current_timestamp, due_time TIME DEFAULT NULL, priority INT NOT NULL DEFAULT 4 CHECK(priority > 0), repeat_times_per_day INT NOT NULL DEFAULT 1 CHECK(repeat_times_per_day > 0), repeat_times_per_day_dummy INT NOT NULL DEFAULT 1 CHECK(repeat_times_per_day_dummy > 0), created_at DATETIME NOT NULL DEFAULT current_timestamp, lastcleared_at DATETIME DEFAULT NULL, clear_times_intime INT NOT NULL DEFAULT 0 CHECK(clear_times_intime >=0), clear_times_outoftime INT NOT NULL DEFAULT 0 CHECK(clear_times_outoftime>=0), archived BOOLEAN NOT NULL DEFAULT 0,  postpone BOOLEAN NOT NULL DEFAULT 0, repeat_duration INT NOT NULL DEFAULT 1 CHECK(repeat_duration > 0), periodically_cycles INT NOT NULL DEFAULT 1 CHECK(periodically_cycles > 0), specified_day DATETIME DEFAULT NULL, specific_page_as_pdf INT NOT NULL DEFAULT 0 CHECK(specific_page_as_pdf >=0), specific_scrollvalue_as_webpage INT NOT NULL DEFAULT -1 CHECK(specific_scrollvalue_as_webpage >= -1 and specific_scrollvalue_as_webpage <= 100), ten_seconds_increment_as_sounds_and_movie INT NOT NULL DEFAULT 0 CHECK(ten_seconds_increment_as_sounds_and_movie >= 0), two_seconds_increment_as_movie INT NOT NULL DEFAULT 0 CHECK(two_seconds_increment_as_movie >= 0), relational_file_1 TEXT DEFAULT NULL, relational_file_2 TEXT DEFAULT NULL, auto_create_first_variable_branches_mmfile BOOL NOT NULL DEFAULT FALSE)";
            string createTableSql = "create table if not exists simpletasksystem.tasks(id INT AUTO_INCREMENT NOT NULL PRIMARY KEY, kmn LONGTEXT NOT NULL, kmt INT NOT NULL DEFAULT 1 CHECK(kmt > 0), htl INT NOT NULL DEFAULT 1 CHECK(htl > 0), pcp_id INT NOT NULL DEFAULT 1 CHECK(pcp_id > 0), description TEXT DEFAULT NULL, estimated_time INT NOT NULL DEFAULT 1 CHECK(estimated_time > 0), due_date DATETIME NOT NULL DEFAULT current_timestamp, due_time TIME DEFAULT NULL, priority INT NOT NULL DEFAULT 4 CHECK(priority > 0), repeat_times_per_day INT NOT NULL DEFAULT 1 CHECK(repeat_times_per_day > 0), repeat_times_per_day_dummy INT NOT NULL DEFAULT 1 CHECK(repeat_times_per_day_dummy > 0), created_at DATETIME NOT NULL DEFAULT current_timestamp, lastcleared_at DATETIME DEFAULT NULL, clear_times_intime INT NOT NULL DEFAULT 0 CHECK(clear_times_intime >=0), clear_times_outoftime INT NOT NULL DEFAULT 0 CHECK(clear_times_outoftime>=0), archived BOOLEAN NOT NULL DEFAULT 0,  postpone BOOLEAN NOT NULL DEFAULT 0, repeat_duration INT NOT NULL DEFAULT 1 CHECK(repeat_duration > 0), periodically_cycles INT NOT NULL DEFAULT 1 CHECK(periodically_cycles > 0), specified_day DATETIME DEFAULT NULL, specific_page_as_pdf INT NOT NULL DEFAULT 0 CHECK(specific_page_as_pdf >=0), specific_scrollvalue_as_webpage INT NOT NULL DEFAULT 0 CHECK(specific_scrollvalue_as_webpage >= -1 and specific_scrollvalue_as_webpage <= 100), ten_seconds_increment_as_sounds_and_movie INT NOT NULL DEFAULT 0 CHECK(ten_seconds_increment_as_sounds_and_movie >= 0), two_seconds_increment_as_movie INT NOT NULL DEFAULT 0 CHECK(two_seconds_increment_as_movie >= 0), relational_file_1 TEXT DEFAULT NULL, relational_file_2 TEXT DEFAULT NULL, auto_create_first_variable_branches_mmfile BOOL NOT NULL DEFAULT FALSE, repeat_patterns INT NOT NULL DEFAULT 1 CHECK(repeat_patterns>0) temporary_repeat_task_count INT NOT NULL DEFAULT 0)";
            cDBu.CreateTable(createTableSql, this.ConnectionString);
        }



    }
}
