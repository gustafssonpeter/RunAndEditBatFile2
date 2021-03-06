﻿using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Linq;

namespace DB_Updater
{
    public partial class Form1 : Form
    {
        int fromQf, toQf, rbState;
        bool isFirstRun, isNumberFrom, isNumberTo, isFromBaseToQf1, isRestored, upgradeToQfNext, useQfNextPath;
        string replaceFileLatestVersion, strFileProd, strFileHist, restoreToBaseBat, setupLocalTrunkBat,
            restoreAndToQF1bat, dirForUpgradeFiles, qfFileName, dirForCopyQfFiles, qfServerPath, pathToDeliveryOrSystem, qfPath;
        string myHostName = System.Net.Dns.GetHostName();
        string About = File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString("yyyy.MM.dd.HHmm");

        string help =
@"To use this program you need to create the directory C:\Databaser on your computer. This directory need to be shared to the network.

QF files will automatically be copied to the directory -> ""Path to QF files folder""

To restore to Trunk the Base version needs to be the previous version as Trunk.
e.g. if Trunk is 5.12, base should be 5.11";

        //        string copyUnzipFile =
        //@"
        //copy \\profdoc.lab\dfs01\Gemensam\Test\Verktyg\unzip.exe c:\Databaser\unzip.exe /Y";

        //        string copyDbFiles =
        //@"
        //copy \\profdoc.lab\dfs01\Databaser\Test\Orginal\%VERSION%\P%FILE_VERSION%TCO_LATEST.bak c:\Databaser\P%FILE_VERSION%TCO_LATEST.bak /Y
        //copy \\profdoc.lab\dfs01\Databaser\Test\Orginal\%VERSION%\H%FILE_VERSION%TCO_LATEST.bak c:\Databaser\H%FILE_VERSION%TCO_LATEST.bak /Y";

        string upgradeToLatestTrunk =
@"
copy \\profdoc.lab\dfs01\Gemensam\Test\Verktyg\unzip.exe c:\Databaser\unzip.exe /Y
for /f ""delims="" %%i in (\\profdoc.lab\dfs01\System\Autobuild\dblatest.txt) do copy ""\\profdoc.lab\dfs01\System\Autobuild\%%i.exe"" ""c:\databaser"" /Y
for /f ""delims="" %%i in (\\profdoc.lab\dfs01\System\Autobuild\dblatest.txt) do unzip ""c:\databaser\%%i.exe""
for /f ""delims="" %%i in (\\profdoc.lab\dfs01\System\Autobuild\dblatest.txt) do cd ""c:\databaser\%%i""
START /B ""Upgrade historic"" ""Upgrade Historic.bat"" SYSADM SYSADM %DATABASE_H% %CLIENT_UPGRADE%
START ""Upgrade production"" ""Upgrade Production.bat"" SYSADM SYSADM %DATABASE_P% %CLIENT_UPGRADE%";

        string runRestoreScript =
@"
copy \\profdoc.lab\dfs01\Databaser\Test\Orginal\%VERSION%\P%FILE_VERSION%TCO_LATEST.bak c:\Databaser\P%FILE_VERSION%TCO_LATEST.bak /Y
copy \\profdoc.lab\dfs01\Databaser\Test\Orginal\%VERSION%\H%FILE_VERSION%TCO_LATEST.bak c:\Databaser\H%FILE_VERSION%TCO_LATEST.bak /Y
        cd ""c:\databaser""
sqlcmd -S %CLIENT% -d %DATABASE_P% -U SYSADM -P SYSADM -i DBupdate_restoreSQL.sql -o ""c:\databaser\DBupdate_restoreSQL.txt""";

        string runRestoreScriptHist =
@"
copy \\profdoc.lab\dfs01\Databaser\Test\Orginal\%VERSION%\H%FILE_VERSION%TCO_LATEST.bak c:\Databaser\H%FILE_VERSION%TCO_LATEST.bak /Y
cd ""c:\databaser""
sqlcmd -S %CLIENT% -d %DATABASE_H% -U SYSADM -P SYSADM -i DBupdate_restoreSQL.sql -o ""c:\databaser\DBupdate_restoreSQL.txt""";

        string upgradeFromQfToQf =
@"
copy \\profdoc.lab\dfs01\Gemensam\Test\Verktyg\unzip.exe %QF_DIR_PATH%\unzip.exe /Y
copy ""%PATH_TO_FIND_QF_FILES_TO_COPY%"" ""%QF_DIR_PATH%\%QF_FILE_NAME%"" /Y
cd ""%QF_DIR_PATH%""
unzip -o ""%QF_DIR_PATH%\%QF_FILE_NAME%""
cd ""%QF_FILE_PATH%""
START /B ""Upgrade historic"" ""Upgrade Historic From %VERSION% QF%FROM% To %VERSION% QF%TO%.bat"" SYSADM SYSADM %DATABASE_H% %CLIENT_UPGRADE%
START /B ""Upgrade production"" ""Upgrade Production From %VERSION% QF%FROM% To %VERSION% QF%TO%.bat"" SYSADM SYSADM %DATABASE_P% %CLIENT_UPGRADE% 
pause";

        string upgradeToQF1 =
@"
copy \\profdoc.lab\dfs01\Gemensam\Test\Verktyg\unzip.exe %QF_DIR_PATH%\unzip.exe /Y
copy ""%PATH_TO_FIND_QF_FILES_TO_COPY%"" ""%QF_DIR_PATH%\%QF_FILE_NAME%"" /Y
cd ""%QF_DIR_PATH%""
unzip -o ""%QF_DIR_PATH%\%QF_FILE_NAME%""
cd ""%QF_FILE_PATH%""
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

        string sqlRestoreScriptHist =
@"
use master
RESTORE DATABASE %DATABASE_H% FROM  DISK = N'%RESTORE_FILE_HIST%' WITH  FILE = 1,  NOUNLOAD,  REPLACE,  STATS = 10
GO
use %DATABASE_H%
update databaser set dat_databas = '%DATABASE_H%', dat_orgdatabas='%DATABASE_H%', dat_servernamn='%CLIENT%' where dat_typ ='H'
Exec a_ResetHistProdUser";

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
            if (radioButtonUpgradeQFdb.Checked)
            {
                useQfNextPath = false;
                upgradeToQfNext = false;
                string pathToDeliveryQfDatabase = @"\\profdoc.lab\dfs01\Utveckling\Delivery\" + getVersionDot() + @"\Arkiv\QF Database\";
                //To check how many qf dirs there is
                if (int.Parse(getVersionDot().Replace(".", "")) >= 510)
                {
                    if (countDirs(pathToDeliveryQfDatabase, "*QF*") < int.Parse(textBoxTo.Text))
                    {
                        if (countDirs(pathToDeliveryQfDatabase, "*QF*") != -1)
                        {
                            DialogResult dialogResult = MessageBox.Show("There are only " + countDirs(pathToDeliveryQfDatabase, "*QF*") + " QF's available on Delivery!\nDo you want to upgrade to next QF (Not yet released)?", "Upgrade to QF Next", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                                upgradeToQfNext = true;
                            else
                                upgradeToQfNext = false;
                            startQfUpgrade();
                        }
                        else
                            MessageBox.Show("There are no QF's available on Delivery for version " + getVersionDot());
                    }
                    else
                        startQfUpgrade();
                }
                //To check how many qf files there is
                else
                {
                    if (countFiles(pathToDeliveryQfDatabase, "*QF*") < int.Parse(textBoxTo.Text))
                    {
                        if (countFiles(pathToDeliveryQfDatabase, "*QF*") != -1)
                            MessageBox.Show("There are only " + countFiles(pathToDeliveryQfDatabase, "*QF*") + " QF's available!");
                        else
                            MessageBox.Show("There are no QF's available for version " + getVersionDot());
                    }
                    else
                        startQfUpgrade();
                }
            }

            if (radioButtonRestoreToBase.Checked)
                restoreToBaseVersion();

            if (radioButtonRestoreToTRUNK.Checked)
                restoreToTrunk();

            if (radioButtonRestoreFromOtherFiles.Checked)
            {
                if (!isLocalServer() && (textBoxFileProd.Text.Contains(@"c:") || textBoxFileHist.Text.Contains(@"c:")))
                    MessageBox.Show("You can't restore from a local files when you restore a database on a server");
                else
                    restoreFromOtherFiles();
            }

            if (rbUpgradeFromPath.Checked)
                upgradeFromPathFiles();

            if (rbBackupDb.Checked)
            {
                if (!isLocalServer() && textBoxBackupPath.Text.Contains(@"c:"))
                    MessageBox.Show("You can't backup to a local file when you backup from a server");
                else
                    runDbBackup();
            }


            if (isRestored)
            {
                try
                {
                    MessageBox.Show(File.ReadAllText("C:\\databaser\\DBupdate_restoreSQL.txt"), "Restore result!");
                }
                catch
                {
                    return;
                }

                isRestored = false;
            }
            if (checkBoxAutoSave.Checked == true)
                saveSettings();
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
            //checkBoxCopyFiles.Checked = false;
            checkBoxRestoreDB.Checked = false;
            //checkBoxDeleteFolders.Checked = false;
        }

        private void startQfUpgrade()
        {
            int outputValue, count;
            if (!String.IsNullOrEmpty(textBoxFrom.Text) && !String.IsNullOrEmpty(textBoxTo.Text) && !String.IsNullOrEmpty(textBoxVersion.Text)
                && !String.IsNullOrEmpty(textBoxClient.Text) && !String.IsNullOrEmpty(textBoxDatabaseP.Text) && !String.IsNullOrEmpty(textBoxDatabaseH.Text) && !String.IsNullOrEmpty(textBoxQfPath.Text))
            {
                isNumberFrom = int.TryParse(textBoxFrom.Text, out outputValue);
                isNumberTo = int.TryParse(textBoxTo.Text, out outputValue);

                if (!isNumberFrom || !isNumberTo)
                    MessageBox.Show("Only numbers in QF From and QF To!");
                else
                {
                    if (Directory.Exists(textBoxQfPath.Text))
                    {
                        //För att beräkna hur många ggr vi ska köra loopen med count
                        if (int.Parse(getVersionDot().Replace(".", "")) >= 510)
                        {
                            if (upgradeToQfNext)
                            {
                                count = countDirs(@"\\profdoc.lab\dfs01\Utveckling\Delivery\" + getVersionDot() + @"\Arkiv\QF Database\", "*QF*") - int.Parse(textBoxFrom.Text);
                                count++;
                            }
                            else
                            {
                                if (int.Parse(textBoxTo.Text) > countDirs(@"\\profdoc.lab\dfs01\Utveckling\Delivery\" + getVersionDot() + @"\Arkiv\QF Database\", "*QF*"))
                                    count = countDirs(@"\\profdoc.lab\dfs01\Utveckling\Delivery\" + getVersionDot() + @"\Arkiv\QF Database\", "*QF*");
                                else
                                    count = int.Parse(textBoxTo.Text) - int.Parse(textBoxFrom.Text);
                            }
                        }
                        else
                        {
                            if (int.Parse(textBoxTo.Text) > countFiles(@"\\profdoc.lab\dfs01\Utveckling\Delivery\" + getVersionDot() + @"\Arkiv\QF Database\", "*QF*"))
                                count = countFiles(@"\\profdoc.lab\dfs01\Utveckling\Delivery\" + getVersionDot() + @"\Arkiv\QF Database\", "*QF*");
                            else
                                count = int.Parse(textBoxTo.Text) - int.Parse(textBoxFrom.Text);
                        }

                        fromQf = int.Parse(textBoxFrom.Text);
                        isFirstRun = true;

                        while (count > 0)
                        {
                            //if-sats för första körningen
                            if (textBoxFrom.Text == "0" && isFirstRun)
                            {
                                restoreAndToQF1bat = "";
                                isFromBaseToQf1 = true;
                                toQf = 1;
                                if (checkBoxRestoreDB.Checked == true)
                                {
                                    createFile("C:\\Databaser\\DBupdate_restoreSQL.sql", sqlRestoreScript);
                                    //if (checkBoxCopyFiles.Checked == true)
                                    //{
                                 //   if (isLocalServer())
                                        restoreAndToQF1bat = runRestoreScript + upgradeToQF1;
                                    //restoreAndToQF1bat = copyUnzipFile + copyDbFiles + runRestoreScript + upgradeToQF1;
                                //    else
                                        //restoreAndToQF1bat = copyUnzipFile + runRestoreScript + upgradeToQF1;
                               //         restoreAndToQF1bat = runRestoreScript + upgradeToQF1;
                                    //}

                                    //else
                                    //    restoreAndToQF1bat = runRestoreScript + upgradeToQF1;
                                    isRestored = true;
                                }
                                else
                                {
                                    restoreAndToQF1bat = upgradeToQF1;
                                    isRestored = false;
                                }
                                createFile("C:\\Databaser\\DBupdate_RestoreQF.bat", restoreAndToQF1bat);
                                startFile("C:\\Databaser\\DBupdate_RestoreQF.bat");
                            }

                            //if-sats för resterande QF'ar
                            if (!isFromBaseToQf1)
                            {
                                if (textBoxFrom.Text == "0")
                                    fromQf++;
                                toQf = fromQf + 1;

                                if (int.Parse(getVersionDot().Replace(".", "")) >= 510)
                                {
                                    string path = @"\\profdoc.lab\dfs01\Utveckling\Delivery\" + getVersionDot() + @"\Arkiv\QF Database\";
                                    if (toQf > countDirs(path, "*QF*"))
                                        useQfNextPath = true;
                                    else
                                        useQfNextPath = false;
                                }

                                createFile("C:\\Databaser\\DBupdate_QfToQf.bat", upgradeFromQfToQf);
                                startFile("C:\\Databaser\\DBupdate_QfToQf.bat");

                                if (textBoxFrom.Text != "0")
                                    fromQf++;
                            }
                            count--;
                            isFirstRun = false;
                            isFromBaseToQf1 = false;
                            //Delete zip file
                            if (File.Exists(textBoxQfPath.Text + @"\" + qfFileName) && isFileInUse(textBoxQfPath.Text + @"\" + qfFileName) == false)
                                File.Delete(textBoxQfPath.Text + @"\" + qfFileName);
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
                //if (checkBoxCopyFiles.Checked == true)
                //{
                    if (isLocalServer())
                        //restoreToBaseBat = copyUnzipFile + copyDbFiles + runRestoreScript;
                        restoreToBaseBat = runRestoreScript;
                    else
                        //restoreToBaseBat = copyUnzipFile + runRestoreScript;
                        restoreToBaseBat = runRestoreScript;
                //}
                //else
                //    restoreToBaseBat = runRestoreScript;
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
                String strDbFileName = "";
                //Delete old db folders
                //if (checkBoxDeleteFolders.Checked == true)
                    deleteFolderPath(@"c:\databaser", @"CGM ANALYTIX database*");
                if (File.Exists(@"\\profdoc.lab\dfs01\System\Autobuild\dblatest.txt"))
                    strDbFileName = File.ReadAllText(@"\\profdoc.lab\dfs01\System\Autobuild\dblatest.txt");

                setupLocalTrunkBat = "";
                //if (checkBoxCopyFiles.Checked == true)
                //{
                 //   if (isLocalServer())
                        //setupLocalTrunkBat = copyUnzipFile + copyDbFiles + runRestoreScript + upgradeToLatestTrunk;
                 //       setupLocalTrunkBat = runRestoreScript + upgradeToLatestTrunk;
                   // else
                        //setupLocalTrunkBat = copyUnzipFile + runRestoreScript + upgradeToLatestTrunk;
                        setupLocalTrunkBat = runRestoreScript + upgradeToLatestTrunk;
                //}
                //else
                //    setupLocalTrunkBat = runRestoreScript + upgradeToLatestTrunk;

                createFile("C:\\Databaser\\DBupdate_restoreSQL.sql", sqlRestoreScript);
                createFile("C:\\Databaser\\DBupdate_SetupLocalTrunk.bat", setupLocalTrunkBat);
                isRestored = true;
                startFile("C:\\Databaser\\DBupdate_SetupLocalTrunk.bat");

                //Delete the zip file
                string filepath = @"c:\databaser\" + strDbFileName + ".exe";
                if (File.Exists(filepath) && isFileInUse(filepath) == false)
                    File.Delete(@"c:\databaser\" + strDbFileName + ".exe");
            }
            else
                MessageBox.Show("Please enter:\rVersion, Client, Database Prod. and Database Hist.");
        }

        private void restoreFromOtherFiles()
        {
            if (!String.IsNullOrEmpty(textBoxClient.Text) && (!String.IsNullOrEmpty(textBoxDatabaseP.Text) || !String.IsNullOrEmpty(textBoxDatabaseH.Text)) && (!String.IsNullOrEmpty(textBoxFileProd.Text) || !String.IsNullOrEmpty(textBoxFileHist.Text)))
            {
                if (File.Exists(textBoxFileHist.Text) || File.Exists(textBoxFileProd.Text))
                {
                    if (String.IsNullOrEmpty(textBoxDatabaseP.Text) || String.IsNullOrEmpty(textBoxFileProd.Text))
                    {
                        createFile("C:\\Databaser\\DBupdate_restoreSQL.sql", sqlRestoreScriptHist);
                        createFile("C:\\Databaser\\DBupdate_RestoreFromOtherFiles.bat", runRestoreScriptHist);
                    }

                    if (String.IsNullOrEmpty(textBoxDatabaseH.Text) || String.IsNullOrEmpty(textBoxFileHist.Text))
                    {
                        createFile("C:\\Databaser\\DBupdate_restoreSQL.sql", sqlRestoreScriptProd);
                        createFile("C:\\Databaser\\DBupdate_RestoreFromOtherFiles.bat", runRestoreScript);
                    }

                    if (!String.IsNullOrEmpty(textBoxDatabaseP.Text) && !String.IsNullOrEmpty(textBoxDatabaseH.Text))
                    {
                        createFile("C:\\Databaser\\DBupdate_restoreSQL.sql", sqlRestoreScript);
                        createFile("C:\\Databaser\\DBupdate_RestoreFromOtherFiles.bat", runRestoreScript);
                    }
                    startFile("C:\\Databaser\\DBupdate_RestoreFromOtherFiles.bat");
                    isRestored = true;
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
                this.textBoxBackupPath.Text = folderBrowserDialog1.SelectedPath;
        }

        private void createFile(string filePath, string content)
        {
            qfPath = "";
            if (textBoxVersion.Text.Contains("."))
                replaceFileLatestVersion = textBoxVersion.Text.Replace(".", "") + "0";
            if (textBoxVersion.Text.Contains(","))
                replaceFileLatestVersion = textBoxVersion.Text.Replace(",", "") + "0";


            if (radioButtonUpgradeQFdb.Checked == true)
            {
                if (int.Parse(getVersionDot().Replace(".", "")) <= 59)
                {
                    dirForCopyQfFiles = @"\\profdoc.lab\dfs01\Utveckling\Delivery\" + getVersionDot() + @"\Arkiv\QF Database\";
                    string pattern = "CGM LAB ANALYTIX Database " + getVersionDot() + " QF" + toQf.ToString() + "*";

                    if (toQf == 1)
                    {
                        try
                        {
                            var dirInfo = new DirectoryInfo(dirForCopyQfFiles);
                            var file = (from f in dirInfo.GetFiles(pattern) orderby f.LastWriteTime descending select f).Last();
                            qfFileName = file.ToString();
                            dirForUpgradeFiles = file.ToString().Replace(".exe", "");
                        }
                        catch
                        {
                            MessageBox.Show("There are no QF files!");
                            return;
                        }
                    }
                    else
                    {
                        try
                        {
                            var dirInfo = new DirectoryInfo(dirForCopyQfFiles);
                            var myFile = (from f in dirInfo.GetFiles(pattern) orderby f.LastWriteTime descending select f).First();
                            qfFileName = myFile.ToString();
                            dirForUpgradeFiles = myFile.ToString().Replace(".exe", "");
                        }
                        catch
                        {
                            MessageBox.Show("There are no more QF files!");
                            return;
                        }
                    }
                    qfServerPath = "";
                    pathToDeliveryOrSystem = @"\\profdoc.lab\dfs01\Utveckling\Delivery\";
                }
                else
                {
                    if (upgradeToQfNext && useQfNextPath)
                    {
                        dirForCopyQfFiles = @"\\profdoc.lab\dfs01\System\Autobuild\";
                        string pattern = "CGM ANALYTIX Database " + getVersionDot() + " QF" + toQf.ToString() + "*";

                        try
                        {
                            var dirInfo = new DirectoryInfo(dirForCopyQfFiles);
                            var file = (from f in dirInfo.GetFiles(pattern) orderby f.LastWriteTime descending select f).Last();
                            qfFileName = file.ToString();
                            dirForUpgradeFiles = file.ToString().Replace(".exe", "");
                        }
                        catch
                        {
                            MessageBox.Show("There are no QF next files!");
                            return;
                        }
                        pathToDeliveryOrSystem = @"\\profdoc.lab\dfs01\System\Autobuild\";
                    }

                    else
                    {
                        dirForCopyQfFiles = @"\\profdoc.lab\dfs01\Utveckling\Delivery\" + getVersionDot() + @"\Arkiv\QF Database\QF" + toQf.ToString() + @"\";

                        try
                        {
                            var directory = new DirectoryInfo(dirForCopyQfFiles);
                            var myFile = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First();
                            qfFileName = myFile.ToString();
                            dirForUpgradeFiles = myFile.ToString().Replace(".exe", "");
                        }
                        catch
                        {
                            MessageBox.Show("There are no more QF files!");
                            return;
                        }
                        pathToDeliveryOrSystem = @"\\profdoc.lab\dfs01\Utveckling\Delivery\";
                    }
                    qfServerPath = "QF" + toQf.ToString() + "\\";
                }
                if (upgradeToQfNext && useQfNextPath)
                    qfPath = pathToDeliveryOrSystem + qfFileName;
                else
                    qfPath = pathToDeliveryOrSystem + getVersionDot() + @"\Arkiv\QF Database\" + qfServerPath + qfFileName;

                content = content.Replace("%PATH_TO_FIND_QF_FILES_TO_COPY%", qfPath);
                content = content.Replace("%QF_FILE_NAME%", qfFileName);
                content = content.Replace("%QF_FILE_PATH%", textBoxQfPath.Text + @"\" + dirForUpgradeFiles);
                content = content.Replace("%TO%", toQf.ToString());
                content = content.Replace("%FROM%", fromQf.ToString());
                content = content.Replace("%VERSION_DOT%", getVersionDot());
                content = content.Replace("%QF_DIR_PATH%", textBoxQfPath.Text);
            }
            content = content.Replace("%DATABASE_P%", textBoxDatabaseP.Text);
            content = content.Replace("%DATABASE_H%", textBoxDatabaseH.Text);
            content = content.Replace("%VERSION%", getVersionDot());
           // if (checkBoxCopyFiles.Checked == true && checkBoxCopyFiles.Enabled == true)
                content = content.Replace("%FILE_VERSION%", replaceFileLatestVersion);
            if (rbUpgradeFromPath.Checked == true)
                content = content.Replace("%PATH%", textBoxPath.Text);

            if (radioButtonRestoreFromOtherFiles.Checked)
            {
                if (!String.IsNullOrEmpty(textBoxFileProd.Text))
                    content = content.Replace("%RESTORE_FILE_PROD%", textBoxFileProd.Text);
                if (!String.IsNullOrEmpty(textBoxFileHist.Text))
                    content = content.Replace("%RESTORE_FILE_HIST%", textBoxFileHist.Text);
            }
            else
            {
                if (isLocalServer())
                {
                    content = content.Replace("%RESTORE_FILE_PROD%", @"C:\Databaser\P" + replaceFileLatestVersion + "TCO_LATEST.bak");
                    content = content.Replace("%RESTORE_FILE_HIST%", @"C:\Databaser\H" + replaceFileLatestVersion + "TCO_LATEST.bak");
                }
                else
                {
                    content = content.Replace("%RESTORE_FILE_PROD%", @"\\profdoc.lab\dfs01\Databaser\Test\Orginal\" + textBoxVersion.Text.Replace(",", ".") + @"\P" + replaceFileLatestVersion + "TCO_LATEST.bak");
                    content = content.Replace("%RESTORE_FILE_HIST%", @"\\profdoc.lab\dfs01\Databaser\Test\Orginal\" + textBoxVersion.Text.Replace(",", ".") + @"\H" + replaceFileLatestVersion + "TCO_LATEST.bak");
                }
            }

            if (isLocalServer())
            {
                content = content.Replace("%CLIENT_UPGRADE%", textBoxClient.Text);
                content = content.Replace("%CLIENT%", textBoxClient.Text);
            }

            else
            {
                string str = textBoxClient.Text;
                if (textBoxClient.Text.IndexOf(@"\") != -1)
                    str = str.Insert(str.IndexOf(@"\"), " ");
                if (textBoxClient.Text.IndexOf(@"/") != -1)
                {
                    str = str.Replace(@"/", @"\");
                    str = str.Insert(str.IndexOf(@"\"), " ");
                }
                content = content.Replace("%CLIENT_UPGRADE%", str);
                if (textBoxClient.Text.IndexOf(@"/") != -1)
                {
                    str = textBoxClient.Text;
                    str = str.Replace(@"/", @"\");
                    content = content.Replace("%CLIENT%", str);
                }
                else
                    content = content.Replace("%CLIENT%", textBoxClient.Text);
            }
            StreamWriter writer = new StreamWriter(filePath);
            writer.Write(content);
            writer.Close();
        }

        private void createBackupSqlFile(string filePath, string content)
        {
            content = content.Replace("%FOLDER_PATH%", textBoxBackupPath.Text);
            content = content.Replace("%DATABASE%", textBoxBackupDb.Text);
            content = content.Replace("%FILENAME%", textBoxBackupFile.Text);

            StreamWriter writer = new StreamWriter(filePath);
            writer.Write(content);
            writer.Close();
        }

        private void createBackupBatFile(string filePath, string content)
        {
            if (textBoxBackupClient.Text.IndexOf("/") != -1)
                content = content.Replace("%CLIENT%", textBoxBackupClient.Text.Replace(@"/", @"\"));
            else
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
                //checkBoxCopyFiles.Enabled = true;
                if (checkBoxRestoreDB.Checked == true)
                    button1.Text = "Restore and upgrade to QF";
            }
            else
            {
                checkBoxRestoreDB.Enabled = false;
                //checkBoxCopyFiles.Enabled = false;
                button1.Text = "Upgrade to QF";
            }
        }

        private void checkBoxRestoreDB_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxRestoreDB.Enabled == true && radioButtonUpgradeQFdb.Checked == true && checkBoxRestoreDB.Checked == true && textBoxFrom.Text == "0")
            {
                button1.Text = "Restore and upgrade to QF";
               // checkBoxCopyFiles.Enabled = true;
            }

            else
            {
                button1.Text = "Upgrade to QF";
                //checkBoxCopyFiles.Enabled = false;
            }
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
            textBoxFrom.Text = MySettings.Default.QFfrom;
            textBoxTo.Text = MySettings.Default.QFto;
            checkBoxRestoreDB.Checked = MySettings.Default.checkBoxRestoreDB;
            //checkBoxCopyFiles.Checked = MySettings.Default.checkBoxCopyFiles;
            //checkBoxDeleteFolders.Checked = MySettings.Default.checkBoxDeleteFolder;
            checkBoxAutoSave.Checked = MySettings.Default.checkBoxAutoSave;

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
            //if (checkBoxCopyFiles.Checked == true)
            //    MySettings.Default.checkBoxCopyFiles = true;
            //if (checkBoxCopyFiles.Checked == false)
            //    MySettings.Default.checkBoxCopyFiles = false;
            //if (checkBoxDeleteFolders.Checked == true)
            //    MySettings.Default.checkBoxDeleteFolder = true;
            //if (checkBoxDeleteFolders.Checked == false)
            //    MySettings.Default.checkBoxDeleteFolder = false;
            if (checkBoxAutoSave.Checked == true)
                MySettings.Default.checkBoxAutoSave = true;
            if (checkBoxAutoSave.Checked == false)
                MySettings.Default.checkBoxAutoSave = false;

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
            MySettings.Default.QFfrom = textBoxFrom.Text;
            MySettings.Default.QFto = textBoxTo.Text;
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
            MessageBox.Show("Made by:\nPeter Gustafsson\ngustafsson.peter@gmail.com\n\nVersion : " + About);
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
            //checkBoxCopyFiles.Enabled = false;
            buttonBackupPath.Enabled = false;
            textBoxBackupClient.Enabled = false;
            textBoxBackupDb.Enabled = false;
            textBoxBackupPath.Enabled = false;
            textBoxBackupFile.Enabled = false;
            if (checkBoxRestoreDB.Enabled == true && radioButtonUpgradeQFdb.Checked == true && checkBoxRestoreDB.Checked == true)
            {
                button1.Text = "Restore and upgrade to QF";
                //checkBoxCopyFiles.Enabled = true;
            }

            else
            {
                button1.Text = "Upgrade to QF";
                //checkBoxCopyFiles.Enabled = false;
            }
           // checkBoxDeleteFolders.Enabled = false;
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
            //checkBoxCopyFiles.Enabled = true;
            buttonBackupPath.Enabled = false;
            textBoxBackupClient.Enabled = false;
            textBoxBackupDb.Enabled = false;
            textBoxBackupPath.Enabled = false;
            textBoxBackupFile.Enabled = false;
            button1.Text = "Restore from base";
            //checkBoxDeleteFolders.Enabled = false;
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
            //checkBoxCopyFiles.Enabled = true;
            buttonBackupPath.Enabled = false;
            textBoxBackupClient.Enabled = false;
            textBoxBackupDb.Enabled = false;
            textBoxBackupPath.Enabled = false;
            textBoxBackupFile.Enabled = false;
            button1.Text = "Restore/upgrade to TRUNK";
            //checkBoxDeleteFolders.Enabled = true;
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
            //checkBoxCopyFiles.Enabled = false;
            buttonBackupPath.Enabled = false;
            textBoxBackupClient.Enabled = false;
            textBoxBackupDb.Enabled = false;
            textBoxBackupPath.Enabled = false;
            textBoxBackupFile.Enabled = false;
            button1.Text = "Restore from files";
            //checkBoxDeleteFolders.Enabled = false;
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
            //checkBoxCopyFiles.Enabled = false;
            buttonBackupPath.Enabled = false;
            textBoxBackupClient.Enabled = false;
            textBoxBackupDb.Enabled = false;
            textBoxBackupPath.Enabled = false;
            textBoxBackupFile.Enabled = false;
            button1.Text = "Upgrade from path";
            //checkBoxDeleteFolders.Enabled = false;
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
            //checkBoxCopyFiles.Enabled = false;
            buttonBackupPath.Enabled = true;
            textBoxBackupClient.Enabled = true;
            textBoxBackupDb.Enabled = true;
            textBoxBackupPath.Enabled = true;
            textBoxBackupFile.Enabled = true;
            button1.Text = "Backup database";
            //checkBoxDeleteFolders.Enabled = false;
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

        //private string getSubFolderPath(string strPath, string SearchString)
        //{
        //    try
        //    {
        //        string path = "";
        //        string[] dirs = Directory.GetDirectories(strPath, SearchString);
        //        Array.Sort(dirs);
        //        Array.Reverse(dirs);
        //        foreach (string dir in dirs)
        //        {
        //            string firstNormalizedSearchString = SearchString.Replace("*", "");
        //            string secondNormalizedSearchString = dir.Replace(" ", "");
        //            if (secondNormalizedSearchString.ToLower().Contains(firstNormalizedSearchString.ToLower()))
        //                path = dir;
        //        }
        //        return path;
        //    }
        //    catch (Exception ee)
        //    {
        //        MessageBox.Show(ee.ToString());
        //        return null;
        //    }
        //}

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

        private bool isFileInUse(string path)
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

        private bool isLocalServer()
        {
            if (rbBackupDb.Checked)
            {
                if (textBoxBackupClient.Text.Contains("SDEVDBBOR"))
                    return false;
                else
                    return true;
            }
            else
            {
                if (textBoxClient.Text.Contains("SDEVDBBOR"))
                    return false;
                else
                    return true;
            }
        }

        private int countDirs(string path, string searchString)
        {
            try
            {
                string[] dirs = Directory.GetDirectories(path, searchString);
                return dirs.Length;
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
                return -1;
            }
        }

        private int countFiles(string path, string searchString)
        {
            try
            {
                string[] files = Directory.GetFiles(path, searchString);
                return files.Length;
            }
            catch
            {
                return -1;
            }
        }

        private string getVersionDot()
        {
            string strVersionDot = "";
            if (textBoxVersion.Text.Contains("."))
                strVersionDot = textBoxVersion.Text;
            if (textBoxVersion.Text.Contains(","))
                strVersionDot = textBoxVersion.Text.Replace(",", ".");
            return strVersionDot;
        }
    }
}
