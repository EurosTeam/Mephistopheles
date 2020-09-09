using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Net.Security;

namespace Méphistophélès
{
    public partial class Form1 : Form
    {
        private static HttpClient client = new HttpClient();
        int mov;
        int movX;
        int movY;
        bool ssl = false, server_status_cracked = false, server_status_nulled = false, server_status_breakingIn = false, ssl2 = false;

        public void StopAuth()
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

            foreach (var process in Process.GetProcessesByName("python"))
            {
                process.Kill();
            }
        }

        public Form1()
        {
            this.Icon = Properties.Resources.icon;
            //checking for update
            var response = client.GetStringAsync("https://raw.githubusercontent.com/EurosTeam/Mephistopheles/master/version.txt");
            var version = response.Result;
            if(version != "1.8\n")
            {
                MessageBox.Show("A new update is available on github !");
            }
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
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mov = 1;
            movX = e.X;
            movY = e.Y;
        }
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if(mov == 1)
            {
                this.SetDesktopLocation(MousePosition.X - movX,MousePosition.Y - movY);
            }
        }
        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mov = 0;
        }

        private void stopbutton_cracked_Click(object sender, EventArgs e)
        {
            StopAuth();
            server_status_cracked = false;
            serverstatus.Text = "OFF";
            serverstatus.ForeColor = System.Drawing.Color.Red;
        }

        private void startnulled_button_Click(object sender, EventArgs e)
        {
            if(server_status_nulled == true)
            {
                MessageBox.Show("You didn't stop the server,please stop the server before starting other mode.");
            }

            if(server_status_nulled == false)
            {
                StreamWriter optionWriter = new StreamWriter("server\\nulled\\config.ini");

                if (secretkey_nulled.Text != null)
                {
                    optionWriter.WriteLine("[secret]");
                    optionWriter.WriteLine("secretkey=" + secretkey_nulled.Text);
                }

                optionWriter.Close();

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
                StreamWriter streamWriter = new StreamWriter("hosts.bak");

                string line = streamReader.ReadLine();

                while (line != null)
                {
                    line = streamReader.ReadLine();
                    streamWriter.WriteLine(line);
                }

                streamReader.Close();
                streamWriter.Close();

                string payload = "\x0D" + "127.0.0.1 nulledauth.net";
                //what this does ? all the traffic that will pass through nulledauth.net will actually go to our server
                File.AppendAllText("C:\\Windows\\System32\\drivers\\etc\\hosts", payload);
                //set the server status to ON
                server_status_nulled = true;
                serverstatus_nulled.Text = "ON";
                serverstatus_nulled.ForeColor = System.Drawing.Color.Lime;
            }
        }

        private void stopnulled_button_Click(object sender, EventArgs e)
        {
            StopAuth();
            server_status_nulled = false;
            serverstatus_nulled.Text = "OFF";
            serverstatus_nulled.ForeColor = System.Drawing.Color.Red;
        }

        private void ssl_check_CheckedChanged(object sender, EventArgs e)
        {
            ssl = !ssl;
        }

        private void metroLink1_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.youtube.com/watch?v=dcBDijs0Y84");
        }

        private void metroLink2_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.youtube.com/watch?v=YcO_dE0Z19g");
        }

        private void stop_breaking_Click(object sender, EventArgs e)
        {
            StopAuth();
            server_status_breakingIn = false;
            server_status_breaking.Text = "OFF";
            server_status_breaking.ForeColor = System.Drawing.Color.Red;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void create_Click(object sender, EventArgs e)
        {
            //builder
            string program_source = Properties.Resources.Source;
            program_source = program_source.Replace("#server", authurl.Text);
            string server_source = Properties.Resources.Server_Source;
            if(ssl2 == false)
                server_source = server_source.Replace("#port", "80");
            if(ssl2 == true)
                server_source = server_source.Replace("#port", "443");
            server_source = server_source.Replace("#auth", goodboymsg.Text);
            using (SaveFileDialog saveFile = new SaveFileDialog())
            {
                saveFile.Filter = "Executable (*.exe)|*.exe";
                saveFile.Title = "Auth byppass software file name";
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    new Compiler(program_source, saveFile.FileName);
                    File.WriteAllText(Path.GetDirectoryName(saveFile.FileName) + "\\server.py", server_source);
                    File.WriteAllText(Path.GetDirectoryName(saveFile.FileName) + "\\server.crt", Properties.Resources.server_crt);
                    File.WriteAllText(Path.GetDirectoryName(saveFile.FileName) + "\\server.key", Properties.Resources.server_key);
                }
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Txt Files (*.txt)|*.txt";
            openFileDialog.ShowDialog();
            string ReadFile = File.ReadAllText(openFileDialog.FileName);
            goodboymsg.Text = ReadFile;
        }

        private void ssl_builder_CheckedChanged(object sender, EventArgs e)
        {
            ssl2 = !ssl2;
        }

        private void start_breaking_Click(object sender, EventArgs e)
        {
            if(server_status_breakingIn == true)
            {
                MessageBox.Show("You didn't stop the server, please stop the server before starting other mode.");
            }

            //starting the server
            Process p = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/c \"python server\\breakingin\\server.py\"";
            p.StartInfo = startInfo;
            p.Start();

            //saving the host file to oldhost.txt for get less trouble that possible if you close without stopping the server.
            StreamReader streamReader = new StreamReader("C:\\Windows\\System32\\drivers\\etc\\hosts");
            StreamWriter streamWriter = new StreamWriter("hosts.bak");

            string line = streamReader.ReadLine();

            while (line != null)
            {
                line = streamReader.ReadLine();
                streamWriter.WriteLine(line);
            }

            streamReader.Close();
            streamWriter.Close();
            string payload = "\x0D" + "127.0.0.1 breakingin.to";
            //what this does ? all the traffic that will pass through breakingin.to will actually go to our server
            File.AppendAllText("C:\\Windows\\System32\\drivers\\etc\\hosts", payload);
            //set the server status to ON
            server_status_breakingIn = true;
            server_status_breaking.Text = "ON";
            server_status_breaking.ForeColor = System.Drawing.Color.Lime;
        }

        private void metroLabel8_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.youtube.com/channel/UCN9SbyGOmm4cj_xzykyXJPQ?sub_confirmation=1"); // if you want to support me :p
        }

        private void startbutton_cracked_Click(object sender, EventArgs e)
        {
            if(server_status_cracked == true)
            {
                MessageBox.Show("You didn't stop the server,please stop the server before starting other mode.");
            }
            if(server_status_cracked == false)
            {
                //write the username to the file
                StreamWriter streamWriter1 = new StreamWriter("server\\cracked\\crackedauth.txt");
                StreamWriter config = new StreamWriter("server\\cracked\\config.ini");

                //if cracked.to authkey textbox is not empty
                if (secretkey_box.Text != null)
                {
                    config.WriteLine("[secret]");
                    config.WriteLine("secretkey=" + secretkey_box.Text);
                }

                config.Close();

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
                StreamWriter streamWriter = new StreamWriter("hosts.bak");

                string line = streamReader.ReadLine();

                while (line != null)
                {
                    line = streamReader.ReadLine();
                    streamWriter.WriteLine(line);
                }
                streamReader.Close();
                streamWriter.Close();

                string payload = "\x0D" + "127.0.0.1 cracked.to";
                //what this does ? all the traffic that will pass through cracked.to will actually go to our server
                File.AppendAllText("C:\\Windows\\System32\\drivers\\etc\\hosts", payload);
                server_status_cracked = true;
                serverstatus.Text = "ON";
                serverstatus.ForeColor = System.Drawing.Color.Lime;
            }
        }
    }
}
