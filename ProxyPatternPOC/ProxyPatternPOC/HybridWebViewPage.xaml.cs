using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProxyPatternPOC
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HybridWebViewPage : ContentPage
    {
        public HybridWebViewPage()
        {
            InitializeComponent();
            hybridWebView.RegisterAction("callAPI", parm => FetchData(parm));
        }

        async private void FetchData(string parm)
        {

            string strURL= "http://samples.openweathermap.org/data/2.5/weather?q=London,uk&appid=b1b15e88fa797225412429c1c50c122a1";
            using (HttpClientHandler ClientHandler = new HttpClientHandler())
            using (HttpClient Client = new HttpClient(ClientHandler))
            {
                using (HttpResponseMessage ResponseMessage = await Client.GetAsync(strURL))
                {
                    if (ResponseMessage.StatusCode == HttpStatusCode.OK)
                    {
                        using (HttpContent Content = ResponseMessage.Content)
                        {
                            string result = await Content.ReadAsStringAsync();
                            hybridWebView.CallJavascript(result);
                        }
                    }
                }
            }
        }
    }
}
