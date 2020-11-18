using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows.Forms;

namespace frontend
{
    public partial class FormLogin : Form
    {

        public FormLogin()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label_password_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void button_register_Click(object sender, EventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            string url = Url.Header + Url.RegisterUrl;
            string username = textBox_uid.Text;
            string password = MD5.GetMD5Hash(textBox_password.Text);
            List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("uid", username),
                new KeyValuePair<string, string>("password", password)
            };
            var response = httpClient.PostAsync(new Uri(url), new FormUrlEncodedContent(paramList)).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            if (result.Equals("OK"))
            {
                Global.uid = textBox_uid.Text;
                Global.password = password;
                Hide();
                var formFindDesk = new FormFindDesk();
                formFindDesk.Show();
            }
            else
            {
                MessageBox.Show("注册失败");
            }
            httpClient.Dispose();
        }
        private void button_submit_Click(object sender, EventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            string url = Url.Header + Url.LoginUrl;
            string username = textBox_uid.Text;
            string password = MD5.GetMD5Hash(textBox_password.Text);
            List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("uid", username),
                new KeyValuePair<string, string>("password", password)
            };

            var response = httpClient.PostAsync(new Uri(url), new FormUrlEncodedContent(paramList)).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            if (result.Equals("OK"))
            {
                Global.uid = textBox_uid.Text;
                Global.password = password;
                Hide();
                var formFindDesk = new FormFindDesk();

                formFindDesk.Show();
            }
            else
            {
                MessageBox.Show("登录遇到了问题，请检查账号或密码");
            }
            httpClient.Dispose();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {

        }

        private void FormLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.Text)
            {
                case "上海 阿里云":
                    Url.Header = "https://csharp.huhaorui.com";
                    break;
                case "杭州 电信":
                    Url.Header = "https://csharp.nas.huhaorui.com:8888";
                    break;
                case "localhost":
                    Url.Header = "http://localhost:5000";
                    break;       
            }
        }
    }
}