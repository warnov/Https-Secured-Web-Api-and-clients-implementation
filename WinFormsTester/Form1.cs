using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Windows.Forms;

namespace WinFormsTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnRequest_Click(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            WebClient client = new WebClient();
            client.UseDefaultCredentials = true;
            var user = txtUser.Text;
            var pwd = txtPwd.Text;
            var uri = "https://localhost:44329/Token";
            var baseuri = "https://localaccountstest.azurewebsites.net/";
            var parameters = $"grant_type=password&username={user}&password={pwd}";
            string result = string.Empty;


            //using (WebClient wc = new WebClient())
            //{
            //    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            //    result = wc.UploadString(uri, parameters);
            //}

 

            using (var hc = new HttpClient())
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                hc.BaseAddress = new Uri(baseuri);
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type","password"),
                    new KeyValuePair<string, string>("username",user),
                    new KeyValuePair<string, string>("password",pwd)
                });
                var oResult = hc.PostAsync("/Token", content).Result;
                result = oResult.Content.ReadAsStringAsync().Result;                
            }


            rtbResponse.Text = result;
        }
    }
}
