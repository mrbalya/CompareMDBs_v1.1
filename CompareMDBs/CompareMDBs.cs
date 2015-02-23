using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CompareMDBs
{
    public partial class CompareMDBs : Form
    {
        public CompareMDBs()
        {
            InitializeComponent();
            this.Text = "Compare MDBs";
            this.MinimumSize = new Size(555, 310);
            this.listView1.View = View.Details;
            this.listView1.Columns.Add("Table name", listView1.Width, HorizontalAlignment.Left);
            this.pckr_dueDate.Format = DateTimePickerFormat.Custom;
            this.pckr_dueDate.CustomFormat = "yyyy-MM-dd";
            this.pckr_dueDate.Value = DateTime.Today.AddYears(-1);
            Compare.SelectedIndexChanged += new EventHandler(Tabs_SelectedIndexChanged); //handling of tab selected
        }

        private void Tabs_SelectedIndexChanged(object sender, EventArgs e)//handling of tab selected
        {
            if (Compare.SelectedTab == tab_delOldData)
            {
                if (!File.Exists(".\\Teamsoft.mdb"))
                    MessageBox.Show("File Teamsoft.mdb doesn't exist in current directory.");
                else
                {
                    List<string> allTables = new List<string>();
                    //array with tables that should be displayed at the top of CheckedListView
                    string[] highPriorityTables = new string[] {"info_task", "info_contactpotential", "info_companypreparation2",
                                                "info_taskpreparation", "info_taskmaterial", "info_action", "info_companypromo"};

                    allTables = Program.GetDBTables(".\\Teamsoft.mdb");
                    //check if high priority tables exists in current DB and add it to listview
                    for (int i = 0; i < highPriorityTables.Count(); i++)
                        if (!lst_allTables.Items.Contains(highPriorityTables[i].ToString()))
                        lst_allTables.Items.Add(highPriorityTables[i]);
                    //check if table is an "info_" table and add it to listview if it isn't there yet
                    allTables.ForEach(delegate(String name)
                    {
                        if (!lst_allTables.Items.Contains(name) && name.StartsWith("info_"))
                            lst_allTables.Items.Add(name);
                    });
                }
            }
        }
        
        //compare MDBs
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                listView1.Items.Clear();

                if (!File.Exists(".\\empty.mdb") || !File.Exists(".\\Teamsoft.mdb"))
                    MessageBox.Show("Files empty.mdb or Teamsoft.mdb don't exist in current directory.");
                else
                {
                    var empty = Program.GetDBTables(".\\empty.mdb");
                    var teamsoft = Program.GetDBTables(".\\Teamsoft.mdb");
                    var NotFoundInTeamsoft = empty.Except(teamsoft).ToList();

                    int counter = 0;
                    NotFoundInTeamsoft.ForEach(delegate(String name)
                    {
                        listView1.Items.Add(name, counter);
                        counter++;
                    });

                    Program.insertTables(".\\empty.mdb", ".\\Teamsoft.mdb", NotFoundInTeamsoft);
                    MessageBox.Show("All tables have been inserted!");

                    Program.prepareDB(".\\Teamsoft.mdb");
                    Program.setPrimaryKeys(".\\Teamsoft.mdb");

                    try
                    {
                        Program.compactAndRepair(".\\Teamsoft.mdb");
                        MessageBox.Show("Database compacted!");
                    }
                    catch (Exception e2)
                    {
                        MessageBox.Show(e2.Message.ToString());
                    }
                }
            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Oops! " + ex.Message + " \n Try open database with MS Access.");

            }
        }

        //drop table
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(".\\Teamsoft.mdb"))
                    MessageBox.Show("File Teamsoft.mdb doesn't exist in current directory.");
                else
                {
                    if (Program.GetDBTables(".\\Teamsoft.mdb").Contains(txtDelTable.Text.ToString()))
                        Program.dropThisShit(".\\Teamsoft.mdb", txtDelTable.Text.ToString(), true);
                    else MessageBox.Show("Table with name " + txtDelTable.Text.ToString() +
                                        " couldn't be found in database. Try drop another table.");
                }
            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Oops! " + ex.Message + " \n Try open database with MS Access.");

            }
        }

        //delete old data
        private void btn_deleteOldData_Click(object sender, EventArgs e)
        {
            string conString = ("Provider=Microsoft.JET.OLEDB.4.0;data source= .\\teamsoft.mdb;Persist Security Info=False;");
            OleDbConnection dbconn = new OleDbConnection(conString);
            dbconn.Open();
            OleDbCommand dbcommand = new OleDbCommand();

            foreach (Object item in lst_allTables.CheckedItems)
            {
                DataTable columns = dbconn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns,
                                                        new object[] { null, null, item.ToString(), null });
                string delQuery = "";
                var schema = dbconn.GetSchema("COLUMNS");
                string dueDate = pckr_dueDate.Value.ToString("yyyy-MM-dd");
                //choose values in what column should be compared with selected date
                if (schema.Select("TABLE_NAME='" + item.ToString() + "' AND COLUMN_NAME='datefrom'").Length > 0)
                    delQuery = "DELETE FROM " + item.ToString() + " where datefrom < #" + dueDate + "#";
                else if (schema.Select("TABLE_NAME='" + item.ToString() + "' AND COLUMN_NAME='date'").Length > 0)
                    delQuery = "DELETE FROM " + item.ToString() + " where date < #" + dueDate + "#";
                else
                    delQuery = "DELETE FROM " + item.ToString() + " where currenttime < #" + dueDate + "#";
                
                dbcommand.CommandText = delQuery;
                dbcommand.CommandType = CommandType.Text;
                dbcommand.Connection = dbconn;

                int result = Program.ConvertFromDBVal<int>(dbcommand.ExecuteNonQuery()); 
                //MessageBox.Show("Old values from table '" + item.ToString() + "' were deleted!");
            }
            dbconn.Close();
            Program.compactAndRepair(".\\Teamsoft.mdb");
            MessageBox.Show("Old values from all table were deleted!");
        }
        
        //export tables checked in listview to new MDB
        private void btn_exportToNewMDB_Click(object sender, EventArgs e)
        {
            if (lst_allTables.CheckedItems.Count == 0)
                MessageBox.Show("There's no tables to import! Please check tables in list and try again.");
            else
            {
                if (!File.Exists(".\\exported.mdb"))
                    Program.CreateNewAccessDatabase(".\\exported.mdb");
                List<string> lstToExport = new List<string>();
                lstToExport.Clear();
                foreach (Object item in lst_allTables.CheckedItems)
                {
                    if (Program.GetDBTables(".\\exported.mdb").Contains(item.ToString()))
                        Program.dropThisShit(".\\exported.mdb", item.ToString(), false);
                    lstToExport.Add(item.ToString());
                }

                Program.insertTables(".\\Teamsoft.mdb", ".\\exported.mdb", lstToExport);
                Program.compactAndRepair(".\\exported.mdb");
                MessageBox.Show("All selcted tables were exported into 'export.mdb'");
            }
        }

        //compact and repair Teamsoft.mdb + add indexes
        private void btn_compact_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(".\\Teamsoft.mdb"))
                    MessageBox.Show("File Teamsoft.mdb doesn't exist in current directory.");
                else
                {
                    Program.prepareDB(".\\Teamsoft.mdb");
                    Program.compactAndRepair(".\\Teamsoft.mdb");
                    Program.CreateIndexes(".\\Teamsoft.mdb");
                }
            }
            catch(OleDbException ex)
            {
                MessageBox.Show("Oops! \n" + ex.Message + " \n Try compare MDB with ");
            }
        }

    }
}
