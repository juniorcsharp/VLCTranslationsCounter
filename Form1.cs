﻿using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace VLCTranslationsCounter
{
    public partial class Form1 : Form
    {
        //Address
       private static string address = "192.168.1.85:8003";    //any address of your translation

        //string address = "www.google.com";  //for testing

        public Form1()
        {
            InitializeComponent();
        }

        public void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            GetInfoFromCmd();
        }

        //For start cmd
        public void GetInfoFromCmd()
        {
            try
            {
                //Creating cmd process
                ProcessStartInfo proc = new ProcessStartInfo();
                //proc.Verb = "runas";
                proc.FileName = @"cmd.exe";
                proc.RedirectStandardOutput = true;
                //proc.Arguments = @"/c ping " + address; //for testing
                proc.Arguments = @"/c netstat -a | findstr " + address;

                //Hide window
                proc.WindowStyle = ProcessWindowStyle.Hidden;
                proc.UseShellExecute = false;
                proc.CreateNoWindow = true;

                //Starting this process
                Process cmd = Process.Start(proc);

                //Putting the result of cmd command to tempResult
                StreamReader reader = cmd.StandardOutput;
                string tempResult = reader.ReadToEnd();   //puttig all in temp
                string[] words = tempResult.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                int counter = 0;
                //string tcp = "Reply";   //for testing
                string tcp = "TCP";
                foreach (string temp in words)   //seardhing for TCP user who is connected
                {
                    if (temp.Contains(tcp))
                    {
                        counter++;
                    }
                }
                String count = counter.ToString();

                // Making notifyIcon1
                IconMaker maker = new IconMaker();
                this.notifyIcon1.Icon = maker.MakeIcon(count);

                //Closing reader
                reader.Close();

                //Closing cmd process
                cmd.WaitForExit();
            } 

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());   //if any exception acquires
                Application.Exit();
            } 
        }
          
        private void Form1_Load(object sender, EventArgs e)
        {
            Hide();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
                Hide();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void RestoreContextMenuItem_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void CloseApplicationContextMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
            notifyIcon1.Visible = false;
        }

        private void IPAddressTextBox_TextChanged(object sender, EventArgs e)
        {
            address = IPAddressTextBox.Text;
        }
    }

}
