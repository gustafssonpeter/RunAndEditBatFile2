namespace DB_Updater
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxClient = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.textBoxDatabaseP = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxDatabaseH = new System.Windows.Forms.TextBox();
            this.checkBoxRestoreDB = new System.Windows.Forms.CheckBox();
            this.textBoxFrom = new System.Windows.Forms.TextBox();
            this.textBoxTo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxVersion = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbBackupDb = new System.Windows.Forms.RadioButton();
            this.radioButtonRestoreFromOtherFiles = new System.Windows.Forms.RadioButton();
            this.rbUpgradeFromPath = new System.Windows.Forms.RadioButton();
            this.radioButtonRestoreToTRUNK = new System.Windows.Forms.RadioButton();
            this.radioButtonRestoreToBase = new System.Windows.Forms.RadioButton();
            this.radioButtonUpgradeQFdb = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.buttonFolderPath = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.buttonFileProd = new System.Windows.Forms.Button();
            this.textBoxFileProd = new System.Windows.Forms.TextBox();
            this.textBoxFileHist = new System.Windows.Forms.TextBox();
            this.labelFileProd = new System.Windows.Forms.Label();
            this.labelFileHist = new System.Windows.Forms.Label();
            this.buttonFileHist = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBoxQfPath = new System.Windows.Forms.TextBox();
            this.buttonQfPath = new System.Windows.Forms.Button();
            this.labelQfPath = new System.Windows.Forms.Label();
            this.checkBoxCopyFiles = new System.Windows.Forms.CheckBox();
            this.textBoxBackupClient = new System.Windows.Forms.TextBox();
            this.textBoxBackupDb = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxBackupPath = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.buttonBackupPath = new System.Windows.Forms.Button();
            this.textBoxBackupFile = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.checkBoxDeleteFolders = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(241, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Client";
            // 
            // textBoxClient
            // 
            this.textBoxClient.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxClient.Enabled = false;
            this.textBoxClient.Location = new System.Drawing.Point(244, 120);
            this.textBoxClient.Name = "textBoxClient";
            this.textBoxClient.Size = new System.Drawing.Size(133, 20);
            this.textBoxClient.TabIndex = 7;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(443, 345);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(182, 35);
            this.button2.TabIndex = 17;
            this.button2.Text = "Clear Form";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBoxDatabaseP
            // 
            this.textBoxDatabaseP.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxDatabaseP.Enabled = false;
            this.textBoxDatabaseP.Location = new System.Drawing.Point(244, 171);
            this.textBoxDatabaseP.Name = "textBoxDatabaseP";
            this.textBoxDatabaseP.Size = new System.Drawing.Size(133, 20);
            this.textBoxDatabaseP.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(241, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Database Production";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(241, 208);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Database Histotic";
            // 
            // textBoxDatabaseH
            // 
            this.textBoxDatabaseH.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxDatabaseH.Enabled = false;
            this.textBoxDatabaseH.Location = new System.Drawing.Point(244, 224);
            this.textBoxDatabaseH.Name = "textBoxDatabaseH";
            this.textBoxDatabaseH.Size = new System.Drawing.Size(133, 20);
            this.textBoxDatabaseH.TabIndex = 9;
            // 
            // checkBoxRestoreDB
            // 
            this.checkBoxRestoreDB.AutoSize = true;
            this.checkBoxRestoreDB.Enabled = false;
            this.checkBoxRestoreDB.Location = new System.Drawing.Point(34, 59);
            this.checkBoxRestoreDB.Name = "checkBoxRestoreDB";
            this.checkBoxRestoreDB.Size = new System.Drawing.Size(154, 17);
            this.checkBoxRestoreDB.TabIndex = 1;
            this.checkBoxRestoreDB.Text = "Restore db to Base version\r\n";
            this.checkBoxRestoreDB.UseVisualStyleBackColor = true;
            this.checkBoxRestoreDB.CheckedChanged += new System.EventHandler(this.checkBoxRestoreDB_CheckedChanged);
            // 
            // textBoxFrom
            // 
            this.textBoxFrom.Enabled = false;
            this.textBoxFrom.Location = new System.Drawing.Point(34, 120);
            this.textBoxFrom.Name = "textBoxFrom";
            this.textBoxFrom.Size = new System.Drawing.Size(78, 20);
            this.textBoxFrom.TabIndex = 2;
            this.textBoxFrom.TextChanged += new System.EventHandler(this.textBoxFrom_TextChanged);
            // 
            // textBoxTo
            // 
            this.textBoxTo.Enabled = false;
            this.textBoxTo.Location = new System.Drawing.Point(34, 171);
            this.textBoxTo.Name = "textBoxTo";
            this.textBoxTo.Size = new System.Drawing.Size(78, 20);
            this.textBoxTo.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 104);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "QF From (0 = from base)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 155);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "QF To";
            // 
            // textBoxVersion
            // 
            this.textBoxVersion.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxVersion.Enabled = false;
            this.textBoxVersion.Location = new System.Drawing.Point(244, 69);
            this.textBoxVersion.Name = "textBoxVersion";
            this.textBoxVersion.Size = new System.Drawing.Size(133, 20);
            this.textBoxVersion.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(241, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(119, 26);
            this.label6.TabIndex = 16;
            this.label6.Text = "Version to restore to\r\n(Base version e.g. 5.10)";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbBackupDb);
            this.groupBox1.Controls.Add(this.radioButtonRestoreFromOtherFiles);
            this.groupBox1.Controls.Add(this.rbUpgradeFromPath);
            this.groupBox1.Controls.Add(this.radioButtonRestoreToTRUNK);
            this.groupBox1.Controls.Add(this.radioButtonRestoreToBase);
            this.groupBox1.Controls.Add(this.radioButtonUpgradeQFdb);
            this.groupBox1.Location = new System.Drawing.Point(406, 40);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(219, 178);
            this.groupBox1.TabIndex = 60;
            this.groupBox1.TabStop = false;
            // 
            // rbBackupDb
            // 
            this.rbBackupDb.AutoSize = true;
            this.rbBackupDb.Location = new System.Drawing.Point(7, 144);
            this.rbBackupDb.Name = "rbBackupDb";
            this.rbBackupDb.Size = new System.Drawing.Size(109, 17);
            this.rbBackupDb.TabIndex = 27;
            this.rbBackupDb.TabStop = true;
            this.rbBackupDb.Text = "Backup database";
            this.rbBackupDb.UseVisualStyleBackColor = true;
            this.rbBackupDb.CheckedChanged += new System.EventHandler(this.rbBackupDb_CheckedChanged);
            // 
            // radioButtonRestoreFromOtherFiles
            // 
            this.radioButtonRestoreFromOtherFiles.AutoSize = true;
            this.radioButtonRestoreFromOtherFiles.Location = new System.Drawing.Point(7, 95);
            this.radioButtonRestoreFromOtherFiles.Name = "radioButtonRestoreFromOtherFiles";
            this.radioButtonRestoreFromOtherFiles.Size = new System.Drawing.Size(133, 17);
            this.radioButtonRestoreFromOtherFiles.TabIndex = 4;
            this.radioButtonRestoreFromOtherFiles.TabStop = true;
            this.radioButtonRestoreFromOtherFiles.Text = "Restore from other files";
            this.radioButtonRestoreFromOtherFiles.UseVisualStyleBackColor = true;
            this.radioButtonRestoreFromOtherFiles.CheckedChanged += new System.EventHandler(this.radioButtonRestoreOtherFiles_CheckedChanged);
            // 
            // rbUpgradeFromPath
            // 
            this.rbUpgradeFromPath.AutoSize = true;
            this.rbUpgradeFromPath.Location = new System.Drawing.Point(7, 120);
            this.rbUpgradeFromPath.Name = "rbUpgradeFromPath";
            this.rbUpgradeFromPath.Size = new System.Drawing.Size(113, 17);
            this.rbUpgradeFromPath.TabIndex = 3;
            this.rbUpgradeFromPath.TabStop = true;
            this.rbUpgradeFromPath.Text = "Upgrade from path";
            this.rbUpgradeFromPath.UseVisualStyleBackColor = true;
            this.rbUpgradeFromPath.CheckedChanged += new System.EventHandler(this.rbUpgradeFromPath_CheckedChanged);
            // 
            // radioButtonRestoreToTRUNK
            // 
            this.radioButtonRestoreToTRUNK.AutoSize = true;
            this.radioButtonRestoreToTRUNK.Location = new System.Drawing.Point(7, 70);
            this.radioButtonRestoreToTRUNK.Name = "radioButtonRestoreToTRUNK";
            this.radioButtonRestoreToTRUNK.Size = new System.Drawing.Size(178, 17);
            this.radioButtonRestoreToTRUNK.TabIndex = 26;
            this.radioButtonRestoreToTRUNK.TabStop = true;
            this.radioButtonRestoreToTRUNK.Text = "Restore and upgrade to TRUNK";
            this.radioButtonRestoreToTRUNK.UseVisualStyleBackColor = true;
            this.radioButtonRestoreToTRUNK.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // radioButtonRestoreToBase
            // 
            this.radioButtonRestoreToBase.AutoSize = true;
            this.radioButtonRestoreToBase.Location = new System.Drawing.Point(7, 45);
            this.radioButtonRestoreToBase.Name = "radioButtonRestoreToBase";
            this.radioButtonRestoreToBase.Size = new System.Drawing.Size(101, 17);
            this.radioButtonRestoreToBase.TabIndex = 1;
            this.radioButtonRestoreToBase.TabStop = true;
            this.radioButtonRestoreToBase.Text = "Restore to Base";
            this.radioButtonRestoreToBase.UseVisualStyleBackColor = true;
            this.radioButtonRestoreToBase.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButtonUpgradeQFdb
            // 
            this.radioButtonUpgradeQFdb.AutoSize = true;
            this.radioButtonUpgradeQFdb.Location = new System.Drawing.Point(7, 20);
            this.radioButtonUpgradeQFdb.Name = "radioButtonUpgradeQFdb";
            this.radioButtonUpgradeQFdb.Size = new System.Drawing.Size(142, 17);
            this.radioButtonUpgradeQFdb.TabIndex = 0;
            this.radioButtonUpgradeQFdb.TabStop = true;
            this.radioButtonUpgradeQFdb.Text = "Upgrade database to QF";
            this.radioButtonUpgradeQFdb.UseVisualStyleBackColor = true;
            this.radioButtonUpgradeQFdb.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(443, 292);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(182, 35);
            this.button1.TabIndex = 16;
            this.button1.Text = "Start Restore/Upgrade";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxPath
            // 
            this.textBoxPath.Location = new System.Drawing.Point(245, 401);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(165, 20);
            this.textBoxPath.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(242, 385);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(170, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Path to folder with db upgrade files";
            // 
            // buttonFolderPath
            // 
            this.buttonFolderPath.Location = new System.Drawing.Point(164, 398);
            this.buttonFolderPath.Name = "buttonFolderPath";
            this.buttonFolderPath.Size = new System.Drawing.Size(75, 23);
            this.buttonFolderPath.TabIndex = 14;
            this.buttonFolderPath.Text = "Browse";
            this.buttonFolderPath.UseVisualStyleBackColor = true;
            this.buttonFolderPath.Click += new System.EventHandler(this.buttonFolderPath_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "*.txt";
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "SQL bakckup files (*.bak)|*.bak|All files (*.*)|*.*";
            // 
            // buttonFileProd
            // 
            this.buttonFileProd.Location = new System.Drawing.Point(164, 293);
            this.buttonFileProd.Name = "buttonFileProd";
            this.buttonFileProd.Size = new System.Drawing.Size(75, 23);
            this.buttonFileProd.TabIndex = 10;
            this.buttonFileProd.Text = "Browse";
            this.buttonFileProd.UseVisualStyleBackColor = true;
            this.buttonFileProd.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBoxFileProd
            // 
            this.textBoxFileProd.Location = new System.Drawing.Point(245, 295);
            this.textBoxFileProd.Name = "textBoxFileProd";
            this.textBoxFileProd.Size = new System.Drawing.Size(165, 20);
            this.textBoxFileProd.TabIndex = 11;
            // 
            // textBoxFileHist
            // 
            this.textBoxFileHist.Location = new System.Drawing.Point(245, 348);
            this.textBoxFileHist.Name = "textBoxFileHist";
            this.textBoxFileHist.Size = new System.Drawing.Size(165, 20);
            this.textBoxFileHist.TabIndex = 13;
            // 
            // labelFileProd
            // 
            this.labelFileProd.AutoSize = true;
            this.labelFileProd.Location = new System.Drawing.Point(242, 279);
            this.labelFileProd.Name = "labelFileProd";
            this.labelFileProd.Size = new System.Drawing.Size(139, 13);
            this.labelFileProd.TabIndex = 23;
            this.labelFileProd.Text = "File to restore Production db";
            // 
            // labelFileHist
            // 
            this.labelFileHist.AutoSize = true;
            this.labelFileHist.Location = new System.Drawing.Point(242, 332);
            this.labelFileHist.Name = "labelFileHist";
            this.labelFileHist.Size = new System.Drawing.Size(123, 13);
            this.labelFileHist.TabIndex = 24;
            this.labelFileHist.Text = "File to restore Historic db";
            // 
            // buttonFileHist
            // 
            this.buttonFileHist.Location = new System.Drawing.Point(164, 346);
            this.buttonFileHist.Name = "buttonFileHist";
            this.buttonFileHist.Size = new System.Drawing.Size(75, 23);
            this.buttonFileHist.TabIndex = 12;
            this.buttonFileHist.Text = "Browse";
            this.buttonFileHist.UseVisualStyleBackColor = true;
            this.buttonFileHist.Click += new System.EventHandler(this.buttonFileHist_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(864, 24);
            this.menuStrip1.TabIndex = 26;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(125, 22);
            this.toolStripMenuItem1.Text = "View help";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // textBoxQfPath
            // 
            this.textBoxQfPath.Location = new System.Drawing.Point(34, 224);
            this.textBoxQfPath.Name = "textBoxQfPath";
            this.textBoxQfPath.Size = new System.Drawing.Size(165, 20);
            this.textBoxQfPath.TabIndex = 4;
            // 
            // buttonQfPath
            // 
            this.buttonQfPath.Location = new System.Drawing.Point(34, 251);
            this.buttonQfPath.Name = "buttonQfPath";
            this.buttonQfPath.Size = new System.Drawing.Size(75, 23);
            this.buttonQfPath.TabIndex = 5;
            this.buttonQfPath.Text = "Browse";
            this.buttonQfPath.UseVisualStyleBackColor = true;
            this.buttonQfPath.Click += new System.EventHandler(this.buttonQfPath_Click);
            // 
            // labelQfPath
            // 
            this.labelQfPath.AutoSize = true;
            this.labelQfPath.Location = new System.Drawing.Point(34, 207);
            this.labelQfPath.Name = "labelQfPath";
            this.labelQfPath.Size = new System.Drawing.Size(87, 13);
            this.labelQfPath.TabIndex = 29;
            this.labelQfPath.Text = "Path to QF folder";
            // 
            // checkBoxCopyFiles
            // 
            this.checkBoxCopyFiles.AutoSize = true;
            this.checkBoxCopyFiles.Location = new System.Drawing.Point(406, 224);
            this.checkBoxCopyFiles.Name = "checkBoxCopyFiles";
            this.checkBoxCopyFiles.Size = new System.Drawing.Size(169, 17);
            this.checkBoxCopyFiles.TabIndex = 30;
            this.checkBoxCopyFiles.Text = "Copy Base restore files from Z:";
            this.checkBoxCopyFiles.UseVisualStyleBackColor = true;
            // 
            // textBoxBackupClient
            // 
            this.textBoxBackupClient.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxBackupClient.Location = new System.Drawing.Point(682, 69);
            this.textBoxBackupClient.Name = "textBoxBackupClient";
            this.textBoxBackupClient.Size = new System.Drawing.Size(131, 20);
            this.textBoxBackupClient.TabIndex = 20;
            // 
            // textBoxBackupDb
            // 
            this.textBoxBackupDb.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxBackupDb.Location = new System.Drawing.Point(682, 120);
            this.textBoxBackupDb.Name = "textBoxBackupDb";
            this.textBoxBackupDb.Size = new System.Drawing.Size(131, 20);
            this.textBoxBackupDb.TabIndex = 21;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(679, 53);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(33, 13);
            this.label8.TabIndex = 34;
            this.label8.Text = "Client";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(679, 104);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(104, 13);
            this.label9.TabIndex = 35;
            this.label9.Text = "Database to backup";
            // 
            // textBoxBackupPath
            // 
            this.textBoxBackupPath.Location = new System.Drawing.Point(682, 224);
            this.textBoxBackupPath.Name = "textBoxBackupPath";
            this.textBoxBackupPath.Size = new System.Drawing.Size(165, 20);
            this.textBoxBackupPath.TabIndex = 23;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(679, 207);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(99, 13);
            this.label10.TabIndex = 37;
            this.label10.Text = "Path for backup file";
            // 
            // buttonBackupPath
            // 
            this.buttonBackupPath.Location = new System.Drawing.Point(682, 251);
            this.buttonBackupPath.Name = "buttonBackupPath";
            this.buttonBackupPath.Size = new System.Drawing.Size(75, 23);
            this.buttonBackupPath.TabIndex = 24;
            this.buttonBackupPath.Text = "Browse";
            this.buttonBackupPath.UseVisualStyleBackColor = true;
            this.buttonBackupPath.Click += new System.EventHandler(this.buttonBackupPath_Click);
            // 
            // textBoxBackupFile
            // 
            this.textBoxBackupFile.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxBackupFile.Location = new System.Drawing.Point(682, 171);
            this.textBoxBackupFile.Name = "textBoxBackupFile";
            this.textBoxBackupFile.Size = new System.Drawing.Size(131, 20);
            this.textBoxBackupFile.TabIndex = 22;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(679, 155);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(105, 13);
            this.label11.TabIndex = 40;
            this.label11.Text = "Name for backup file";
            // 
            // buttonSave
            // 
            this.buttonSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSave.Location = new System.Drawing.Point(443, 398);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(82, 23);
            this.buttonSave.TabIndex = 18;
            this.buttonSave.Text = "Save settings";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(545, 398);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(82, 23);
            this.buttonLoad.TabIndex = 19;
            this.buttonLoad.Text = "Load settings";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // checkBoxDeleteFolders
            // 
            this.checkBoxDeleteFolders.AutoSize = true;
            this.checkBoxDeleteFolders.Location = new System.Drawing.Point(406, 248);
            this.checkBoxDeleteFolders.Name = "checkBoxDeleteFolders";
            this.checkBoxDeleteFolders.Size = new System.Drawing.Size(196, 17);
            this.checkBoxDeleteFolders.TabIndex = 41;
            this.checkBoxDeleteFolders.Text = "Delete old TRUNK database folders";
            this.checkBoxDeleteFolders.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 456);
            this.Controls.Add(this.checkBoxDeleteFolders);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textBoxBackupFile);
            this.Controls.Add(this.buttonBackupPath);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBoxBackupPath);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBoxBackupDb);
            this.Controls.Add(this.textBoxBackupClient);
            this.Controls.Add(this.checkBoxCopyFiles);
            this.Controls.Add(this.labelQfPath);
            this.Controls.Add(this.buttonQfPath);
            this.Controls.Add(this.textBoxQfPath);
            this.Controls.Add(this.buttonFileHist);
            this.Controls.Add(this.labelFileHist);
            this.Controls.Add(this.labelFileProd);
            this.Controls.Add(this.textBoxFileHist);
            this.Controls.Add(this.textBoxFileProd);
            this.Controls.Add(this.buttonFileProd);
            this.Controls.Add(this.buttonFolderPath);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxPath);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxVersion);
            this.Controls.Add(this.checkBoxRestoreDB);
            this.Controls.Add(this.textBoxDatabaseH);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxDatabaseP);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBoxClient);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxTo);
            this.Controls.Add(this.textBoxFrom);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximumSize = new System.Drawing.Size(880, 495);
            this.MinimumSize = new System.Drawing.Size(880, 495);
            this.Name = "Form1";
            this.Text = "DB Updater";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxClient;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBoxDatabaseP;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxDatabaseH;
        private System.Windows.Forms.CheckBox checkBoxRestoreDB;
        private System.Windows.Forms.TextBox textBoxFrom;
        private System.Windows.Forms.TextBox textBoxTo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxVersion;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonRestoreToTRUNK;
        private System.Windows.Forms.RadioButton radioButtonRestoreToBase;
        private System.Windows.Forms.RadioButton radioButtonUpgradeQFdb;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton rbUpgradeFromPath;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button buttonFolderPath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button buttonFileProd;
        private System.Windows.Forms.TextBox textBoxFileProd;
        private System.Windows.Forms.TextBox textBoxFileHist;
        private System.Windows.Forms.Label labelFileProd;
        private System.Windows.Forms.Label labelFileHist;
        private System.Windows.Forms.Button buttonFileHist;
        private System.Windows.Forms.RadioButton radioButtonRestoreFromOtherFiles;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.TextBox textBoxQfPath;
        private System.Windows.Forms.Button buttonQfPath;
        private System.Windows.Forms.Label labelQfPath;
        private System.Windows.Forms.CheckBox checkBoxCopyFiles;
        private System.Windows.Forms.TextBox textBoxBackupClient;
        private System.Windows.Forms.TextBox textBoxBackupDb;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxBackupPath;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button buttonBackupPath;
        private System.Windows.Forms.TextBox textBoxBackupFile;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.RadioButton rbBackupDb;
        private System.Windows.Forms.CheckBox checkBoxDeleteFolders;
    }
}

