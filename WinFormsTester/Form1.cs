using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace WinFormsTester
{
    public partial class Form1 : Form
    {
        string _token = string.Empty;
        string baseuri = "https://localaccountstest.azurewebsites.net/";

        public Form1()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var user = txtUser.Text;
            var pwd = txtPwd.Text;

            using (var hc = new HttpClient())
            {
                hc.BaseAddress = new Uri(baseuri);
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type","password"),
                    new KeyValuePair<string, string>("username",user),
                    new KeyValuePair<string, string>("password",pwd)
                });
                var oResult = hc.PostAsync("/Token", content).Result;
                var sResult = oResult.Content.ReadAsStringAsync().Result;
                rtbResponse.Text = sResult;
                if (sResult.Contains("access_token"))
                {
                    var jResult = JObject.Parse(sResult);
                    _token = jResult["access_token"].ToString();
                    rtbResponse.Text = _token;
                }
            }
        }

        private void btnRequest_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_token))
            {
                using (var hc = new HttpClient())
                {
                    hc.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                    hc.BaseAddress = new Uri(baseuri);
                    var sResult = hc.GetStringAsync("/api/values").Result;
                    rtbResponse.Text = sResult;
                }
            }
            else MessageBox.Show("User is not logged in");
        }
    }
}
