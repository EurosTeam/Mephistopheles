using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace Méphistophélès
{
    public partial class Form1 : Form
    {
        int mov;
        int movX;
        int movY;
        bool nulledauthmode = false,forlaxmode = false;
        public Form1()
        {
            InitializeComponent();
        }

        //Quit Button
        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //this is for moving the form with form border style to none
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Location = Screen.AllScreens[0].WorkingArea.Location;
        }

        //this is for moving the form with form border style to none
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mov = 1;
            movX = e.X;
            movY = e.Y;
        }

        //this is for moving the form with form border style to none
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if(mov == 1)
            {
                this.SetDesktopLocation(MousePosition.X - movX,MousePosition.Y - movY);
            }
        }
        //this is for moving the form with form border style to none
        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mov = 0;
        }

        //cracked.to auth bypass start button
        private void button2_Click(object sender, EventArgs e)
        {
            //write the username to the file
            StreamWriter streamWriter1 = new StreamWriter("server\\cracked\\crackedauth.txt");
            StreamWriter streamWriter2 = new StreamWriter("server\\cracked\\secret.txt");
            StreamWriter config = new StreamWriter("server\\cracked\\config.ini");

            if(textBox5.Text != null)
            {
                streamWriter2.WriteLine(textBox5.Text);
            }

            if(forlaxmode == true)
            {
                config.WriteLine("[options]");
                config.WriteLine("forlaxmode=True");
            }
            else
            {
                config.WriteLine("[options]");
                config.WriteLine("forlaxmode=False");
            }

            config.Close();
            streamWriter1.WriteLine("{\"auth\":true,\"username\":\""+textBox2.Text+ "\",\"group\":\"100\"");

            streamWriter1.Close();
            streamWriter2.Close();

            //starting the server
            Process p = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/c \"python server\\cracked\\server.py\"";
            p.StartInfo = startInfo;
            p.Start();

            //saving the host file to oldhost.txt for get less trouble that possible if you close without stoping the server.
            StreamReader streamReader = new StreamReader("C:\\Windows\\System32\\drivers\\etc\\hosts");
            StreamWriter streamWriter = new StreamWriter("oldhost.txt");

            string line = streamReader.ReadLine();

            while(line != null)
            {
                line = streamReader.ReadLine();
                streamWriter.WriteLine(line);
            }
            streamReader.Close();
            streamWriter.Close();

            string payload = "\x0D" + textBox1.Text+ " cracked.to";
            //what this does ? all the traffic that will pass through cracked.to will actually go to our server
            File.AppendAllText("C:\\Windows\\System32\\drivers\\etc\\hosts", payload);
            label15.ForeColor = System.Drawing.Color.LimeGreen;
            label15.Text = "ON";
        }
        //cracked.to auth bypass stop button
        private void button3_Click(object sender, EventArgs e)
        {
            //write the old host file.
            StreamReader streamReader = new StreamReader("oldhost.txt");
            StreamWriter writer = new StreamWriter("C:\\Windows\\System32\\drivers\\etc\\hosts");

            string line = streamReader.ReadLine();

            while(line != null)
            {
                line = streamReader.ReadLine();
                writer.WriteLine(line);
            }
            writer.Close();
            streamReader.Close();

            foreach(var process in Process.GetProcessesByName("python"))
            {
                process.Kill();
            }
            label15.ForeColor = System.Drawing.Color.DarkRed;
            label15.Text = "OFF";
            //set the server status to OFF
        }

        //Nulled Auth Mode checkbox
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            nulledauthmode = !nulledauthmode;
        }

        //nulled.to auth bypass start button
        private void button4_Click(object sender, EventArgs e)
        {
            StreamWriter optionWriter = new StreamWriter("server\\nulled\\config.ini");
            StreamWriter secretKeyWriter = new StreamWriter("server\\nulled\\secret.txt");
            //if secret key textbox is not null write the secret key to secret.txt
            if(textBox4.Text != null)
            {
                secretKeyWriter.WriteLine(textBox4.Text);
            }

            if(nulledauthmode == true)
            {
                optionWriter.WriteLine("[options]");
                optionWriter.WriteLine("nulledauthmode=True");
            }
            else
            {
                optionWriter.WriteLine("[options]");
                optionWriter.WriteLine("nulledauthmode=False");
            }

            optionWriter.Close();
            secretKeyWriter.Close();

            //starting the server
            Process p = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/c \"python server\\nulled\\server.py\"";
            p.StartInfo = startInfo;
            p.Start();

            //saving the host file to oldhost.txt for get less trouble that possible if you close without stopping the server.
            StreamReader streamReader = new StreamReader("C:\\Windows\\System32\\drivers\\etc\\hosts");
            StreamWriter streamWriter = new StreamWriter("oldhost.txt");

            string line = streamReader.ReadLine();

            while (line != null)
            {
                line = streamReader.ReadLine();
                streamWriter.WriteLine(line);
            }

            streamReader.Close();
            streamWriter.Close();

            if (nulledauthmode == false)
            {
                string payload = "\x0D" + textBox3.Text + " www.nulled.to";
                //what this does ? all the traffic that will pass through nulled.to will actually go to our server
                File.AppendAllText("C:\\Windows\\System32\\drivers\\etc\\hosts", payload);
                //set the server status to ON
                label13.ForeColor = System.Drawing.Color.LimeGreen;
                label13.Text = "ON";
            }
            else
            {
                string payload = "\x0D" + textBox3.Text + " nulledauth.net";
                //what this does ? all the traffic that will pass through nulledauth.net will actually go to our server
                File.AppendAllText("C:\\Windows\\System32\\drivers\\etc\\hosts", payload);
                //set the server status to ON
                label13.ForeColor = System.Drawing.Color.LimeGreen;
                label13.Text = "ON";
            }
        }

        //nulled.to auth bypass stop button
        private void button5_Click(object sender, EventArgs e)
        {
            //write the old host file.
            StreamReader streamReader = new StreamReader("oldhost.txt");
            StreamWriter writer = new StreamWriter("C:\\Windows\\System32\\drivers\\etc\\hosts");
            string line = streamReader.ReadLine();

            while (line != null)
            {
                line = streamReader.ReadLine();
                writer.WriteLine(line);
            }

            writer.Close();
            streamReader.Close();

            //The program search through all process the python process once he found it he kill it
            foreach (var process in Process.GetProcessesByName("python"))
            {
                process.Kill();
            }
            //set the server status to OFF
            label13.ForeColor = System.Drawing.Color.DarkRed;
            label13.Text = "OFF";
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://cainofthebible.github.io/mephistopheles/");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://cainofthebible.github.io/mephistopheles/");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/channel/UCN9SbyGOmm4cj_xzykyXJPQ?sub_confirmation=1"); // if you want to support me :p
        }

        private void button6_Click(object sender, EventArgs e)
        {
            StreamWriter custom = new StreamWriter("server\\custom\\custom.txt");
            StreamWriter config = new StreamWriter("server\\custom\\config.ini");
            custom.WriteLine(textBox7.Text);

            if(textBox9.Text == "")
            {
                config.WriteLine("[options]");
                config.WriteLine("port=80");
            }
            else
            {
                config.WriteLine("[options]");
                config.WriteLine("port="+textBox9.Text);
            }

            custom.Close();
            config.Close();

            //starting the server
            Process p = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/c \"python server\\custom\\server.py\"";
            p.StartInfo = startInfo;
            p.Start();

            //saving the host file to oldhost.txt for get less trouble that possible if you close without stopping the server.
            StreamReader streamReader = new StreamReader("C:\\Windows\\System32\\drivers\\etc\\hosts");
            StreamWriter streamWriter = new StreamWriter("oldhost.txt");

            string line = streamReader.ReadLine();

            while (line != null)
            {
                line = streamReader.ReadLine();
                streamWriter.WriteLine(line);
            }

            streamReader.Close();
            streamWriter.Close();
            string payload = "\x0D" + textBox6.Text + " "+textBox8.Text;
            //what this does ? all the traffic that will pass through nulled.to will actually go to our server
            File.AppendAllText("C:\\Windows\\System32\\drivers\\etc\\hosts", payload);
            //set the server status to ON
            label10.ForeColor = System.Drawing.Color.LimeGreen;
            label10.Text = "ON";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            StreamReader streamReader = new StreamReader("oldhost.txt");
            StreamWriter writer = new StreamWriter("C:\\Windows\\System32\\drivers\\etc\\hosts");
            string line = streamReader.ReadLine();

            while (line != null)
            {
                line = streamReader.ReadLine();
                writer.WriteLine(line);
            }

            writer.Close();
            streamReader.Close();

            //The program search through all process the python process once he found it he kill it
            foreach (var process in Process.GetProcessesByName("python"))
            {
                process.Kill();
            }
            //set the server status to OFF
            label10.ForeColor = System.Drawing.Color.DarkRed;
            label10.Text = "OFF";
        }

        //forlaxmode checkbox
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            forlaxmode = !forlaxmode;
        }
    }
}
