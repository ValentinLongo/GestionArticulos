using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NuevaAppCoronel
{
    public partial class MainPage : ContentPage
    {
        public static string URL;
        HttpClient cliente = new HttpClient();
        public MainPage()
        {
            InitializeComponent();
        }

        string nameBD;
        string ip;
        string term;
        string ruta;
        int IdUsuario;
        string url;
        //"Data Source = 192.168.1.210\\MASER_INF;Initial Catalog =MAURO; User ID = sa; Password=1220; MultipleActiveResultSets=True";
        private async void btnConectar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txturl.Text) || !string.IsNullOrEmpty(terminal.Text))
                {
                    terminalDispositivo();
                    IpServidor();
                    ComprobarTerminal();
                }
                else
                {
                    await DisplayAlert("Ingrese la Conexion", "Se requieren Datos", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Mensaje", $"{ex}", "Ok");
            }
        }

        public async void ComprobarTerminal()
        {
            try
            {
                var numSerie = DeviceInfo.Name;
                URL = URL.Replace('\n', '/');
                string url = "" + URL + "/Terminal/" + terminal.Text + "/" + (numSerie) + "";
                HttpResponseMessage response = cliente.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Mensaje", "Conexion Lista, vuelva a ingresar a la app", "Ok");
                    Process.GetCurrentProcess().CloseMainWindow();
                }
                else
                {
                    await DisplayAlert("Advertencia", "Debe poner otro numero de terminal", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Mensaje", $"{ex}", "Ok");
            }
        }


        private void IpServidor()
        {
            ip = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "url.txt");
            //URL = File.ReadAllText(ip);
            //FileInfo file = new FileInfo(ip);
            StreamWriter sw;
            string url = "http://" + txturl.Text + "";
            URL = url;
            try
            {
                if (File.Exists(ip) == false)
                {
                    sw = File.CreateText(ip);
                    sw.WriteLine(url);
                    sw.Flush(); //Guardo el archivo
                    sw.Close(); //Cierro el archivo
                }
                else if (File.Exists(ip) == true)
                {
                    File.Delete(ip); //Elimino el archivo
                    sw = File.CreateText(ip);
                    sw.WriteLine(url);
                    sw.Flush(); //Guardo el archivo
                    sw.Close(); //Cierro el archivo
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Mensaje", "" + ex.Message, "OK");
            }
        }

        private void terminalDispositivo()
        {
            term = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "terminal.txt");
            StreamWriter sw;
            try
            {
                if (File.Exists(term) == false)
                {
                    sw = File.CreateText(term);
                    sw.WriteLine(terminal.Text);
                    sw.Flush(); //Guardo el archivo
                    sw.Close(); //Cierro el archivo
                }
                else if (File.Exists(term) == true)
                {
                    File.Delete(term); //Elimino el archivo
                    sw = File.CreateText(term);
                    sw.WriteLine(terminal.Text);
                    sw.Flush(); //Guardo el archivo
                    sw.Close(); //Cierro el archivo
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Mensaje", "" + ex.Message, "OK");
            }
        }
    }
}

