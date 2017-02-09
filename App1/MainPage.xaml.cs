using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public string ip = "http://10.21.0.137";
        public MainPage()
        {
            this.InitializeComponent();
            getEstados();
        }

        private async void getEstados()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            var response = await httpClient.GetAsync("/20131011110142/api/estado");
            var str = response.Content.ReadAsStringAsync().Result;
            List<Models.Estado> obj = JsonConvert.DeserializeObject<List<Models.Estado>>(str);
            lstEstados.ItemsSource = obj;
        }


        private void btnListarCidades_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnInserirVeic_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
