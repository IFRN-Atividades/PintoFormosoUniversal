using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
            getUsuarios();
        }

        public string ip = "http://10.21.0.137";
        List<Models.Usuario> lista;


        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            verificarLogin(txtUsuarioLogin.Text, passwordBox.Password.ToString());
        }

        public async void getUsuarios()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            var response = await httpClient.GetAsync("/20131011110142/api/usuario");
            var str = response.Content.ReadAsStringAsync().Result;
            List<Models.Usuario> obj = JsonConvert.DeserializeObject<List<Models.Usuario>>(str);
            lista = obj;
        }

        public async void verificarLogin(string usuario, string s)
        {
            if (lista.Any(c => c.Nome == usuario))
            {
                Models.Usuario u = lista.Find(c => c.Nome.Contains(usuario));
                if (u.Senha == passwordBox.Password.ToString())
                {
                    this.Frame.Navigate(typeof(MainPage));
                }
                else
                {
                    var dialog = new MessageDialog("Errou a senha, parceiro(a)");
                    await dialog.ShowAsync();
                }
                
            }
            else
            {
                var dialog = new MessageDialog("Não existe esse usuário, parceiro(a)");
                await dialog.ShowAsync();
            }

        }
    }
}
