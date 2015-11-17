using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace DB_Updater
{
    public partial class Form1 : Form
    {
        string replaceFileLatestVersion, strFileProd, strFileHist, strSearch, strSearchResult,
            restoreToBaseBat, setupLocalTrunkBat, restoreAndToQF1bat, strDbFileName;
        string myHostName = System.Net.Dns.GetHostName();
        int count, from, to, outputValue, rbState;
        bool isFirstRun, isNumberFrom, isNumberTo, isRestoreFromBase, isNotLocalServer, isRestored;

        string help =
@"To use this program you need to create the directory C:\Databaser on your computer. This directory need to be shared to the network.

You also need to put the unziped Database QF files in the direcory ""Path to folder of QF upgrade files""";

        string copyFiles =
@"
xcopy \\profdoc.lab\dfs01\Gemensam\Test\Verktyg\unzip.exe c:\Databaser\unzip.exe /D /Y
xcopy \\profdoc.lab\dfs01\Databaser\Test\Orginal\%VERSION%\P%FILE_VERSION%TCO_LATEST.bak c:\Databaser\P%FILE_VERSION%TCO_LATEST.bak /D /Y
xcopy \\profdoc.lab\dfs01\Databaser\Test\Orginal\%VERSION%\H%FILE_VERSION%TCO_LATEST.bak c:\Databaser\H%FILE_VERSION%TCO_LATEST.bak /D /Y";

        string upgradeToLatestTrunk =
@"
for /f ""delims="" %%i in (\\profdoc.lab\dfs01\System\Autobuild\dblatest.txt) do xcopy ""\\profdoc.lab\dfs01\System\Autobuild\%%i.exe"" ""c:\databaser"" /D /Y
for /f ""delims="" %%i in (\\profdoc.lab\dfs01\System\Autobuild\dblatest.txt) do unzip ""c:\databaser\%%i.exe""
for /f ""delims="" %%i in (\\profdoc.lab\dfs01\System\Autobuild\dblatest.txt) do cd ""c:\databaser\%%i""
START /B ""Upgrade historic"" ""Upgrade Historic.bat"" SYSADM SYSADM %DATABASE_H% %CLIENT_UPGRADE%
START ""Upgrade production"" ""Upgrade Production.bat"" SYSADM SYSADM %DATABASE_P% %CLIENT_UPGRADE%";

        string runRestoreScript =
@"
cd ""c:\databaser""
sqlcmd -S %CLIENT% -d %DATABASE_P% -U SYSADM -P SYSADM -i DBupdate_restoreSQL.sql -o ""c:\databaser\DBupdate_restoreSQL.txt""";

        string upgradeFromQfToQf =
@"
cd ""%QF_PATH%""
START /B ""Upgrade historic"" ""Upgrade Historic From %VERSION% QF%FROM% To %VERSION% QF%TO%.bat"" SYSADM SYSADM %DATABASE_H% %CLIENT_UPGRADE%
START /B ""Upgrade production"" ""Upgrade Production From %VERSION% QF%FROM% To %VERSION% QF%TO%.bat"" SYSADM SYSADM %DATABASE_P% %CLIENT_UPGRADE% 
pause";

        string upgradeToQF1 =
@"
cd ""%QF_PATH%""
START /B ""Upgrade production"" ""Upgrade Production From %VERSION% To %VERSION% QF1.bat"" SYSADM SYSADM %DATABASE_P% %CLIENT_UPGRADE% 
START /B ""Upgrade historic"" ""Upgrade Historic From %VERSION% To %VERSION% QF1.bat"" SYSADM SYSADM %DATABASE_H% %CLIENT_UPGRADE% 
pause";

        string upgradeFromPath =
@"
cd ""%PATH%""
START /B ""Upgrade historic"" ""Upgrade Historic.bat"" SYSADM SYSADM %DATABASE_H% %CLIENT_UPGRADE% 
START ""Upgrade production"" ""Upgrade Production.bat"" SYSADM SYSADM %DATABASE_P% %CLIENT_UPGRADE% ";

        string sqlRestoreScript =
@"
use master
RESTORE DATABASE %DATABASE_P% FROM  DISK = N'%RESTORE_FILE_PROD%' WITH  FILE = 1,  NOUNLOAD,  REPLACE,  STATS = 10
GO
RESTORE DATABASE %DATABASE_H%  FROM  DISK = N'%RESTORE_FILE_HIST%' WITH  FILE = 1,  NOUNLOAD,  REPLACE,  STATS = 10
GO
use %DATABASE_P%
update databaser set dat_databas = '%DATABASE_P%', dat_orgdatabas='%DATABASE_P%', dat_servernamn='%CLIENT%' where dat_typ ='P'
update databaser set dat_databas = '%DATABASE_H%', dat_orgdatabas='%DATABASE_H%', dat_servernamn='%CLIENT%' where dat_typ ='H'
Exec GrantAnalytixPermissions
use %DATABASE_H%
Exec a_ResetHistProdUser";

        string sqlRestoreScriptProd =
@"
use master
RESTORE DATABASE %DATABASE_P% FROM  DISK = N'%RESTORE_FILE_PROD%' WITH  FILE = 1,  NOUNLOAD,  REPLACE,  STATS = 10
GO
use %DATABASE_P%
update databaser set dat_databas = '%DATABASE_P%', dat_orgdatabas='%DATABASE_P%', dat_servernamn='%CLIENT%' where dat_typ ='P'
Exec GrantAnalytixPermissions";

        string sqlBackupDatabase =
@"BACKUP DATABASE [%DATABASE%] TO  DISK = N'%FOLDER_PATH%\%FILENAME%.bak' WITH NOFORMAT, INIT,  NAME = N'Backup db', SKIP, NOREWIND, NOUNLOAD,  STATS = 10
GO";

        string runBackupScript =
@"
cd ""c:\databaser""
sqlcmd -S %CLIENT% -d %DATABASE% -U SYSADM -P SYSADM -i DbBackup.sql -o ""c:\databaser\DbBackupSQL.txt""";


        public Form1()
        {
            InitializeComponent();
            loadSettings();
        }

        //Start button
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBoxClient.Text.IndexOf(@"\") != -1)
                isNotLocalServer = true;
            else
                isNotLocalServer = false;

            if (radioButtonUpgradeQFdb.Checked)
                startQfUpgrade();

            if (radioButtonRestoreToBase.Checked)
                restoreToBaseVersion();

            if (radioButtonRestoreToTRUNK.Checked)
                restoreToTrunk();

            if (radioButtonRestoreFromOtherFiles.Checked)
                restoreToOther();

            if (rbUpgradeFromPath.Checked)
                upgradeFromPathFiles();

            if (rbBackupDb.Checked)
                runDbBackup();

            if (isRestored)
            {
                MessageBox.Show(File.ReadAllText("C:\\databaser\\DBupdate_restoreSQL.txt"), "Restore result!");
                isRestored = false;
            }
        }

        //Clear button
        private void button2_Click(object sender, EventArgs e)
        {
            radioButtonRestoreToTRUNK.Checked = true;
            textBoxVersion.Clear();
            textBoxClient.Clear();
            textBoxDatabaseP.Clear();
            textBoxDatabaseH.Clear();
            textBoxPath.Clear();
            textBoxFileProd.Clear();
            textBoxFileHist.Clear();
            textBoxQfPath.Clear();
            textBoxBackupClient.Clear();
            textBoxBackupDb.Clear();
            textBoxBackupFile.Clear();
            textBoxBackupPath.Clear();
            textBoxFrom.Clear();
            textBoxTo.Clear();
            checkBoxNotCopyFiles.Checked = false;
            checkBoxRestoreDB.Checked = false;
            checkBoxDeleteFolders.Checked = false;
        }

        private void startQfUpgrade()
        {
            if (!String.IsNullOrEmpty(textBoxFrom.Text) && !String.IsNullOrEmpty(textBoxTo.Text) && !String.IsNullOrEmpty(textBoxVersion.Text)
                && !String.IsNullOrEmpty(textBoxClient.Text) && !String.IsNullOrEmpty(textBoxDatabaseP.Text) && !String.IsNullOrEmpty(textBoxDatabaseH.Text) && !String.IsNullOrEmpty(textBoxQfPath.Text))
            {
                isNumberFrom = int.TryParse(textBoxFrom.Text, out outputValue);
                isNumberTo = int.TryParse(textBoxTo.Text, out outputValue);

                if (!isNumberFrom || !isNumberTo)
                {
                    MessageBox.Show("Only numbers in QF From and QF To!");
                }
                else
                {
                    if (Directory.Exists(textBoxQfPath.Text))
                    {
                        count = int.Parse(textBoxTo.Text) - int.Parse(textBoxFrom.Text);
                        from = int.Parse(textBoxFrom.Text);
                        isFirstRun = true;

                        while (count > 0)
                        {
                            //if-sats för första körningen om restore av Db
                            if (textBoxFrom.Text == "0" && isFirstRun)
                            {
                                restoreAndToQF1bat = "";
                                isRestoreFromBase = true;
                                if (checkBoxRestoreDB.Checked == true)
                                {
                                    createFile("C:\\Databaser\\DBupdate_restoreSQL.sql", sqlRestoreScript);
                                    if (checkBoxNotCopyFiles.Checked == true)
                                        restoreAndToQF1bat = runRestoreScript + upgradeToQF1;
                                    else
                                        restoreAndToQF1bat = copyFiles + runRestoreScript + upgradeToQF1;
                                }
                                else
                                    restoreAndToQF1bat = runRestoreScript + upgradeToQF1;
                                to = 1;
                                createFile("C:\\Databaser\\DBupdate_RestoreQF.bat", restoreAndToQF1bat);
                                isRestored = true;
                                startFile("C:\\Databaser\\DBupdate_RestoreQF.bat");
                            }

                            //if-sats för resterande, eller första med bara upgrade av Db
                            if (!isRestoreFromBase)
                            {
                                if (textBoxFrom.Text == "0")
                                    from++;
                                to = from + 1;

                                createFile("C:\\Databaser\\DBupdate_QfToQf.bat", upgradeFromQfToQf);
                                startFile("C:\\Databaser\\DBupdate_QfToQf.bat");

                                if (textBoxFrom.Text != "0")
                                    from++;
                            }
                            count--;
                            isFirstRun = false;
                            isRestoreFromBase = false;
                        }
                    }
                    else
                        MessageBox.Show("The path " + textBoxQfPath.Text + " don't exists");
                }
            }
            else
                MessageBox.Show("Please enter:\rQF From, QF To, QF Path, Version, Client, Database Prod. and Database Hist.");
        }

        private void runDbBackup()
        {
            if (textBoxBackupClient.Text.IndexOf(@"\") != -1)
                isNotLocalServer = true;
            else
                isNotLocalServer = false;

            if (!String.IsNullOrEmpty(textBoxBackupClient.Text) && !String.IsNullOrEmpty(textBoxBackupDb.Text) && !String.IsNullOrEmpty(textBoxBackupPath.Text))
            {
                if (Directory.Exists(textBoxBackupPath.Text))
                {
                    createBackupSqlFile("C:\\Databaser\\DbBackup.sql", sqlBackupDatabase);
                    createBackupBatFile("C:\\Databaser\\DbBackup.bat", runBackupScript);

                    startFile("C:\\Databaser\\DbBackup.bat");
                }
                else
                    MessageBox.Show("The directory " + textBoxBackupPath.Text + " don't exists");
            }
            else
                MessageBox.Show("Please enter:\rClient, Database to backup, Name of backup file and Path for backup file");

            if (File.Exists(@"C:\databaser\DbBackupSQL.txt"))
                MessageBox.Show(File.ReadAllText("C:\\databaser\\DbBackupSQL.txt"), "Backup result!");
        }

        private void restoreToBaseVersion()
        {
            if (!String.IsNullOrEmpty(textBoxVersion.Text) && !String.IsNullOrEmpty(textBoxClient.Text) && !String.IsNullOrEmpty(textBoxDatabaseP.Text) && !String.IsNullOrEmpty(textBoxDatabaseH.Text))
            {
                restoreToBaseBat = "";
                if (checkBoxNotCopyFiles.Checked == true)
                    restoreToBaseBat = runRestoreScript;
                else
                    restoreToBaseBat = copyFiles + runRestoreScript;
                createFile("C:\\Databaser\\DBupdate_restoreSQL.sql", sqlRestoreScript);
                createFile("C:\\Databaser\\DBupdate_RestoreToBase.bat", restoreToBaseBat);
                isRestored = true;
                startFile("C:\\Databaser\\DBupdate_RestoreToBase.bat");
            }
            else
                MessageBox.Show("Please enter:\rVersion, Client, Database Prod. and Database Hist.");
        }

        private void restoreToTrunk()
        {
            if (!String.IsNullOrEmpty(textBoxVersion.Text) && !String.IsNullOrEmpty(textBoxClient.Text) && !String.IsNullOrEmpty(textBoxDatabaseP.Text) && !String.IsNullOrEmpty(textBoxDatabaseH.Text))
            {
                //Delete old db folders
                if (checkBoxDeleteFolders.Checked == true)
                    deleteFolderPath(@"c:\databaser", @"CGM ANALYTIX database*");
                if (File.Exists(@"\\profdoc.lab\dfs01\System\Autobuild\dblatest.txt"))
                    strDbFileName = File.ReadAllText(@"\\profdoc.lab\dfs01\System\Autobuild\dblatest.txt");

                setupLocalTrunkBat = "";
                if (checkBoxNotCopyFiles.Checked == true)
                    setupLocalTrunkBat = runRestoreScript + upgradeToLatestTrunk;
                else
                    setupLocalTrunkBat = copyFiles + runRestoreScript + upgradeToLatestTrunk;
                createFile("C:\\Databaser\\DBupdate_restoreSQL.sql", sqlRestoreScript);
                createFile("C:\\Databaser\\DBupdate_SetupLocalTrunk.bat", setupLocalTrunkBat);
                isRestored = true;
                startFile("C:\\Databaser\\DBupdate_SetupLocalTrunk.bat");

                //Delete the zip file
                string filepath = @"c:\databaser\" + strDbFileName + ".exe";
                if (File.Exists(filepath) && FileInUse(filepath) == false)
                    File.Delete(@"c:\databaser\" + strDbFileName + ".exe");
            }
            else
                MessageBox.Show("Please enter:\rVersion, Client, Database Prod. and Database Hist.");
        }

        private void restoreToOther()
        {
            if (!String.IsNullOrEmpty(textBoxClient.Text) && (!String.IsNullOrEmpty(textBoxDatabaseP.Text) || !String.IsNullOrEmpty(textBoxDatabaseH.Text)) && (!String.IsNullOrEmpty(textBoxFileProd.Text) || !String.IsNullOrEmpty(textBoxFileHist.Text)))
            {
                if (File.Exists(textBoxFileHist.Text) || File.Exists(textBoxFileProd.Text))
                {
                    if (String.IsNullOrEmpty(textBoxDatabaseH.Text))
                        createFile("C:\\Databaser\\DBupdate_restoreSQL.sql", sqlRestoreScriptProd);
                    else
                        createFile("C:\\Databaser\\DBupdate_restoreSQL.sql", sqlRestoreScript);
                    createFile("C:\\Databaser\\DBupdate_RestoreToOtherFiles.bat", runRestoreScript);
                    isRestored = true;
                    startFile("C:\\Databaser\\DBupdate_RestoreToOtherFiles.bat");
                }
                else
                    MessageBox.Show("One (or both) of the backup files to restore from doesn't exists");
            }
            else
                MessageBox.Show("Please enter:\rClient, Database Prod, Database Hist\nFile to restore Prod and File to restore Hist");
        }

        private void upgradeFromPathFiles()
        {
            if (!String.IsNullOrEmpty(textBoxVersion.Text) && !String.IsNullOrEmpty(textBoxClient.Text) && !String.IsNullOrEmpty(textBoxDatabaseP.Text) && !String.IsNullOrEmpty(textBoxDatabaseH.Text) && !String.IsNullOrEmpty(textBoxPath.Text))
            {
                if (Directory.Exists(textBoxPath.Text))
                {
                    createFile("C:\\Databaser\\DBupdate_upgradeFromPath.bat", upgradeFromPath);
                    startFile("C:\\Databaser\\DBupdate_upgradeFromPath.bat");
                }
                else
                    MessageBox.Show("The directory " + textBoxPath.Text + " don't exists");
            }
            else
                MessageBox.Show("Please enter:\rClient, Database Prod, Database Hist and Path");
        }

        private void buttonBackupPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBoxBackupPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void createFile(string filePath, string content)
        {
            if (textBoxVersion.Text.Contains("."))
                replaceFileLatestVersion = textBoxVersion.Text.Replace(".", "") + "0";
            if (textBoxVersion.Text.Contains(","))
                replaceFileLatestVersion = textBoxVersion.Text.Replace(",", "") + "0";

            if (radioButtonUpgradeQFdb.Checked == true)
            {
                strSearch = @"*database*" + textBoxVersion.Text.Replace(",", ".") + @"*" + "qf" + @"*" + to.ToString() + @"*";
                strSearchResult = getSubFolderPath(textBoxQfPath.Text, strSearch);
                content = content.Replace("%QF_PATH%", strSearchResult);
                content = content.Replace("%TO%", to.ToString());
                content = content.Replace("%FROM%", from.ToString());
            }

            content = content.Replace("%DATABASE_P%", textBoxDatabaseP.Text);
            content = content.Replace("%DATABASE_H%", textBoxDatabaseH.Text);
            content = content.Replace("%VERSION%", textBoxVersion.Text.Replace(",", "."));
            if (checkBoxNotCopyFiles.Checked == false)
                content = content.Replace("%FILE_VERSION%", replaceFileLatestVersion);
            if (rbUpgradeFromPath.Checked == true)
                content = content.Replace("%PATH%", textBoxPath.Text);

            if (radioButtonRestoreFromOtherFiles.Checked)
            {
                if (isNotLocalServer)
                {
                    content = content.Replace("%RESTORE_FILE_PROD%", @"\\" + myHostName + textBoxFileProd.Text.Remove(0, 2));
                    content = content.Replace("%RESTORE_FILE_HIST%", @"\\" + myHostName + textBoxFileHist.Text.Remove(0, 2));
                }
                else
                {
                    content = content.Replace("%RESTORE_FILE_PROD%", textBoxFileProd.Text);
                    content = content.Replace("%RESTORE_FILE_HIST%", textBoxFileHist.Text);
                }

            }
            else
            {
                if (isNotLocalServer)
                {
                    content = content.Replace("%RESTORE_FILE_PROD%", @"\\" + myHostName + @"\Databaser\P" + replaceFileLatestVersion + "TCO_LATEST.bak");
                    content = content.Replace("%RESTORE_FILE_HIST%", @"\\" + myHostName + @"\Databaser\H" + replaceFileLatestVersion + "TCO_LATEST.bak");
                }

                else
                {
                    content = content.Replace("%RESTORE_FILE_PROD%", @"C:\Databaser\P" + replaceFileLatestVersion + "TCO_LATEST.bak");
                    content = content.Replace("%RESTORE_FILE_HIST%", @"C:\Databaser\H" + replaceFileLatestVersion + "TCO_LATEST.bak");
                }
            }

            if (isNotLocalServer)
            {
                string str = textBoxClient.Text;
                str = str.Insert(str.IndexOf(@"\"), " ");

                content = content.Replace("%CLIENT_UPGRADE%", str);
                content = content.Replace("%CLIENT%", textBoxClient.Text);
            }

            else
            {
                content = content.Replace("%CLIENT_UPGRADE%", textBoxClient.Text);
                content = content.Replace("%CLIENT%", textBoxClient.Text);
            }

            StreamWriter writer = new StreamWriter(filePath);
            writer.Write(content);
            writer.Close();
        }

        private void createBackupSqlFile(string filePath, string content)
        {
            if (isNotLocalServer)
            {
                string str = textBoxBackupPath.Text.Remove(0, 2);
                str = @"\\" + myHostName + str;
                content = content.Replace("%FOLDER_PATH%", str);
            }
            else
                content = content.Replace("%FOLDER_PATH%", textBoxBackupPath.Text);
            content = content.Replace("%DATABASE%", textBoxBackupDb.Text);
            content = content.Replace("%FILENAME%", textBoxBackupFile.Text);

            StreamWriter writer = new StreamWriter(filePath);
            writer.Write(content);
            writer.Close();
        }

        private void createBackupBatFile(string filePath, string content)
        {
            content = content.Replace("%CLIENT%", textBoxBackupClient.Text);
            content = content.Replace("%DATABASE%", textBoxBackupDb.Text);

            StreamWriter writer = new StreamWriter(filePath);
            writer.Write(content);
            writer.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            saveSettings();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            loadSettings();
        }

        private void textBoxFrom_TextChanged(object sender, EventArgs e)
        {
            if (textBoxFrom.Text == "0" || textBoxFrom.Text.Length <= 0)
            {
                checkBoxRestoreDB.Enabled = true;
                button1.Text = "Restore and upgrade to QF";
            }
            else
            {
                checkBoxRestoreDB.Enabled = false;
                button1.Text = "Upgrade to QF";
            }

        }

        private void checkBoxRestoreDB_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxRestoreDB.Enabled == true && radioButtonUpgradeQFdb.Checked == true && checkBoxRestoreDB.Checked == true)
                button1.Text = "Restore and upgrade to QF";
            else
                button1.Text = "Upgrade to QF";
        }

        private void loadSettings()
        {
            textBoxVersion.Text = MySettings.Default.version;
            textBoxClient.Text = MySettings.Default.client;
            textBoxDatabaseP.Text = MySettings.Default.dbp;
            textBoxDatabaseH.Text = MySettings.Default.dbh;
            textBoxPath.Text = MySettings.Default.path;
            textBoxFileProd.Text = MySettings.Default.restoreFileProd;
            textBoxFileHist.Text = MySettings.Default.restoreFileHist;
            textBoxQfPath.Text = MySettings.Default.qfPath;
            textBoxBackupClient.Text = MySettings.Default.backupClient;
            textBoxBackupDb.Text = MySettings.Default.backupDatabase;
            textBoxBackupFile.Text = MySettings.Default.backupFile;
            textBoxBackupPath.Text = MySettings.Default.backupPath;
            checkBoxRestoreDB.Checked = MySettings.Default.checkBoxRestoreDB;
            checkBoxNotCopyFiles.Checked = MySettings.Default.checkBoxNotCopyFiles;
            checkBoxDeleteFolders.Checked = MySettings.Default.checkBoxDeleteFolder;

            switch (MySettings.Default.rb)
            {
                case 1:
                    radioButtonUpgradeQFdb.Checked = true;
                    break;
                case 2:
                    radioButtonRestoreToBase.Checked = true;
                    break;
                case 3:
                    radioButtonRestoreToTRUNK.Checked = true;
                    break;
                case 4:
                    radioButtonRestoreFromOtherFiles.Checked = true;
                    break;
                case 5:
                    rbUpgradeFromPath.Checked = true;
                    break;
                case 6:
                    rbBackupDb.Checked = true;
                    break;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void saveSettings()
        {
            if (radioButtonUpgradeQFdb.Checked)
                rbState = 1;
            if (radioButtonRestoreToBase.Checked)
                rbState = 2;
            if (radioButtonRestoreToTRUNK.Checked)
                rbState = 3;
            if (radioButtonRestoreFromOtherFiles.Checked)
                rbState = 4;
            if (rbUpgradeFromPath.Checked)
                rbState = 5;
            if (rbBackupDb.Checked)
                rbState = 6;

            if (checkBoxRestoreDB.Checked == true)
                MySettings.Default.checkBoxRestoreDB = true;
            if (checkBoxRestoreDB.Checked == false)
                MySettings.Default.checkBoxRestoreDB = false;
            if (checkBoxNotCopyFiles.Checked == true)
                MySettings.Default.checkBoxNotCopyFiles = true;
            if (checkBoxNotCopyFiles.Checked == false)
                MySettings.Default.checkBoxNotCopyFiles = false;
            if (checkBoxDeleteFolders.Checked == true)
                MySettings.Default.checkBoxDeleteFolder = true;
            if (checkBoxDeleteFolders.Checked == false)
                MySettings.Default.checkBoxDeleteFolder = false;

            MySettings.Default.version = textBoxVersion.Text;
            MySettings.Default.client = textBoxClient.Text;
            MySettings.Default.dbp = textBoxDatabaseP.Text;
            MySettings.Default.dbh = textBoxDatabaseH.Text;
            MySettings.Default.path = textBoxPath.Text;
            MySettings.Default.restoreFileProd = textBoxFileProd.Text;
            MySettings.Default.restoreFileHist = textBoxFileHist.Text;
            MySettings.Default.rb = rbState;
            MySettings.Default.qfPath = textBoxQfPath.Text;
            MySettings.Default.backupClient = textBoxBackupClient.Text;
            MySettings.Default.backupDatabase = textBoxBackupDb.Text;
            MySettings.Default.backupFile = textBoxBackupFile.Text;
            MySettings.Default.backupPath = textBoxBackupPath.Text;
            MySettings.Default.Save();
        }

        private void buttonFolderPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBoxPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                strFileProd = openFileDialog1.FileName;
                this.textBoxFileProd.Text = strFileProd;
            }
        }

        private void buttonFileHist_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                strFileHist = openFileDialog1.FileName;
                this.textBoxFileHist.Text = strFileHist;
            }
        }

        private void buttonQfPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBoxQfPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(help);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Made by:\nPeter Gustafsson");
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBoxFrom.Enabled = true;
            textBoxTo.Enabled = true;
            textBoxVersion.Enabled = true;
            textBoxClient.Enabled = true;
            textBoxDatabaseP.Enabled = true;
            textBoxDatabaseH.Enabled = true;
            textBoxPath.Enabled = false;
            checkBoxRestoreDB.Enabled = true;
            label1.Enabled = true;
            label2.Enabled = true;
            label3.Enabled = true;
            label4.Enabled = true;
            label5.Enabled = true;
            label6.Enabled = true;
            label7.Enabled = false;
            label8.Enabled = false;
            label9.Enabled = false;
            label10.Enabled = false;
            label11.Enabled = false;
            buttonFolderPath.Enabled = false;
            labelFileProd.Enabled = false;
            labelFileHist.Enabled = false;
            textBoxFileProd.Enabled = false;
            textBoxFileHist.Enabled = false;
            buttonFileProd.Enabled = false;
            buttonFileHist.Enabled = false;
            button1.Enabled = true;
            labelQfPath.Enabled = true;
            textBoxQfPath.Enabled = true;
            buttonQfPath.Enabled = true;
            checkBoxNotCopyFiles.Enabled = true;
            checkBoxNotCopyFiles.Enabled = false;
            buttonBackupPath.Enabled = false;
            textBoxBackupClient.Enabled = false;
            textBoxBackupDb.Enabled = false;
            textBoxBackupPath.Enabled = false;
            textBoxBackupFile.Enabled = false;
            if (checkBoxRestoreDB.Enabled == true && radioButtonUpgradeQFdb.Checked == true && checkBoxRestoreDB.Checked == true)
                button1.Text = "Restore and upgrade to QF";
            else
                button1.Text = "Upgrade to QF";
            checkBoxDeleteFolders.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBoxFrom.Enabled = false;
            textBoxTo.Enabled = false;
            textBoxVersion.Enabled = true;
            textBoxClient.Enabled = true;
            textBoxDatabaseP.Enabled = true;
            textBoxDatabaseH.Enabled = true;
            textBoxPath.Enabled = false;
            checkBoxRestoreDB.Enabled = false;
            label1.Enabled = false;
            label2.Enabled = false;
            label3.Enabled = true;
            label4.Enabled = true;
            label5.Enabled = true;
            label6.Enabled = true;
            label7.Enabled = false;
            label8.Enabled = false;
            label9.Enabled = false;
            label10.Enabled = false;
            label11.Enabled = false;
            buttonFolderPath.Enabled = false;
            labelFileProd.Enabled = false;
            labelFileHist.Enabled = false;
            textBoxFileProd.Enabled = false;
            textBoxFileHist.Enabled = false;
            buttonFileProd.Enabled = false;
            buttonFileHist.Enabled = false;
            button1.Enabled = true;
            labelQfPath.Enabled = false;
            textBoxQfPath.Enabled = false;
            buttonQfPath.Enabled = false;
            checkBoxNotCopyFiles.Enabled = true;
            buttonBackupPath.Enabled = false;
            textBoxBackupClient.Enabled = false;
            textBoxBackupDb.Enabled = false;
            textBoxBackupPath.Enabled = false;
            textBoxBackupFile.Enabled = false;
            button1.Text = "Restore from base";
            checkBoxDeleteFolders.Enabled = false;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            textBoxFrom.Enabled = false;
            textBoxTo.Enabled = false;
            textBoxVersion.Enabled = true;
            textBoxClient.Enabled = true;
            textBoxDatabaseP.Enabled = true;
            textBoxDatabaseH.Enabled = true;
            textBoxPath.Enabled = false;
            checkBoxRestoreDB.Enabled = false;
            label1.Enabled = false;
            label2.Enabled = false;
            label3.Enabled = true;
            label4.Enabled = true;
            label5.Enabled = true;
            label6.Enabled = true;
            label7.Enabled = false;
            label8.Enabled = false;
            label9.Enabled = false;
            label10.Enabled = false;
            label11.Enabled = false;
            buttonFolderPath.Enabled = false;
            labelFileProd.Enabled = false;
            labelFileHist.Enabled = false;
            textBoxFileProd.Enabled = false;
            textBoxFileHist.Enabled = false;
            buttonFileProd.Enabled = false;
            buttonFileHist.Enabled = false;
            button1.Enabled = true;
            labelQfPath.Enabled = false;
            textBoxQfPath.Enabled = false;
            buttonQfPath.Enabled = false;
            checkBoxNotCopyFiles.Enabled = true;
            buttonBackupPath.Enabled = false;
            textBoxBackupClient.Enabled = false;
            textBoxBackupDb.Enabled = false;
            textBoxBackupPath.Enabled = false;
            textBoxBackupFile.Enabled = false;
            button1.Text = "Restore to TRUNK";
            checkBoxDeleteFolders.Enabled = true;
        }

        private void radioButtonRestoreOtherFiles_CheckedChanged(object sender, EventArgs e)
        {
            textBoxFrom.Enabled = false;
            textBoxTo.Enabled = false;
            textBoxVersion.Enabled = false;
            textBoxClient.Enabled = true;
            textBoxDatabaseP.Enabled = true;
            textBoxDatabaseH.Enabled = true;
            textBoxPath.Enabled = false;
            checkBoxRestoreDB.Enabled = false;
            label1.Enabled = false;
            label2.Enabled = false;
            label3.Enabled = true;
            label4.Enabled = true;
            label5.Enabled = true;
            label6.Enabled = false;
            label7.Enabled = false;
            label8.Enabled = false;
            label9.Enabled = false;
            label10.Enabled = false;
            label11.Enabled = false;
            buttonFolderPath.Enabled = false;
            labelFileProd.Enabled = true;
            labelFileHist.Enabled = true;
            textBoxFileProd.Enabled = true;
            textBoxFileHist.Enabled = true;
            buttonFileProd.Enabled = true;
            buttonFileHist.Enabled = true;
            button1.Enabled = true;
            labelQfPath.Enabled = false;
            textBoxQfPath.Enabled = false;
            buttonQfPath.Enabled = false;
            checkBoxNotCopyFiles.Enabled = true;
            buttonBackupPath.Enabled = false;
            textBoxBackupClient.Enabled = false;
            textBoxBackupDb.Enabled = false;
            textBoxBackupPath.Enabled = false;
            textBoxBackupFile.Enabled = false;
            button1.Text = "Restore from files";
            checkBoxDeleteFolders.Enabled = false;
        }

        private void rbUpgradeFromPath_CheckedChanged(object sender, EventArgs e)
        {
            textBoxFrom.Enabled = false;
            textBoxTo.Enabled = false;
            textBoxVersion.Enabled = false;
            textBoxClient.Enabled = true;
            textBoxDatabaseP.Enabled = true;
            textBoxDatabaseH.Enabled = true;
            textBoxPath.Enabled = true;
            checkBoxRestoreDB.Enabled = false;
            label1.Enabled = false;
            label2.Enabled = false;
            label3.Enabled = true;
            label4.Enabled = true;
            label5.Enabled = true;
            label6.Enabled = false;
            label7.Enabled = true;
            label8.Enabled = false;
            label9.Enabled = false;
            label10.Enabled = false;
            label11.Enabled = false;
            buttonFolderPath.Enabled = true;
            labelFileProd.Enabled = false;
            labelFileHist.Enabled = false;
            textBoxFileProd.Enabled = false;
            textBoxFileHist.Enabled = false;
            buttonFileProd.Enabled = false;
            buttonFileHist.Enabled = false;
            button1.Enabled = true;
            labelQfPath.Enabled = false;
            textBoxQfPath.Enabled = false;
            buttonQfPath.Enabled = false;
            checkBoxNotCopyFiles.Enabled = false;
            buttonBackupPath.Enabled = false;
            textBoxBackupClient.Enabled = false;
            textBoxBackupDb.Enabled = false;
            textBoxBackupPath.Enabled = false;
            textBoxBackupFile.Enabled = false;
            button1.Text = "Upgrade from path";
            checkBoxDeleteFolders.Enabled = false;
        }

        private void rbBackupDb_CheckedChanged(object sender, EventArgs e)
        {
            textBoxFrom.Enabled = false;
            textBoxTo.Enabled = false;
            textBoxVersion.Enabled = false;
            textBoxClient.Enabled = false;
            textBoxDatabaseP.Enabled = false;
            textBoxDatabaseH.Enabled = false;
            textBoxPath.Enabled = false;
            checkBoxRestoreDB.Enabled = false;
            label1.Enabled = false;
            label2.Enabled = false;
            label3.Enabled = false;
            label4.Enabled = false;
            label5.Enabled = false;
            label6.Enabled = false;
            label7.Enabled = false;
            label8.Enabled = true;
            label9.Enabled = true;
            label10.Enabled = true;
            label11.Enabled = true;
            buttonFolderPath.Enabled = false;
            labelFileProd.Enabled = false;
            labelFileHist.Enabled = false;
            textBoxFileProd.Enabled = false;
            textBoxFileHist.Enabled = false;
            buttonFileProd.Enabled = false;
            buttonFileHist.Enabled = false;
            button1.Enabled = true;
            labelQfPath.Enabled = false;
            textBoxQfPath.Enabled = false;
            buttonQfPath.Enabled = false;
            checkBoxNotCopyFiles.Enabled = false;
            buttonBackupPath.Enabled = true;
            textBoxBackupClient.Enabled = true;
            textBoxBackupDb.Enabled = true;
            textBoxBackupPath.Enabled = true;
            textBoxBackupFile.Enabled = true;
            button1.Text = "Backup database";
            checkBoxDeleteFolders.Enabled = false;
        }

        static public void startFile(string file)
        {
            if (File.Exists(file))
            {
                Process proc = new Process();
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.Arguments = @"/C " + file;
                proc.Start();
                proc.WaitForExit();
                proc.Close();
            }
            else
                MessageBox.Show("File " + file + " doesn't exists");
        }

        private string getSubFolderPath(string strPath, string SearchString)
        {
            try
            {
                string path = "";
                string[] dirs = Directory.GetDirectories(strPath, SearchString);
                Array.Sort(dirs);
                Array.Reverse(dirs);
                foreach (string dir in dirs)
                {
                    string firstNormalizedSearchString = SearchString.Replace("*", "");
                    string secondNormalizedSearchString = dir.Replace(" ", "");
                    if (secondNormalizedSearchString.ToLower().Contains(firstNormalizedSearchString.ToLower()))
                        path = dir;
                }
                return path;
                
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
                return null;
            }
        }


        private bool FileInUse(string path)
        {
            try
            {
                //if file is not lock then below statement will successfully executed otherwise it's goes to catch.
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                    return false;
            }
            catch (IOException)
            {
                return true;
            }
        }

        private void deleteFolderPath(string strPath, string SearchString)
        {
            try
            {
                string[] dirs = Directory.GetDirectories(strPath, SearchString);
                foreach (string dir in dirs)
                {
                    if (Directory.Exists(dir))
                    {
                        DialogResult dialogResult = MessageBox.Show("Do you want to delete the old db folder:\n" + dir, "Delete!", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                            Directory.Delete(dir, true);
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}