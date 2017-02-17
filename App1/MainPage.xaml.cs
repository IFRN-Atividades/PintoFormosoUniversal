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
            getCidades();
            getUsuários();
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
            var result = from Models.Cidade c in obj
                         join Models.Estado e in ListaEstados
                         on c.IdEstado equals e.Id
                         select c.comEstado(e);
            lstCidades.ItemsSource = result;
        }


        private async void btnListarCidades_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            var response = await httpClient.GetAsync("/20131011110142/api/cidade");
            var str = response.Content.ReadAsStringAsync().Result;
            List<Models.Cidade> obj = JsonConvert.DeserializeObject<List<Models.Cidade>>(str);
            var result = from Models.Cidade c in obj
                         join Models.Estado k in ListaEstados
                         on c.IdEstado equals k.Id
                         select c.comEstado(k);
            lstCidades.ItemsSource = result;
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

        private int IdCidade;

        private void lstCidades_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var curItem = (Models.Cidade)lstCidades.SelectedItem;
                txtNomeCidade.Text = curItem.Nome;
                cmbEstado.SelectedValue = curItem.IdEstado;
                IdCidade = curItem.Id;
            }
            catch (Exception ex)
            {

            }
        }

        private async void btnEditarCidade_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);

            var response = await httpClient.GetAsync("/20131011110142/api/cidade");
            var str = response.Content.ReadAsStringAsync().Result;
            List<Models.Cidade> obj = JsonConvert.DeserializeObject<List<Models.Cidade>>(str);
            Models.Cidade item = (from Models.Cidade f in obj where f.Id == IdCidade select f).Single();

            item.Nome = txtNomeCidade.Text;
            item.IdEstado = (int)cmbEstado.SelectedValue;

            var content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
            await httpClient.PutAsync("/20131011110142/api/cidade/" + item.Id, content);

            IdCidade = 0;
            lstCidades.SelectedIndex = -1;
            getCidades();
        }

        private async void btnExcluirCidade_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            await httpClient.DeleteAsync("/20131011110142/api/cidade/" + IdCidade);
        }

        private async void btnListarUsuarios_Click(object sender, RoutedEventArgs e)
        {
            getUsuários();
        }

        private async void btnInserirUsuario_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            Models.Usuario u = new Models.Usuario
            {
                Nome = txtNomeUsuario.Text,
                Senha = txtSenhaUsuario.Password.ToString()
            };
            string s = JsonConvert.SerializeObject(u);
            var content = new StringContent(s, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/20131011110142/api/usuario", content);
            getUsuários();
        }

        public async void getUsuários()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            var response = await httpClient.GetAsync("/20131011110142/api/usuario");
            var str = response.Content.ReadAsStringAsync().Result;
            List<Models.Usuario> obj = JsonConvert.DeserializeObject<List<Models.Usuario>>(str);
            lstUsuarios.ItemsSource = obj;
        }

        public int IdUsuario;

        private async void btnEditarUsuario_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);

            var response = await httpClient.GetAsync("/20131011110142/api/usuario");
            var str = response.Content.ReadAsStringAsync().Result;
            List<Models.Usuario> obj = JsonConvert.DeserializeObject<List<Models.Usuario>>(str);
            Models.Usuario item = (from Models.Usuario f in obj where f.Id == IdUsuario select f).Single();

            item.Nome = txtNomeUsuario.Text;
            item.Senha = txtSenhaUsuario.Password.ToString();

            var content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
            await httpClient.PutAsync("/20131011110142/api/usuario/" + item.Id, content);

            IdUsuario = 0;
            lstUsuarios.SelectedIndex = -1;
            getUsuários();
        }

        private void lstUsuarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var curItem = (Models.Usuario)lstUsuarios.SelectedItem;
                txtNomeUsuario.Text = curItem.Nome;
                txtSenhaUsuario.Password = curItem.Senha;
                IdUsuario = curItem.Id;
            }
            catch (Exception ex)
            {

            }
        }

        private async void BtnExcluirUsuario_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            await httpClient.DeleteAsync("/20131011110142/api/usuario/" + IdUsuario);
            getUsuários();
        }
    }
}
