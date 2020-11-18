using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Windows.Forms;

namespace frontend
{
    public partial class FormDesk : Form
    {

        public FormDesk()
        {
            this.DoubleBuffered = true;//设置本窗体
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            HttpClient httpClient = new HttpClient();
            string url = Url.Header + Url.status;
            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("uid", Global.uid),
                new KeyValuePair<string, string>("password", Global.password),
                new KeyValuePair<string, string>("seat",Global.seat.ToString())
            };
            var response = await httpClient.PostAsync(new Uri(url), new FormUrlEncodedContent(parameters));
            var result = await response.Content.ReadAsStringAsync();
            if (result.Equals("NG"))
            {
                return;
            }

            label_desknumber.Text = "第" + result.Split(":")[0] + "桌";
            ShowStatus(result.Split(":")[1]);
            Judge(result.Split(":")[2]);
            Ready(result.Split(":")[3]);
            if (Win(result.Split(":")[1]) == Global.seat)
            {
                MessageBox.Show("你赢了");
                Global.GameBegin = false;
                button_ready.Visible = true;
            }
            else if (Win(result.Split(":")[1]) == 3 - Global.seat)
            {
                MessageBox.Show("你输了");
                Global.GameBegin = false;
                button_ready.Visible = true;
            }
            else if (Win(result.Split(":")[1]) == 3)
            {
                MessageBox.Show("平局");
                Global.GameBegin = false;
                button_ready.Visible = true;
            }
            label_myname.Text = "▲  " + result.Split(":")[4];
            label_othername.Text = "☻  " + result.Split(":")[5];
            if (label_myname.Text != "▲  ")
            {
                label_myscore.Text = result.Split(":")[6];
                pictureBox1.Visible = true;
            }
            else
            {
                label_myscore.Text = "";
                pictureBox1.Visible = false;
            }


            if (label_othername.Text != "☻  ")
            {
                label_otherscore.Text = result.Split(":")[7];
                pictureBox2.Visible = true;
            }
            else
            {
                label_otherscore.Text = "";
                pictureBox2.Visible = false;
            }
            timer1.Start();
        }

        private void SetStatus(Button button, char status)
        {
            button.Font = new Font("等线", 20, FontStyle.Bold);

            if (status != '0')
            {
                button.Text = status == '1' ? "▲" : "☻";
                button.Enabled = false;
            }
            else
            {
                button.Enabled = true;
                button.Text = "";
            }
        }

        private bool WinForSomeOne(string status, char number)
        {
            for (var i = 0; i < 3; i++)
            {
                if (status[i] == number && status[i + 3] == number && status[i + 6] == number)
                {
                    return true;
                }
            }

            for (var i = 0; i < 3; i++)
            {
                if (status[3 * i] == number && status[3 * i + 1] == number && status[3 * i + 2] == number)
                {
                    return true;
                }
            }

            if (status[0] == number && status[4] == number && status[8] == number)
            {
                return true;
            }

            if (status[2] == number && status[4] == number && status[6] == number)
            {
                return true;
            }

            return false;
        }

        private int Win(string status)
        {
            if (WinForSomeOne(status, '1'))
            {
                return 1;
            }
            else if (WinForSomeOne(status, '2'))
            {
                return 2;
            }
            else if (status.IndexOf("0", StringComparison.Ordinal) == -1)
            {
                return 3;
            }
            else
            {
                return 0;
            }
        }

        private void ShowStatus(string status)
        {
            SetStatus(button1, status[0]);
            SetStatus(button2, status[1]);
            SetStatus(button3, status[2]);
            SetStatus(button4, status[3]);
            SetStatus(button5, status[4]);
            SetStatus(button6, status[5]);
            SetStatus(button7, status[6]);
            SetStatus(button8, status[7]);
            SetStatus(button9, status[8]);
        }

        private void Judge(string status)
        {
            if (int.Parse(status) != Global.seat)
            {
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                button7.Enabled = false;
                button8.Enabled = false;
                button9.Enabled = false;
            }
        }

        private void Ready(string status)
        {
            if (status[Global.seat - 1] == '0')
            {
                button_standup.Enabled = true;
                button_ready.Text = "准备";
            }
            else
            {
                button_standup.Enabled = false;
                button_ready.Text = "取消";
            }

            if (!status.Equals("11"))
            {
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                button7.Enabled = false;
                button8.Enabled = false;
                button9.Enabled = false;
            }
            else if (!Global.GameBegin)
            {
                Global.GameBegin = true;
                MessageBox.Show("游戏开始");
                button_ready.Visible = false;
            }
        }

        private async void FormDesk_Load(object sender, EventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            string url = Url.Header + Url.status;
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("uid", Global.uid));
            parameters.Add(new KeyValuePair<string, string>("password", Global.password));
            var response = await httpClient.PostAsync(new Uri(url), new FormUrlEncodedContent(parameters));
            var result = await response.Content.ReadAsStringAsync();
            if (result.Equals("NG"))
            {
                return;
            }

            label_desknumber.Text = "第" + result.Split(":")[0] + "桌";
            ShowStatus(result.Split(":")[1]);
            Judge(result.Split(":")[2]);
            Ready(result.Split(":")[3]);
            label_myname.Text = "▲  " + result.Split(":")[4];
            label_othername.Text = "☻  " + result.Split(":")[5];
            if (label_myname.Text != "▲  ")
            {
                label_myscore.Text = result.Split(":")[6];
                pictureBox1.Visible = true;
            }
            else
            {
                label_myscore.Text = "";
                pictureBox1.Visible = false;
            }


            if (label_othername.Text != "☻  ")
            {
                label_otherscore.Text = result.Split(":")[7];
                pictureBox2.Visible = true;
            }
            else
            {
                label_otherscore.Text = "";
                pictureBox2.Visible = false;
            }
            timer1.Interval = 300;
        }

        private async void button_ready_Click(object sender, EventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            string url = Url.Header + Url.ready;
            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("uid", Global.uid),
                new KeyValuePair<string, string>("password", Global.password),
                new KeyValuePair<string, string>("attribute", button_ready.Text == "准备" ? "ready" : "unready")
            };
            var response = await httpClient.PostAsync(new Uri(url), new FormUrlEncodedContent(parameters));
            var result = await response.Content.ReadAsStringAsync();
            button_ready.Text = button_ready.Text == "准备" ? "取消" : "准备";
        }

        private async void Press(int key)
        {
            HttpClient httpClient = new HttpClient();
            string url = Url.Header + Url.press;
            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("uid", Global.uid),
                new KeyValuePair<string, string>("password", Global.password),
                new KeyValuePair<string, string>("key", key.ToString())
            };
            var response = await httpClient.PostAsync(new Uri(url), new FormUrlEncodedContent(parameters));
            var result = await response.Content.ReadAsStringAsync();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            Press(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            Press(1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            Press(2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button4.Enabled = false;
            Press(3);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button5.Enabled = false;
            Press(4);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button6.Enabled = false;
            Press(5);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button7.Enabled = false;
            Press(6);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            button8.Enabled = false;
            Press(7);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            button9.Enabled = false;
            Press(8);
        }

        private async void button_standup_Click(object sender, EventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            string url = Url.Header + Url.EnterDesk;
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("uid", Global.uid));
            parameters.Add(new KeyValuePair<string, string>("password", Global.password));
            parameters.Add(new KeyValuePair<string, string>("seat", Global.seat.ToString()));

            parameters.Add(new KeyValuePair<string, string>("attribute", "standup"));

            var response = await httpClient.PostAsync(new Uri(url), new FormUrlEncodedContent(parameters));
            var result = await response.Content.ReadAsStringAsync();
            if (result.Equals("OK"))
            {
                FormFindDesk formDesk = new FormFindDesk();
                formDesk.Show();
                Dispose();
            }
        }


        private void FormDesk_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void label_desknumber_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}