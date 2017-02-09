using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
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
        public List<Models.Estado> ListaEstados = new List<Models.Estado>();

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
            ListaEstados = obj;
            populateEstado();
        }

        public async void getCidades()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            var response = await httpClient.GetAsync("/20131011110142/api/cidade");
            var str = response.Content.ReadAsStringAsync().Result;
            List<Models.Cidade> obj = JsonConvert.DeserializeObject<List<Models.Cidade>>(str);
            lstCidades.ItemsSource = obj;
        }


        private async void btnListarCidades_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            var response = await httpClient.GetAsync("/20131011110142/api/cidade");
            var str = response.Content.ReadAsStringAsync().Result;
            List<Models.Cidade> obj = JsonConvert.DeserializeObject<List<Models.Cidade>>(str);
            lstCidades.ItemsSource = obj;
        }

        private async void btnInserirVeic_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            Models.Cidade c = new Models.Cidade
            {
                Nome = txtNomeCidade.Text,
                IdEstado = (int)cmbEstado.SelectedValue
            };
            
            string s = JsonConvert.SerializeObject(c);
            var content = new StringContent(s, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/20131011110142/api/cidade", content);
            getCidades();
        }

        public void populateEstado()
        {
            cmbEstado.ItemsSource = null;
            cmbEstado.ItemsSource = ListaEstados;
            cmbEstado.SelectedValuePath = "Id";
            cmbEstado.DisplayMemberPath = "Nome";
        }
    }
}
