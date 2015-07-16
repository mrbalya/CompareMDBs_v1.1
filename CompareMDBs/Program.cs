using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Management;
//using System.Data.SqlClient;
using ADOX; //Requires Microsoft ADO Ext. 2.8 for DDL and Security
using ADODB;

namespace CompareMDBs
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CompareMDBs());
            
        }
        /*
        public void testConnection(string path)
        {
            String connect = ("Provider=Microsoft.JET.OLEDB.4.0;data source=" + path + ";Persist Security Info=False;");
            OleDbConnection con = new OleDbConnection(connect);
            try
            {
                con.Open();
                con.Close();
            }
            catch (OleDbException e)
            {
                MessageBox.Show("Oops! " + e.Message + " Try open database with MS Access.");

            }
        }
        */

        public static T ConvertFromDBVal<T>(object obj) // usefull if query could return null
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default(T); // returns the default value for the type
            }
            else
            {
                return (T)obj;
            }
        }

        
        public static List<string> GetDBTables(string path)
        {
            List<string> allTables = new List<string>();
            String connect = ("Provider=Microsoft.JET.OLEDB.4.0;data source=" + path + ";Persist Security Info=False;");
            OleDbConnection con = new OleDbConnection(connect);
            con.Open();

            DataTable tables = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
                                                        new object[] { null, null, null, "TABLE" });

            int counter = 1;
            foreach (DataRow row in tables.Rows)
            {
                allTables.Add(row[2].ToString());
                counter++;
            }
            con.Close();

            return allTables;
        }

        public static void addToLog (string textToAdd)
        {
            if (!File.Exists(".\\CompareMDBs.log") )
                File.Create(".\\CompareMDBs.log").Dispose();


            using (StreamWriter w = File.AppendText(".\\CompareMDBs.log"))
            {
                w.WriteLine(DateTime.Now + " " + textToAdd);
            }

        }

        public static void insertTables(string path_from, string path_to, List<string> _tables)
        {

            string conString = ("Provider=Microsoft.JET.OLEDB.4.0;data source=" + path_to + ";Persist Security Info=False;");
            OleDbConnection dbconn = new OleDbConnection(conString);
            dbconn.Open();
            OleDbCommand dbcommand = new OleDbCommand();

            _tables.ForEach(delegate(String name)
            {
                string selQuery = "SELECT f.* INTO " + name + " FROM " + name + " AS f IN '" + path_from + "';";

                dbcommand.CommandText = selQuery;
                dbcommand.CommandType = CommandType.Text;
                dbcommand.Connection = dbconn;
                int result = ConvertFromDBVal<int>(dbcommand.ExecuteNonQuery()); //dbcommand.ExecuteNonQuery();
                addToLog("Таблица " + name + " была импортирована в базу " + path_to.Except(".\\"));
            });
            
            dbconn.Close();
        }

        public static void dropThisShit(string path, string _tableForDel, bool _checkIsExch)
        {
            string conString = ("Provider=Microsoft.JET.OLEDB.4.0;data source=" + path + ";Persist Security Info=False;");
            OleDbConnection dbconn = new OleDbConnection(conString);
            dbconn.Open();
            OleDbCommand dbcommand = new OleDbCommand();

            List<string> allColumns = new List<string>();

            DataTable columns = dbconn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns,
                                                        new object[] { null, null, _tableForDel, null });

            bool hasTime = false;
            int counter = 0;
            foreach (DataRow row in columns.Rows)
            {
                allColumns.Add(row["COLUMN_NAME"].ToString());
                if (row["COLUMN_NAME"].ToString() == "time") hasTime = true;
                counter++;
            }

            if (hasTime && _checkIsExch)
            {
                string selQuery = "SELECT count(*) FROM " + _tableForDel + " where time is not null;";
                dbcommand.CommandText = selQuery;
                dbcommand.CommandType = CommandType.Text;
                dbcommand.Connection = dbconn;

                int RowsAffected = (int)dbcommand.ExecuteScalar();
                if (RowsAffected > 0)
                    MessageBox.Show("В таблице " + _tableForDel + " есть записи, которые не были отправлены на сервер. Таблицу удалить нельзя!" +
                                    "\n Сделайте от пользователя отправку данных за период и повторите попытку.");
                else
                {
                    string dropQuery = "DROP TABLE " + _tableForDel + ";";
                    dbcommand.CommandText = dropQuery;
                    dbcommand.CommandType = CommandType.Text;
                    dbcommand.Connection = dbconn;
                    int result = ConvertFromDBVal<int>(dbcommand.ExecuteNonQuery()); //dbcommand.ExecuteNonQuery();
                    //MessageBox.Show("Table " + _tableForDel + " was dropped!");
                    if (path.Contains("Teamsoft.mdb"))
                    {
                        addToLog("Таблица " + _tableForDel + " была удалена");
                        MessageBox.Show("Таблица " + _tableForDel + " была удалена!");
                    }
                    ///////////////////////////////////////////////////////////////msgbox?
                }
            }
            else
            {
                string dropQuery2 = "DROP TABLE " + _tableForDel + ";";
                dbcommand.CommandText = dropQuery2;
                dbcommand.CommandType = CommandType.Text;
                dbcommand.Connection = dbconn;
                int result = ConvertFromDBVal<int>(dbcommand.ExecuteNonQuery());//dbcommand.ExecuteNonQuery();
                if (_checkIsExch)
                {
                    //MessageBox.Show("Table " + _tableForDel + " was dropped!");
                    addToLog("Таблица " + _tableForDel + " была удалена");
                    MessageBox.Show("Таблица " + _tableForDel + " была удалена!");
                }
            }
            dbconn.Close();
        }

        public static List<string> getTablesWithoutPK(string _path)
        {
            string conString = ("Provider=Microsoft.JET.OLEDB.4.0;data source=" + _path + ";Persist Security Info=False;");
            OleDbConnection dbconn = new OleDbConnection(conString);
            dbconn.Open();
            OleDbCommand dbcommand = new OleDbCommand();

            var returnList = new List<string>();
            var allTables = GetDBTables(_path);

            int counter = 0;
            bool switcher;// true means that we need add a PK, false - we don't
            allTables.ForEach(delegate(String name)
            {
                switcher = true;
                DataTable mySchema = dbconn.GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys,
                                    new Object[] { null, null, allTables[counter] });

                foreach (DataRow r in mySchema.Rows)
                {
                    switcher = false;
                }

                // if switcher is true and it's 'info' or 'rep' table, then add current table lo list
                if (switcher && (allTables[counter].Contains("info_") || allTables[counter].Contains("rep_")))
                {
                    returnList.Add(allTables[counter]);
                    //addToLog("У таблицы " + allTables[counter] + " нет первичного ключа");
                }
                counter++;
            });

            dbconn.Close();

            return returnList;
        }

        public static void prepareDB(string _path)
        {
            string conString = ("Provider=Microsoft.JET.OLEDB.4.0;data source=" + _path + ";Persist Security Info=False;");
            OleDbConnection dbconn = new OleDbConnection(conString);
            dbconn.Open();
            OleDbCommand dbcommand = new OleDbCommand();
            dbcommand.CommandType = CommandType.Text;
            dbcommand.Connection = dbconn;
            
            var tablesWithoutPK = getTablesWithoutPK(_path);
            if (tablesWithoutPK.Count != 0)
                tablesWithoutPK.ForEach(delegate(String name)
                {
                    string deleteNotNumeric = "Delete from " + name +
                                              " where not isNumeric(id) or id < 1" +        //deleting rows with not positive or not numeric ID
                                              " or guid is null or currenttime is null";    //and rows without currenttime and guid
                    string checkRowsDuplicated = "SELECT COUNT(*)"                          //total count of duplicated rows in tables
                                                + " FROM ( "
                                                + " SELECT DISTINCT id"
                                                + " FROM " + name
                                                + " group by id "
                                                + " having count(*) > 1);";
                    string selectIntoTemp = "Select distinct old.* "                        //one of duplicated rows inserted into temporary table
                                            + "into temp from " + name + " as old "
                                            + "where id in (select id from " + name + " group by id "
                                            + "having count(*) > 1)";
                    string delDuplicates = "delete from " + name + " where id in "          //all duplicated rows deleting from original table
                                            + "(select id from " + name + " group by id "
                                            + "having count(*) > 1)";
                    string restoreFromTemp = "insert into " + name                          //rows from temporary table inserted into original table
                                            + " select * from temp";
                    string dropTemp = "drop table temp";                                    //drop temporary table

                    dbcommand.CommandText = deleteNotNumeric;
                    int result = ConvertFromDBVal<int>(dbcommand.ExecuteScalar());
                    if (result > 0)
                        addToLog("Из таблицы " + name + " удалено" + result + " записей с нечисловыми значениями id");

                    dbcommand.CommandText = checkRowsDuplicated;
                    int rowsDuplicated = (int)dbcommand.ExecuteScalar();
                    if (rowsDuplicated != 0)
                    {
                        addToLog("В таблице " + name + " обнаружены задублировавшиеся строки");
                        string checkRowsFullCopy = "SELECT COUNT(*)"
                                                + " FROM ("
                                                + " SELECT DISTINCTROW id "
                                                + " FROM " + name
                                                + " WHERE id IN"
                                                + " (SELECT DISTINCT id"
                                                + " FROM " + name
                                                + " group by id"
                                                + " having count(*) > 1))";
                        dbcommand.CommandText = checkRowsFullCopy;
                        int rowsFullCopy = (int)dbcommand.ExecuteScalar();

                        if (rowsFullCopy == rowsDuplicated)
                        {
                            dbcommand.CommandText = selectIntoTemp;
                            result = ConvertFromDBVal<int>(dbcommand.ExecuteScalar());

                            dbcommand.CommandText = delDuplicates;
                            result = ConvertFromDBVal<int>(dbcommand.ExecuteScalar());

                            dbcommand.CommandText = restoreFromTemp;
                            result = ConvertFromDBVal<int>(dbcommand.ExecuteScalar());

                            dbcommand.CommandText = dropTemp;
                            result = ConvertFromDBVal<int>(dbcommand.ExecuteScalar());
                            addToLog("Задублировавшиеся строки из таблицы " + name + " удалены");
                        }
                        else
                        {
                            dbcommand.CommandText = delDuplicates;
                            result = ConvertFromDBVal<int>(dbcommand.ExecuteScalar());
                            addToLog("Задублировавшиеся строки из таблицы " + name + " удалены");
                        }
                    }

                });

            //clear info_exception
            string clearExceptions = "DELETE FROM info_exception";
            dbcommand.CommandText = clearExceptions;
            int result2 = ConvertFromDBVal<int>(dbcommand.ExecuteScalar());
            addToLog("Записи из info_exception удалены");

            dbconn.Close();
        }

        public static void setPrimaryKeys(string _path)
        {
            string conString = ("Provider=Microsoft.JET.OLEDB.4.0;data source=" + _path + ";Persist Security Info=False;");
            OleDbConnection dbconn = new OleDbConnection(conString);
            dbconn.Open();
            OleDbCommand dbcommand = new OleDbCommand();

            var tablesWithoutPK = getTablesWithoutPK(_path);

            if (tablesWithoutPK.Count == 0)
                //MessageBox.Show("All tables have primary key!");
                //MessageBox.Show("Во всех таблицах есть первичный ключ!");
                addToLog("Во всех таблицах есть первичный ключ");
            else
            {
                tablesWithoutPK.ForEach(delegate(String name)
                {
                    addToLog("В таблице " + name + " не найден первичный ключ");
                    string alterTable = "ALTER TABLE " + name + " ADD PRIMARY KEY (id);";

                    dbcommand.CommandText = alterTable;
                    dbcommand.CommandType = CommandType.Text;
                    dbcommand.Connection = dbconn;
                    int result = ConvertFromDBVal<int>(dbcommand.ExecuteNonQuery());  //dbcommand.ExecuteNonQuery();
                    addToLog("Первичный ключ для таблицы " + name + " установлен");
                });

                //var message = string.Join(Environment.NewLine, tablesWithoutPK.ToArray());
                //MessageBox.Show("Primary key was added for following tables: " + message);
                //MessageBox.Show("Первичный ключ был добавлен для следующих таблиц: " + message);
            }

            dbconn.Close();
        }

        public static void compactAndRepair(string _path)
        {
            string conString = ("Provider=Microsoft.JET.OLEDB.4.0;data source=" + _path + ";Persist Security Info=False;");
            OleDbConnection dbconn = new OleDbConnection(conString);

            addToLog("Начато сжатие базы данных");
            object[] oParams;

            object objJRO = Activator.CreateInstance(Type.GetTypeFromProgID("JRO.JetEngine"));

            //filling Parameters array
            //cnahge "Jet OLEDB:Engine Type=5" to an appropriate value
            // or leave it as is if you db is JET4X format (access 2000,2002)
            //(yes, jetengine5 is for JET4X, no misprint here)
            oParams = new object[] {
                conString,
                "Provider=Microsoft.Jet.OLEDB.4.0;Data" + 
                " Source=.\\tempdb.mdb;Jet OLEDB:Engine Type=5"};

            //invoke a CompactDatabase method of a JRO object
            //pass Parameters array
            objJRO.GetType().InvokeMember("CompactDatabase",
                BindingFlags.InvokeMethod,
                null,
                objJRO,
                oParams);

            //database is compacted now to a new file C:\\tempdb.mdw
            //let's copy it over an old one and delete it

            System.IO.File.Delete(_path);
            System.IO.File.Move(".\\tempdb.mdb", _path);

            //clean up (just in case)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(objJRO);
            objJRO = null;
            if (dbconn != null)
                dbconn.Close();
            addToLog("Сжатие базы данных завершено");
        }

        public static void CreateNewAccessDatabase(string _path) // NEED ADD CHECK FOR A FREE DISK SPACE HERE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        {
            /*
            try
            {
                string currentdir = Directory.GetCurrentDirectory();
                //DriveInfo driveInfo = new DriveInfo(Directory.GetCurrentDirectory().First(1).ToString());
                //long FreeSpace = driveInfo.AvailableFreeSpace;
            }
            catch (System.IO.IOException errorMesage)
            {
                Console.WriteLine(errorMesage);
            }
            */
            ADOX.Catalog cat = new ADOX.Catalog();

            cat.Create("Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + _path + "; Jet OLEDB:Engine Type=5");
                
            //Now Close the database
            ADODB.Connection con = cat.ActiveConnection as ADODB.Connection;
            if (con != null)
                con.Close();
            cat = null;
            Program.checkCompactError(_path);
        }
       
        public static void CreateIndexes(string _path)
        {
            //pk_info_taskpreparation - for PKs
            //ix_info_taskpreparation_task_id, ix_info_taskpreparation_guid - for _id and guid
            string conString = ("Provider=Microsoft.JET.OLEDB.4.0;data source=" + _path + ";Persist Security Info=False;");
            using (OleDbConnection con = new OleDbConnection())
            {
                var allTables = GetDBTables(_path);
                List<string> tableIndexes = new List<string>();
                DataTable columns;

                con.ConnectionString = conString;
                con.Open();

                allTables.ForEach(delegate(String currentTable)
                {
                    tableIndexes.Clear();
                    columns = con.GetOleDbSchemaTable(OleDbSchemaGuid.Columns,
                                                        new object[] { null, null, currentTable, null });
                    
                    //getting a list of all indexes for current table
                    string[] restrictions = new string[5];
                    restrictions[4] = currentTable;
                    System.Data.DataTable indexes = con.GetSchema("Indexes", restrictions);
                    foreach (DataRow rowIndex in indexes.Rows)
                    {
                        tableIndexes.Add(rowIndex["COLUMN_NAME"].ToString());
                    }


                    //check if column is a foreign key or 'guid' or primary key. if so, check if index exists for this column
                    //if no - create unique asc index
                    string createIndex;
                    foreach(DataRow rowColumn in columns.Rows)
                    {
                        string currentField = rowColumn["COLUMN_NAME"].ToString();
                        if ((currentField.EndsWith("_id") || currentField == "guid")
                            && !tableIndexes.Contains(currentField))
                        {
                            createIndex = "CREATE INDEX ix_" + currentTable + "_" + currentField +
                                                  " ON " + currentTable + " ([" + currentField + "] ASC)";
                            OleDbCommand dbcommand = new OleDbCommand();
                            dbcommand.CommandText = createIndex;
                            dbcommand.CommandType = CommandType.Text;
                            dbcommand.Connection = con;
                            int result = ConvertFromDBVal<int>(dbcommand.ExecuteNonQuery());  //dbcommand.ExecuteNonQuery();
                            addToLog("В таблицу " + currentTable + " добавлен индекс по полю " + currentField);
                        }
                        if (currentField == "id"
                            && !tableIndexes.Contains(currentField))
                        {
                            createIndex = "CREATE UNIQUE INDEX pk_" + currentTable +
                                                  " ON " + currentTable + " (id ASC) WITH PRIMARY";
                            OleDbCommand dbcommand = new OleDbCommand();
                            dbcommand.CommandText = createIndex;
                            dbcommand.CommandType = CommandType.Text;
                            dbcommand.Connection = con;
                            int result = ConvertFromDBVal<int>(dbcommand.ExecuteNonQuery());  //dbcommand.ExecuteNonQuery();
                            addToLog("В таблицу " + currentTable + " добавлен первичный ключ");
                        }

                    }
                });
                con.Close();
            }
        }

        public static void checkCompactError (string _path)
        {
            if (Program.GetDBTables(_path).Contains("MSysCompactError"))
            {
                string conString = ("Provider=Microsoft.JET.OLEDB.4.0;data source=" + _path + ";Persist Security Info=False;");
                OleDbConnection conn = new OleDbConnection(conString);
                conn.Open();
                OleDbCommand dbcommand = new OleDbCommand();

                string selErrorTables = "SELECT DISTINCT ErrorTable FROM MSysCompactError";
                string corruptedTables = "";

                dbcommand.CommandText = selErrorTables;
                dbcommand.CommandType = CommandType.Text;
                dbcommand.Connection = conn;
                try
                {
                    OleDbDataReader dr = dbcommand.ExecuteReader();
                    while (dr.Read())
                    {
                        string currentDr = dr[0].ToString();
                        corruptedTables = corruptedTables + "\n" + currentDr;
                    }
                }
                catch (OleDbException oError)
                {
                    MessageBox.Show("Oops! " + oError.ToString());
                }
                conn.Close();

                addToLog("В БАЗЕ ДАННЫХ " + _path.Replace(".\\", "") + " МОГУТ БЫТЬ ПОВРЕЖДЁННЫЕ ТАБЛИЦЫ:" + corruptedTables);
                MessageBox.Show("В базе данных " + _path.Replace(".\\", "") + " могут быть повреждённые таблицы:" + corruptedTables);
            }
        }
    }
}
