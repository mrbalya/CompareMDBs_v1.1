namespace CompareMDBs
{
    partial class CompareMDBs
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.Compare = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.btn_compact = new System.Windows.Forms.Button();
            this.lbl_conditionTab1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lbl_conditionTab2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.btn_compact2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDelTable = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.tab_delOldData = new System.Windows.Forms.TabPage();
            this.btn_compact3 = new System.Windows.Forms.Button();
            this.lbl_conditionTab3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.progressBar3 = new System.Windows.Forms.ProgressBar();
            this.btn_exportToNewMDB = new System.Windows.Forms.Button();
            this.btn_deleteOldData = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pckr_dueDate = new System.Windows.Forms.DateTimePicker();
            this.lst_allTables = new System.Windows.Forms.CheckedListBox();
            this.Compare.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tab_delOldData.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(423, 240);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 25);
            this.button1.TabIndex = 0;
            this.button1.Text = "Compare MDBs";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listView1
            // 
            this.listView1.Alignment = System.Windows.Forms.ListViewAlignment.Default;
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Location = new System.Drawing.Point(6, 19);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(517, 213);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // Compare
            // 
            this.Compare.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Compare.Controls.Add(this.tabPage1);
            this.Compare.Controls.Add(this.tabPage2);
            this.Compare.Controls.Add(this.tab_delOldData);
            this.Compare.Location = new System.Drawing.Point(0, -2);
            this.Compare.Name = "Compare";
            this.Compare.SelectedIndex = 0;
            this.Compare.Size = new System.Drawing.Size(538, 343);
            this.Compare.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.btn_compact);
            this.tabPage1.Controls.Add(this.lbl_conditionTab1);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.progressBar1);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.listView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(530, 317);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Compare MDBs";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(241, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Список талиц, которые были импортированы:";
            // 
            // btn_compact
            // 
            this.btn_compact.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_compact.Location = new System.Drawing.Point(3, 240);
            this.btn_compact.Name = "btn_compact";
            this.btn_compact.Size = new System.Drawing.Size(100, 25);
            this.btn_compact.TabIndex = 8;
            this.btn_compact.Text = "Compact + index";
            this.btn_compact.UseVisualStyleBackColor = true;
            this.btn_compact.Click += new System.EventHandler(this.btn_compact_Click);
            // 
            // lbl_conditionTab1
            // 
            this.lbl_conditionTab1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbl_conditionTab1.AutoSize = true;
            this.lbl_conditionTab1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_conditionTab1.Location = new System.Drawing.Point(85, 268);
            this.lbl_conditionTab1.Name = "lbl_conditionTab1";
            this.lbl_conditionTab1.Size = new System.Drawing.Size(70, 15);
            this.lbl_conditionTab1.TabIndex = 4;
            this.lbl_conditionTab1.Text = "                     ";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(6, 268);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "Состояние:";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(6, 286);
            this.progressBar1.MarqueeAnimationSpeed = 2;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(517, 23);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 2;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lbl_conditionTab2);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.progressBar2);
            this.tabPage2.Controls.Add(this.btn_compact2);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.txtDelTable);
            this.tabPage2.Controls.Add(this.button2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(530, 317);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Удалить таблицу";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lbl_conditionTab2
            // 
            this.lbl_conditionTab2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbl_conditionTab2.AutoSize = true;
            this.lbl_conditionTab2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_conditionTab2.Location = new System.Drawing.Point(85, 268);
            this.lbl_conditionTab2.Name = "lbl_conditionTab2";
            this.lbl_conditionTab2.Size = new System.Drawing.Size(55, 15);
            this.lbl_conditionTab2.TabIndex = 8;
            this.lbl_conditionTab2.Text = "                ";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(6, 268);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 15);
            this.label6.TabIndex = 7;
            this.label6.Text = "Состояние:";
            // 
            // progressBar2
            // 
            this.progressBar2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar2.Location = new System.Drawing.Point(6, 286);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(517, 23);
            this.progressBar2.TabIndex = 6;
            // 
            // btn_compact2
            // 
            this.btn_compact2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_compact2.Location = new System.Drawing.Point(3, 240);
            this.btn_compact2.Name = "btn_compact2";
            this.btn_compact2.Size = new System.Drawing.Size(100, 25);
            this.btn_compact2.TabIndex = 5;
            this.btn_compact2.Text = "Compact + index";
            this.btn_compact2.UseVisualStyleBackColor = true;
            this.btn_compact2.Click += new System.EventHandler(this.btn_compact2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(276, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Введите название таблицы, которую хотите удалить:";
            // 
            // txtDelTable
            // 
            this.txtDelTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDelTable.Location = new System.Drawing.Point(7, 19);
            this.txtDelTable.Name = "txtDelTable";
            this.txtDelTable.Size = new System.Drawing.Size(276, 20);
            this.txtDelTable.TabIndex = 3;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(423, 240);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 25);
            this.button2.TabIndex = 2;
            this.button2.Text = "Drop table";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tab_delOldData
            // 
            this.tab_delOldData.Controls.Add(this.btn_compact3);
            this.tab_delOldData.Controls.Add(this.lbl_conditionTab3);
            this.tab_delOldData.Controls.Add(this.label5);
            this.tab_delOldData.Controls.Add(this.progressBar3);
            this.tab_delOldData.Controls.Add(this.btn_exportToNewMDB);
            this.tab_delOldData.Controls.Add(this.btn_deleteOldData);
            this.tab_delOldData.Controls.Add(this.label3);
            this.tab_delOldData.Controls.Add(this.label2);
            this.tab_delOldData.Controls.Add(this.pckr_dueDate);
            this.tab_delOldData.Controls.Add(this.lst_allTables);
            this.tab_delOldData.Location = new System.Drawing.Point(4, 22);
            this.tab_delOldData.Name = "tab_delOldData";
            this.tab_delOldData.Padding = new System.Windows.Forms.Padding(3);
            this.tab_delOldData.Size = new System.Drawing.Size(530, 317);
            this.tab_delOldData.TabIndex = 2;
            this.tab_delOldData.Text = "Удалить старые данные";
            this.tab_delOldData.UseVisualStyleBackColor = true;
            // 
            // btn_compact3
            // 
            this.btn_compact3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_compact3.Location = new System.Drawing.Point(3, 240);
            this.btn_compact3.Name = "btn_compact3";
            this.btn_compact3.Size = new System.Drawing.Size(100, 25);
            this.btn_compact3.TabIndex = 9;
            this.btn_compact3.Text = "Compact + index";
            this.btn_compact3.UseVisualStyleBackColor = true;
            this.btn_compact3.Click += new System.EventHandler(this.btn_compact3_Click);
            // 
            // lbl_conditionTab3
            // 
            this.lbl_conditionTab3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbl_conditionTab3.AutoSize = true;
            this.lbl_conditionTab3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_conditionTab3.Location = new System.Drawing.Point(85, 268);
            this.lbl_conditionTab3.Name = "lbl_conditionTab3";
            this.lbl_conditionTab3.Size = new System.Drawing.Size(61, 15);
            this.lbl_conditionTab3.TabIndex = 8;
            this.lbl_conditionTab3.Text = "                  ";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(6, 268);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 15);
            this.label5.TabIndex = 7;
            this.label5.Text = "Состояние:";
            // 
            // progressBar3
            // 
            this.progressBar3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar3.Location = new System.Drawing.Point(6, 286);
            this.progressBar3.Name = "progressBar3";
            this.progressBar3.Size = new System.Drawing.Size(517, 23);
            this.progressBar3.Step = 1;
            this.progressBar3.TabIndex = 6;
            // 
            // btn_exportToNewMDB
            // 
            this.btn_exportToNewMDB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_exportToNewMDB.Location = new System.Drawing.Point(317, 240);
            this.btn_exportToNewMDB.Name = "btn_exportToNewMDB";
            this.btn_exportToNewMDB.Size = new System.Drawing.Size(100, 25);
            this.btn_exportToNewMDB.TabIndex = 5;
            this.btn_exportToNewMDB.Text = "Экспорт данных";
            this.btn_exportToNewMDB.UseVisualStyleBackColor = true;
            this.btn_exportToNewMDB.Click += new System.EventHandler(this.btn_exportToNewMDB_Click);
            // 
            // btn_deleteOldData
            // 
            this.btn_deleteOldData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_deleteOldData.Location = new System.Drawing.Point(423, 240);
            this.btn_deleteOldData.Name = "btn_deleteOldData";
            this.btn_deleteOldData.Size = new System.Drawing.Size(100, 25);
            this.btn_deleteOldData.TabIndex = 4;
            this.btn_deleteOldData.Text = "Удалить данные";
            this.btn_deleteOldData.UseVisualStyleBackColor = true;
            this.btn_deleteOldData.Click += new System.EventHandler(this.btn_deleteOldData_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(321, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Выберите таблицы, в которых нужно удалить старые данные:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 202);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(389, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Выберите, до какой даты следует очистить данные в выбранных таблицах:";
            // 
            // pckr_dueDate
            // 
            this.pckr_dueDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pckr_dueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.pckr_dueDate.Location = new System.Drawing.Point(6, 218);
            this.pckr_dueDate.Name = "pckr_dueDate";
            this.pckr_dueDate.Size = new System.Drawing.Size(200, 20);
            this.pckr_dueDate.TabIndex = 1;
            // 
            // lst_allTables
            // 
            this.lst_allTables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lst_allTables.CheckOnClick = true;
            this.lst_allTables.FormattingEnabled = true;
            this.lst_allTables.Location = new System.Drawing.Point(6, 19);
            this.lst_allTables.Name = "lst_allTables";
            this.lst_allTables.Size = new System.Drawing.Size(517, 169);
            this.lst_allTables.TabIndex = 0;
            // 
            // CompareMDBs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 341);
            this.Controls.Add(this.Compare);
            this.Name = "CompareMDBs";
            this.Text = "Form1";
            this.Compare.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tab_delOldData.ResumeLayout(false);
            this.tab_delOldData.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TabControl Compare;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tab_delOldData;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtDelTable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker pckr_dueDate;
        private System.Windows.Forms.CheckedListBox lst_allTables;
        private System.Windows.Forms.Button btn_deleteOldData;
        private System.Windows.Forms.Button btn_exportToNewMDB;
        private System.Windows.Forms.Button btn_compact;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lbl_conditionTab1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbl_conditionTab3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ProgressBar progressBar3;
        private System.Windows.Forms.Button btn_compact3;
        private System.Windows.Forms.Button btn_compact2;
        private System.Windows.Forms.Label lbl_conditionTab2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.Label label7;
    }
}

