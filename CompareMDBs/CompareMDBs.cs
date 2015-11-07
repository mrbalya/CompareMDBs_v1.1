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

            this.lst_allTables.View = View.Details;
            this.lst_allTables.CheckBoxes = true;
            this.lst_allTables.GridLines = true;
            this.lst_allTables.Columns.Add("", -2, HorizontalAlignment.Left);
            this.lst_allTables.Columns.Add("Table name", -2, HorizontalAlignment.Left);
            this.lst_allTables.Columns.Add("Rows number", -2, HorizontalAlignment.Left);
            //this.lst_allTables.Sorting = SortOrder.Ascending;
            
            //Compare.SelectedIndexChanged += new EventHandler(Tabs_SelectedIndexChanged); //handling of tab selected
            Compare.Selected += new TabControlEventHandler(Tabs_Selected); //handling of tab selected
            Program.addToLog("******** Запуск программы ********");
        }
        private void CompareMDBs_ResizeEnd(object sender, EventArgs e)
        {
            Compare.Refresh();
        }
        private void Tabs_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage.Name == "tab_delOldData")
            {
                lbl_conditionTab3.Text = "";
                lbl_conditionTab3.Refresh();
                progressBar3.Value = 0;

                if (!File.Exists(".\\Teamsoft.mdb"))
                    MessageBox.Show("Файл 'Teamsoft.mdb' не существует в текущей папке.");
                else
                {
                    List<string> allTables = new List<string>();
                    //array with tables that should be displayed at the top of CheckedListView
                    /*
                    string[] highPriorityTables = new string[] {"info_task", "info_contactpotential", "info_companypreparation2",
                                                "info_taskpreparation", "info_taskmaterial", "info_action", "info_companypromo"};
                    */
                    allTables = Program.GetDBTables(".\\Teamsoft.mdb");
                    /*                    
                    List<string> selectedTables = new List<string>();
                    if (lst_allTables.Items.Count > 0 && 
                        lst_allTables.Items.Count != allTables.Count &&
                        lst_allTables.CheckedItems.Count > 0)
                    {
                        foreach (var Item in lst_allTables.CheckedItems)
                                selectedTables.Add(Item.ToString());
                        lst_allTables.Items.Clear();
                    }*/
                    /*
                    //check if high priority tables exists in current DB and add it to listview
                    for (int i = 0; i < highPriorityTables.Count(); i++)
                        allTables.ForEach(delegate(String name)
                        {
                            if (name == highPriorityTables[i].ToString())
                            {
                                List<string> subitemsTables = new List<string>();
                                foreach (ListViewItem item in lst_allTables.Items)
                                {
                                    foreach (ListViewItem.ListViewSubItem subItem in item.SubItems)
                                    {
                                        subitemsTables.Add(subItem.Text);
                                    }
                                }
                                if (!subitemsTables.Contains(name))
                                {
                                    ListViewItem lstViewItem = new ListViewItem();
                                    lstViewItem.SubItems.Add(highPriorityTables[i].ToString());
                                    lstViewItem.SubItems.Add(Program.SelectRowsCount(".\\Teamsoft.mdb", highPriorityTables[i].ToString()).ToString());
                                    lst_allTables.Items.Add(lstViewItem);
                                    lstViewItem = null;
                                }
                                subitemsTables = null;
                            }
                        });
                    */      
                    //check if table is an "info_" table and add it to listview if it isn't there yet
                    allTables.ForEach(delegate(String name)
                    {
                        List<string> subitemsTables = new List<string>();
                        foreach (ListViewItem item in lst_allTables.Items)
                        {
                            foreach (ListViewItem.ListViewSubItem subItem in item.SubItems)
                            {
                                subitemsTables.Add(subItem.Text);
                            }
                        }
                        if (!subitemsTables.Contains(name) && name.StartsWith("info_"))
                        {                            
                            ListViewItem lstViewItem = new ListViewItem();
                            lstViewItem.SubItems.Add(name);
                            lstViewItem.SubItems.Add(Program.SelectRowsCount(".\\Teamsoft.mdb", name).ToString());
                            lst_allTables.Items.Add(lstViewItem);
                            lstViewItem = null;
                        }
                        subitemsTables = null;
                        /*                        
	                    if (!lst_allTables.Items.Contains(name) && name.StartsWith("info_"))
		                    lst_allTables.Items.Add(name);
	                    if (selectedTables.Contains(name))
		                    lst_allTables.SetItemChecked(lst_allTables.Items.IndexOf(name), true);
                        */
                    });
                    
                    lst_allTables.Refresh();
                }
                lst_allTables.ListViewItemSorter = new Sorter(2);
            }

            if (e.TabPage.Name == "tabPage1")
            {
                lbl_conditionTab1.Text = "";
                lbl_conditionTab1.Refresh();
            }

            if (e.TabPage.Name == "tabPage2")
            {
                lbl_conditionTab2.Text = "";
                lbl_conditionTab2.Refresh();
                lbl_rowsCount.Text = "";
                lbl_rowsCount.Refresh();
                progressBar2.Value = 0;

                if (!File.Exists(".\\Teamsoft.mdb"))
                    MessageBox.Show("Файл 'Teamsoft.mdb' не существуют в текущей папке.");
                else
                {
                    if (Program.GetDBTables(".\\Teamsoft.mdb").Contains("rep_del"))
                    {
                        lbl_rowsCount.Text = Program.SelectRowsCount(".\\Teamsoft.mdb", "rep_del").ToString();
                        lbl_rowsCount.Refresh();
                    }
                    else
                    {
                        lbl_rowsCount.Text = "в базе нет rep_del'a =(";
                        lbl_rowsCount.Refresh();
                    }
                }
            }

            sender = null;
            e = null;
        }
        /*
        private void Tabs_SelectedIndexChanged(object sender, EventArgs e)//handling of tab selected
        {
            if (Compare.SelectedTab == tab_delOldData)
            {
                if (!File.Exists(".\\Teamsoft.mdb"))
                    //MessageBox.Show("File 'Teamsoft.mdb' doesn't exist in current directory.");
                    MessageBox.Show("Файл 'Teamsoft.mdb' не существует в текущей папке.");
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
        */
        //compare MDBs
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                listView1.Items.Clear();
                progressBar1.Value = 0;
                lbl_conditionTab1.Text = "";
                lbl_conditionTab1.Refresh();

                if (!File.Exists(".\\empty.mdb") || !File.Exists(".\\Teamsoft.mdb"))
                    //MessageBox.Show("Files empty.mdb or Teamsoft.mdb don't exist in current directory.");
                    MessageBox.Show("Файлы 'empty.mdb' или 'Teamsoft.mdb' не существуют в текущей папке.");
                else
                {
                    lbl_conditionTab1.Text = "Ищем недостающие таблицы";
                    lbl_conditionTab1.Refresh();
                    var empty = Program.GetDBTables(".\\empty.mdb");
                    var teamsoft = Program.GetDBTables(".\\Teamsoft.mdb");
                    var NotFoundInTeamsoft = empty.Except(teamsoft).ToList();

                    int counter = 0;
                    NotFoundInTeamsoft.ForEach(delegate(String name)
                    {
                        listView1.Items.Add(name, counter);
                        listView1.Refresh();
                        counter++;
                    });
                    progressBar1.Value += 2;

                    Program.insertTables(".\\empty.mdb", ".\\Teamsoft.mdb", NotFoundInTeamsoft);
                    //MessageBox.Show("All tables have been inserted!");
                    //MessageBox.Show("Все таблицы были импортированы!");
                    lbl_conditionTab1.Text = "Все таблицы были импортированы! Идёт подготовка БД, подождите.";
                    lbl_conditionTab1.Refresh();
                    progressBar1.Value += 3;

                    Program.prepareDB(".\\Teamsoft.mdb");
                    progressBar1.Value += 10;
                    lbl_conditionTab1.Text = "БД подготовлена. Устанавливаются первичные ключи.";
                    lbl_conditionTab1.Refresh();
                    Program.setPrimaryKeys(".\\Teamsoft.mdb");
                    progressBar1.Value += 35;

                    try
                    {
                        lbl_conditionTab1.Text = "Первичные ключи установлены. Идёт сжатие БД, подождите.";
                        lbl_conditionTab1.Refresh();
                        Program.compactAndRepair(".\\Teamsoft.mdb");
                        progressBar1.Value += 40;
                        //MessageBox.Show("Database compacted!");
                        //MessageBox.Show("Сжатие базы данных выполнено.");
                        lbl_conditionTab1.Text = "Сжатие базы данных выполнено. Идёт поиск недостающих индексов.";
                        lbl_conditionTab1.Refresh();

                        Program.CreateIndexes(".\\Teamsoft.mdb");
                        progressBar1.Value += 10;
                        lbl_conditionTab1.Text = "Готово!";
                        lbl_conditionTab1.Refresh();
                        progressBar1.Value = 0;

                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException.ToString().Length > 0)
                        {
                            Program.addToLog(ex.InnerException.Message);
                            MessageBox.Show(ex.InnerException.Message);
                        }
                        else
                        {
                            Program.addToLog(ex.Message);
                            MessageBox.Show(ex.Message);
                        }
                    }

                    if (lst_allTables.Items.Count > 0 && listView1.Items.Count > 0)
                    {
                        foreach (ListViewItem item in listView1.Items)
                        {
                            if (item.Text.StartsWith("info_"))
                            {
                                ListViewItem lstViewItem = new ListViewItem();
                                lstViewItem.SubItems.Add(item.Text);
                                lstViewItem.SubItems.Add(Program.SelectRowsCount(".\\Teamsoft.mdb", item.Text).ToString());
                                lst_allTables.Items.Add(lstViewItem);
                                lstViewItem = null;
                            }
                        }
                        lst_allTables.ListViewItemSorter = new Sorter(2);
                        lst_allTables.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException.ToString().Length > 0)
                {
                    Program.addToLog(ex.InnerException.Message);
                    MessageBox.Show("Упс! \n" + ex.InnerException.Message + " \n Попробуйте открыть базу в MS Access");
                }
                else
                {
                    Program.addToLog(ex.Message);
                    MessageBox.Show("Упс! \n" + ex.Message + " \n Попробуйте открыть базу в MS Access");
                }
            }
        }

        //drop table
        private void button2_Click(object sender, EventArgs e)
        {
            if (txtDelTable.Text.ToString().Length == 0)
                MessageBox.Show("Название таблицы не введено!");
            else
            {
                DialogResult dialogResult = System.Windows.Forms.DialogResult.Yes;
                if (saveNotSent.Checked == false)
                    dialogResult = MessageBox.Show("Подтвердите удаление таблицы " + txtDelTable.Text + " со всем содержимым. \n " +
                                                 "Если в таблице есть неотправленные данные, они также будут удалены!"
                                               , "To drop, or not to drop: that is the question", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        lbl_conditionTab2.Text = "";
                        lbl_conditionTab2.Refresh();
                        progressBar2.Value = 0;

                        if (!File.Exists(".\\Teamsoft.mdb"))
                            //MessageBox.Show("File 'Teamsoft.mdb' doesn't exist in current directory.");
                            MessageBox.Show("Файл 'Teamsoft.mdb' не существует в текущей папке.");
                        else
                        {
                            if (Program.GetDBTables(".\\Teamsoft.mdb").Contains(txtDelTable.Text.ToString()))
                            {
                                Program.dropThisShit(".\\Teamsoft.mdb", txtDelTable.Text.ToString(), saveNotSent.Checked);
                                if (lst_allTables.Items.Count > 0)
                                {
                                    foreach (ListViewItem item in lst_allTables.Items)
                                    {
                                        foreach (ListViewItem.ListViewSubItem subItem in item.SubItems)
                                        {
                                            if (subItem.Text == txtDelTable.Text.ToString())
                                            {
                                                lst_allTables.Items.Remove(item);
                                                break;
                                            }
                                        }
                                    }
                                }
                                progressBar2.Value = progressBar2.Maximum;
                                lbl_conditionTab2.Text = "Готово!";
                                lbl_conditionTab2.Refresh();
                                progressBar2.Value = 0;
                                if (Program.GetDBTables(".\\Teamsoft.mdb").Contains("rep_del"))
                                {
                                    lbl_rowsCount.Text = Program.SelectRowsCount(".\\Teamsoft.mdb", "rep_del").ToString();
                                    lbl_rowsCount.Refresh();
                                }
                                else
                                {
                                    lbl_rowsCount.Text = "в базе нет rep_del'a =(";
                                    lbl_rowsCount.Refresh();
                                }

                            }
                            else //MessageBox.Show("Table with name " + txtDelTable.Text.ToString() +
                                //               " couldn't be found in database. Try drop another table.");
                                MessageBox.Show("Таблицу с названием '" + txtDelTable.Text.ToString() +
                                            "' не удалось найти в базе данных. Проверьте, правильно ли ввели название таблицы.");
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException.ToString().Length > 0)
                        {
                            Program.addToLog(ex.InnerException.Message);
                            MessageBox.Show("Упс! \n" + ex.InnerException.Message + " \n Попробуйте открыть базу в MS Access");
                        }
                        else
                        {
                            Program.addToLog(ex.Message);
                            MessageBox.Show("Упс! \n" + ex.Message + " \n Попробуйте открыть базу в MS Access");
                        }
                    }
                }
            }      
        }

        //delete old data
        private void btn_deleteOldData_Click(object sender, EventArgs e)
        {
            progressBar3.Value = 0;
            lbl_conditionTab3.Text = "";
            lbl_conditionTab3.Refresh();

            string conString = ("Provider=Microsoft.JET.OLEDB.4.0;data source= .\\teamsoft.mdb;Persist Security Info=False;");
            OleDbConnection dbconn = new OleDbConnection(conString);
            dbconn.Open();
            OleDbCommand dbcommand = new OleDbCommand();

            if (lst_allTables.CheckedItems.Count > 0)
            {
                progressBar3.Maximum = lst_allTables.CheckedItems.Count;// +10;
                foreach (ListViewItem item in lst_allTables.CheckedItems)
                {
                    //System.Threading.Thread.Sleep(1000);
                    lbl_conditionTab3.Text = "Удаляются данные из таблицы " + item.SubItems[1].Text;
                    lbl_conditionTab3.Refresh();
                    //System.Threading.Thread.Sleep(1000);
                    DataTable columns = dbconn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns,
                                                            new object[] { null, null, item.ToString(), null });
                    string delQuery = "";
                    var schema = dbconn.GetSchema("COLUMNS");
                    string dueDate = pckr_dueDate.Value.ToString("yyyy-MM-dd");
                    //choose values in what column should be compared with selected date
                    if (schema.Select("TABLE_NAME='" + item.SubItems[1].Text + "' AND COLUMN_NAME='datefrom'").Length > 0)
                        delQuery = "DELETE FROM " + item.SubItems[1].Text + " where datefrom < #" + dueDate + "#";
                    else if (schema.Select("TABLE_NAME='" + item.SubItems[1].Text + "' AND COLUMN_NAME='date'").Length > 0)
                        delQuery = "DELETE FROM " + item.SubItems[1].Text + " where date < #" + dueDate + "#";
                    else
                        delQuery = "DELETE FROM " + item.SubItems[1].Text + " where currenttime < #" + dueDate + "#";

                    dbcommand.CommandText = delQuery;
                    dbcommand.CommandType = CommandType.Text;
                    dbcommand.Connection = dbconn;

                    int result = Program.ConvertFromDBVal<int>(dbcommand.ExecuteNonQuery());
                    //MessageBox.Show("Old values from table '" + item.ToString() + "' were deleted!");
                    Program.addToLog("Записи, внесённые ранее " + dueDate.ToString() + ", были удалены из таблицы " + item.SubItems[1].Text);
                    progressBar3.Value += 1;
                }
            dbconn.Close();

            lbl_conditionTab3.Text = "Старые данные из выбранных таблиц удалены. Выполняется обновление списка";
            foreach (ListViewItem item in lst_allTables.CheckedItems)
            {
                item.SubItems[2].Text = Program.SelectRowsCount(".\\Teamsoft.mdb", item.SubItems[1].Text).ToString();
                item.Checked = false;
            }
            lst_allTables.ListViewItemSorter = new Sorter(2);
            lst_allTables.Refresh();
            /*
            lbl_conditionTab3.Text = "Старые данные из выбранных таблиц удалены. Выполняется сжатие БД.";
            lbl_conditionTab3.Refresh();
            Program.compactAndRepair(".\\Teamsoft.mdb");
            progressBar3.Value += 7;
            lbl_conditionTab3.Text = "Сжатие БД завершено. Выполняется проставление недостающих индексов.";
            lbl_conditionTab3.Refresh();
            Program.CreateIndexes(".\\Teamsoft.mdb");
            progressBar3.Value += 3;
            */
            lbl_conditionTab3.Text = "Старые данные из выбранных таблиц удалены!";
            lbl_conditionTab3.Refresh();
            progressBar3.Value = 0;
            //MessageBox.Show("Old values from all selected tables were deleted!");
            //MessageBox.Show("Старые данные из выбранных таблиц были удалены!");
            }
            else MessageBox.Show("Хоть одну таблицу из списка выбрать всё-таки нужно.");
        }
        
        //export tables checked in listview to new MDB
        private void btn_exportToNewMDB_Click(object sender, EventArgs e)
        {
            lbl_conditionTab3.Text = "";
            lbl_conditionTab3.Refresh();
            progressBar3.Value = 0;
            progressBar3.Maximum = lst_allTables.CheckedItems.Count * 2 + 10;

            if (lst_allTables.CheckedItems.Count == 0)
                //MessageBox.Show("There's no tables to import! Please check tables in list and try again.");
                MessageBox.Show("Таблицы для импорта не выбраны! Отметьте таблицы в списке и поробуйте повторить.");
            else
            {
                if (!File.Exists(".\\exported.mdb"))
                    Program.CreateNewAccessDatabase(".\\exported.mdb");
                lbl_conditionTab3.Text = "БД подготовлена для импорта таблиц.";
                lbl_conditionTab3.Refresh();
                progressBar3.Value += 2;

                List<string> lstToExport = new List<string>();
                lstToExport.Clear();
                foreach (ListViewItem item in lst_allTables.CheckedItems)
                {
                    if (Program.GetDBTables(".\\exported.mdb").Contains(item.SubItems[1].Text))
                        Program.dropThisShit(".\\exported.mdb", item.SubItems[1].Text, false);
                    lstToExport.Add(item.SubItems[1].Text);
                    progressBar3.Value ++;
                }

                Program.insertTables(".\\Teamsoft.mdb", ".\\exported.mdb", lstToExport);
                progressBar3.Value += lst_allTables.CheckedItems.Count;
                lbl_conditionTab3.Text = "Выбранные таблицы экспортированы в файл 'export.mdb'. Идёт сжатие базы, подождите.";
                lbl_conditionTab3.Refresh();

                Program.compactAndRepair(".\\exported.mdb");
                progressBar3.Value += 8;
                //MessageBox.Show("All selcted tables were exported into 'export.mdb'");
                //MessageBox.Show("Выбранные таблицы экспортированы в файл 'export.mdb'");
                lbl_conditionTab3.Text = "Готово!";
                lbl_conditionTab3.Refresh();
                progressBar3.Value = 0;
            }
        }

        //compact and repair Teamsoft.mdb + add indexes
        private void btn_compact_Click(object sender, EventArgs e)
        {            
            progressBar1.Value = 0;
            lbl_conditionTab1.Text = "";
            lbl_conditionTab1.Refresh();

            if (!File.Exists(".\\Teamsoft.mdb"))
                //MessageBox.Show("File 'Teamsoft.mdb' doesn't exist in current directory.");
                MessageBox.Show("Файл 'Teamsoft.mdb' не существует в текущей папке.");
            else
            {
                progressBar1.Value += 5;

                try
                {
                    lbl_conditionTab1.Text = "Идёт подготовка БД.";
                    lbl_conditionTab1.Refresh();
                    Program.prepareDB(".\\Teamsoft.mdb");
                    progressBar1.Value += 25;

                    lbl_conditionTab1.Text = "БД подготовлена. Идёт сжатие БД, подождите.";
                    lbl_conditionTab1.Refresh();
                    Program.compactAndRepair(".\\Teamsoft.mdb");
                    progressBar1.Value += 50;

                    lbl_conditionTab1.Text = "Сжатие базы данных выполнено. Проставляем индексы.";
                    lbl_conditionTab1.Refresh();
                    Program.CreateIndexes(".\\Teamsoft.mdb");
                    progressBar1.Value += 20;
                    //MessageBox.Show("Сжатие базы данных выполнено, индексирование проведено.");
                    lbl_conditionTab1.Text = "Сжатие базы данных выполнено, индексирование проведено.";
                    lbl_conditionTab1.Refresh();
                    progressBar1.Value = 0;
                }
                catch (Exception ex)
                {
                    lbl_conditionTab1.Text = "Ошибочка вышла =(";
                    lbl_conditionTab1.Refresh();
                    progressBar1.Value = 0;
                    if (ex.InnerException.ToString().Length > 0)
                    {
                        Program.addToLog(ex.InnerException.ToString());
                        MessageBox.Show(ex.InnerException.ToString());
                    }
                    else
                    {
                        Program.addToLog(ex.ToString());
                        MessageBox.Show(ex.ToString());
                    }
                        
                }
            }
        }

        private void btn_compact3_Click(object sender, EventArgs e)
        {
            try
            {
                progressBar3.Value = 0;
                progressBar3.Maximum = 100;
                lbl_conditionTab3.Text = "";
                lbl_conditionTab3.Refresh();

                if (!File.Exists(".\\Teamsoft.mdb"))
                    //MessageBox.Show("File 'Teamsoft.mdb' doesn't exist in current directory.");
                    MessageBox.Show("Файл 'Teamsoft.mdb' не существует в текущей папке.");
                else
                {
                    progressBar3.Value += 5;

                    lbl_conditionTab3.Text = "Идёт подготовка БД.";
                    lbl_conditionTab3.Refresh();
                    Program.prepareDB(".\\Teamsoft.mdb");
                    progressBar3.Value += 25;

                    lbl_conditionTab3.Text = "БД подготовлена. Идёт сжатие БД, подождите.";
                    lbl_conditionTab3.Refresh();
                    try
                    {
                        Program.compactAndRepair(".\\Teamsoft.mdb");
                        progressBar3.Value += 50;

                        lbl_conditionTab3.Text = "Сжатие базы данных выполнено. Проставляем индексы.";
                        lbl_conditionTab3.Refresh();
                        Program.CreateIndexes(".\\Teamsoft.mdb");
                        progressBar3.Value += 20;
                        //MessageBox.Show("Сжатие базы данных выполнено, индексирование проведено.");
                        lbl_conditionTab3.Text = "Сжатие базы данных выполнено, индексирование проведено.";
                        lbl_conditionTab3.Refresh();
                        progressBar3.Value = 0;
                    }
                    catch (Exception ex)
                    {
                        lbl_conditionTab3.Text = "Ошибочка вышла =(";
                        lbl_conditionTab3.Refresh();
                        progressBar3.Value = 0;
                        if (ex.InnerException.ToString().Length > 0)
                        {
                            Program.addToLog(ex.InnerException.ToString());
                            MessageBox.Show(ex.InnerException.ToString());
                        }
                        else
                        {
                            Program.addToLog(ex.ToString());
                            MessageBox.Show(ex.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException.ToString().Length > 0)
                {
                    Program.addToLog(ex.InnerException.Message);
                    MessageBox.Show("Упс! \n" + ex.InnerException.Message + " \n Попробуйте сжать/востановить базу в MS Access");
                }
                else
                {
                    Program.addToLog(ex.Message);
                    MessageBox.Show("Упс! \n" + ex.Message + " \n Попробуйте сжать/востановить базу в MS Access");
                }
            }
        }

        private void btn_compact2_Click(object sender, EventArgs e)
        {
            progressBar2.Value = 0;
            progressBar2.Maximum = 100;
            lbl_conditionTab2.Text = "";
            lbl_conditionTab2.Refresh();

            if (!File.Exists(".\\Teamsoft.mdb"))
                //MessageBox.Show("File 'Teamsoft.mdb' doesn't exist in current directory.");
                MessageBox.Show("Файл 'Teamsoft.mdb' не существует в текущей папке.");
            else
            {
                progressBar2.Value += 5;

                try
                {
                    lbl_conditionTab2.Text = "Идёт подготовка БД.";
                    lbl_conditionTab2.Refresh();
                    Program.prepareDB(".\\Teamsoft.mdb");
                    progressBar2.Value += 25;

                    lbl_conditionTab2.Text = "БД подготовлена. Идёт сжатие БД, подождите.";
                    lbl_conditionTab2.Refresh();
                    Program.compactAndRepair(".\\Teamsoft.mdb");
                    progressBar2.Value += 50;

                    lbl_conditionTab2.Text = "Сжатие базы данных выполнено. Проставляем индексы.";
                    lbl_conditionTab2.Refresh();
                    Program.CreateIndexes(".\\Teamsoft.mdb");
                    progressBar2.Value += 20;
                    //MessageBox.Show("Сжатие базы данных выполнено, индексирование проведено.");
                    lbl_conditionTab2.Text = "Сжатие базы данных выполнено, индексирование проведено.";
                    lbl_conditionTab2.Refresh();
                    progressBar2.Value = 0;
                }
                catch (Exception ex)
                {
                    lbl_conditionTab2.Text = "Ошибочка вышла =(";
                    lbl_conditionTab2.Refresh();
                    progressBar2.Value = 0;
                    if (ex.InnerException.ToString().Length > 0)
                    {
                        Program.addToLog(ex.InnerException.ToString());
                        MessageBox.Show(ex.InnerException.ToString());
                    }
                    else
                    {
                        Program.addToLog(ex.ToString());
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }

    }
}
