using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows.Forms;

namespace frontend
{
    public partial class FormFindDesk : Form
    {
        public FormFindDesk()
        {
            this.DoubleBuffered = true;//设置本窗体
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            InitializeComponent();
        }

        private async void EnterDesk(int desk, int seat)
        {
            var httpClient = new HttpClient();
            var url = Url.Header + Url.EnterDesk;
            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("uid", Global.uid),
                new KeyValuePair<string, string>("password", Global.password),
                new KeyValuePair<string, string>("desk", Convert.ToString(desk)),
                new KeyValuePair<string, string>("seat", Convert.ToString(seat)),
                new KeyValuePair<string, string>("attribute", "sitdown")
            };
            var response = await httpClient.PostAsync(new Uri(url), new FormUrlEncodedContent(parameters));
            var result = await response.Content.ReadAsStringAsync();
            if (!result.Equals("OK")) return;
            Global.seat = seat;
            var formDesk = new FormDesk();
            formDesk.Show();
            Dispose();
        }

        private async void FormFindDesk_Load(object sender, EventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            string url = Url.Header + Url.GetDeskListUrl;
            var response = await httpClient.PostAsync(new Uri(url), new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()));
            var result = await response.Content.ReadAsStringAsync();
            string[] desks = result.Split("\n");
            foreach (var desk in desks)
            {
                var info = desk.Split(" ");
                if (info.Length < 3)
                {
                    break;
                }
                if (info[1].Equals(Global.uid) || info[2].Equals(Global.uid))
                {
                    FormDesk formDesk = new FormDesk();
                    formDesk.Show();
                    Dispose();
                    Global.seat = info[1].Equals(Global.uid) ? 1 : 2;
                    return;
                }
                switch (info[0])
                {
                    case "1":
                        if (info[1] != "0")
                        {
                            button2.Text = info[1];
                            button2.Enabled = false;
                        }

                        if (info[2] != "0")
                        {
                            button3.Text = info[2];
                            button3.Enabled = false;
                        }

                        break;
                    case "2":
                        if (info[1] != "0")
                        {
                            button5.Text = info[1];
                            button5.Enabled = false;
                        }

                        if (info[2] != "0")
                        {
                            button6.Text = info[2];
                            button6.Enabled = false;
                        }
                        break;
                    case "3":
                        if (info[1] != "0")
                        {
                            button8.Text = info[1];
                            button8.Enabled = false;
                        }

                        if (info[2] != "0")
                        {
                            button9.Text = info[2];
                            button9.Enabled = false;
                        }

                        break;
                    case "4":
                        if (info[1] != "0")
                        {
                            button11.Text = info[1];
                            button11.Enabled = false;
                        }

                        if (info[2] != "0")
                        {
                            button12.Text = info[2];
                            button12.Enabled = false;
                        }

                        break;
                    default:
                        break;
                }
            }

            httpClient.Dispose();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (button2.Enabled)
            {
                EnterDesk(1, 1);
                return;
            }
            if (button3.Enabled)
            {
                EnterDesk(1, 2);
                return;
            }
            MessageBox.Show("桌子已满");
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            EnterDesk(1, 2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EnterDesk(1, 1);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            EnterDesk(2, 2);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            EnterDesk(2, 1);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            EnterDesk(3, 2);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            EnterDesk(3, 1);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            EnterDesk(4, 2);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            EnterDesk(4, 1);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            string url = Url.Header + Url.GetDeskListUrl;
            var response = httpClient.PostAsync(new Uri(url), new FormUrlEncodedContent(new List<KeyValuePair<string, string>>())).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            string[] Desks = result.Split("\n");
            foreach (var desk in Desks)
            {
                var info = desk.Split(" ");
                if (info.Length < 3)
                {
                    break;
                }

                switch (info[0])
                {
                    case "1":
                        if (info[1] != "0")
                        {
                            button2.Text = info[1];
                            button2.Enabled = false;
                        }
                        else
                        {
                            button2.Text = "";
                            button2.Enabled = true;
                        }

                        if (info[2] != "0")
                        {
                            button3.Text = info[2];
                            button3.Enabled = false;
                        }
                        else
                        {
                            button3.Text = "";
                            button3.Enabled = true;
                        }
                        break;
                    case "2":
                        if (info[1] != "0")
                        {
                            button5.Text = info[1];
                            button5.Enabled = false;
                        }
                        else
                        {
                            button5.Text = "";
                            button5.Enabled = true;
                        }
                        if (info[2] != "0")
                        {
                            button6.Text = info[2];
                            button6.Enabled = false;
                        }
                        else
                        {
                            button6.Text = "";
                            button6.Enabled = true;
                        }
                        break;
                    case "3":
                        if (info[1] != "0")
                        {
                            button8.Text = info[1];
                            button8.Enabled = false;
                        }
                        else
                        {
                            button8.Text = "";
                            button8.Enabled = true;
                        }

                        if (info[2] != "0")
                        {
                            button9.Text = info[2];
                            button9.Enabled = false;
                        }
                        else
                        {
                            button9.Text = "";
                            button9.Enabled = true;
                        }

                        break;
                    case "4":
                        if (info[1] != "0")
                        {
                            button11.Text = info[1];
                            button11.Enabled = false;
                        }
                        else
                        {
                            button11.Text = "";
                            button11.Enabled = true;
                        }

                        if (info[2] != "0")
                        {
                            button12.Text = info[2];
                            button12.Enabled = false;
                        }
                        else
                        {
                            button12.Text = "";
                            button12.Enabled = true;
                        }

                        break;
                    default:
                        break;
                }
            }
        }

        private void FormFindDesk_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (button8.Enabled)
            {
                EnterDesk(3, 1);
                return;
            }
            if (button9.Enabled)
            {
                EnterDesk(3, 2);
                return;
            }
            MessageBox.Show("桌子已满");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (button5.Enabled)
            {
                EnterDesk(2, 1);
                return;
            }
            if (button6.Enabled)
            {
                EnterDesk(2, 2);
                return;
            }
            MessageBox.Show("桌子已满");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (button11.Enabled)
            {
                EnterDesk(4, 1);
                return;
            }
            if (button12.Enabled)
            {
                EnterDesk(4, 2);
                return;
            }
            MessageBox.Show("桌子已满");
        }
    }
}