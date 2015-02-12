using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
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
            Application.Run(new Form1());
            
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
                    MessageBox.Show("В таблице " + _tableForDel + " есть записи, которые не были отправлены на сервер. Таблицу удалить нельзя!");
                else
                {
                    string dropQuery = "DROP TABLE " + _tableForDel + ";"; 
                    dbcommand.CommandText = dropQuery;
                    dbcommand.CommandType = CommandType.Text;
                    dbcommand.Connection = dbconn;
                    int result = ConvertFromDBVal<int>(dbcommand.ExecuteNonQuery()); //dbcommand.ExecuteNonQuery();
                    MessageBox.Show("Table " + _tableForDel + " was dropped!");
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
                    MessageBox.Show("Table " + _tableForDel + " was dropped!");
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
                    returnList.Add(allTables[counter]);

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
                    string checkRowsDuplicated = "SELECT COUNT(*)"               //total count of duplicated rows in tables
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

                    dbcommand.CommandText = checkRowsDuplicated;
                    int rowsDuplicated = (int)dbcommand.ExecuteScalar();
                    if (rowsDuplicated != 0)
                    {
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
                        }
                        else
                        {
                            dbcommand.CommandText = delDuplicates;
                            result = ConvertFromDBVal<int>(dbcommand.ExecuteScalar());
                        }
                    }

                });

            //clear info_exception
            string clearExceptions = "DELETE FROM info_exception";
            dbcommand.CommandText = clearExceptions;
            int result2 = ConvertFromDBVal<int>(dbcommand.ExecuteScalar());

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
                MessageBox.Show("All tables have primary key!");
            else
            {
                tablesWithoutPK.ForEach(delegate(String name)
                {
                    string alterTable = "ALTER TABLE " + name + " ADD PRIMARY KEY (id);";

                    dbcommand.CommandText = alterTable;
                    dbcommand.CommandType = CommandType.Text;
                    dbcommand.Connection = dbconn;
                    int result = ConvertFromDBVal<int>(dbcommand.ExecuteNonQuery());  //dbcommand.ExecuteNonQuery();
                });

                var message = string.Join(Environment.NewLine, tablesWithoutPK.ToArray());
                MessageBox.Show("Primary key was added for following tables: " + message);
            }

            dbconn.Close();
        }

        public static void compactAndRepair(string _path)
        {
            string conString = ("Provider=Microsoft.JET.OLEDB.4.0;data source=" + _path + ";Persist Security Info=False;");
            OleDbConnection dbconn = new OleDbConnection(conString);

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
        }

        public static void CreateNewAccessDatabase(string _path)
        {
            string conString = ("Provider=Microsoft.JET.OLEDB.4.0;data source=" + _path + ";Persist Security Info=False;");
            

            ADOX.Catalog cat = new ADOX.Catalog();
            ADOX.Table table = new ADOX.Table();

            cat.Create("Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + _path + "; Jet OLEDB:Engine Type=5");
                
            //Now Close the database
            ADODB.Connection con = cat.ActiveConnection as ADODB.Connection;
            if (con != null)
                con.Close();
            cat = null;
        }
       
        public static void CreateIndexes(string _path)
        {
            string conString = ("Provider=Microsoft.JET.OLEDB.4.0;data source=" + _path + ";Persist Security Info=False;");
            using (OleDbConnection con = new OleDbConnection())
            {
                con.ConnectionString = conString;
                con.Open();
                object[] restrictions = new object[3];
                System.Data.DataTable table = con.GetOleDbSchemaTable(OleDbSchemaGuid.Indexes, restrictions);

                foreach (System.Data.DataRow row in table.Rows)
                {
                    string tableName = row[2].ToString();
                    if (tableName.Contains("rep_"))
                    {
                        foreach (System.Data.DataColumn col in table.Columns)
                        {
                            Console.WriteLine(tableName + "_-_" + "{0} = {1}",
                              col.ColumnName, row[col]);
                        }
                        Console.WriteLine("============================");
                    }
                }
                con.Close();
            }
        }
    }
}
