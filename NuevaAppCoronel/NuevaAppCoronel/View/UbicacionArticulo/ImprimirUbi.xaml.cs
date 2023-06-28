using Newtonsoft.Json;
using NuevaAppCoronel.Model;
using NuevaAppCoronel.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NuevaAppCoronel.View.UbicacionArticulo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImprimirUbi : ContentPage
    {
        VMUbicacionArticulos vm = new VMUbicacionArticulos();
        public static string ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "terminal.txt");
        public static int name = Convert.ToInt32(File.ReadAllText(ruta));
        public static string ipUrl = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "url.txt");
        public static string URL = File.ReadAllText(ipUrl);
        HttpClient cliente = new HttpClient();
        public ImprimirUbi()
        {
            InitializeComponent();
            llenarLista();
        }

        private void llenarLista()
        {
            ListaEtiqueta.ItemsSource = vm.listaUbicacionesImprimir(name);
        }

        protected override void OnAppearing()
        {
            llenarLista();
        }

        private async void Eliminar_Clicked(object sender, EventArgs e)
        {
            var data = Preferences.Get("login", "");
            var deserialize = JsonConvert.DeserializeObject<MLogin>(data);
            var usu_codigo = Convert.ToString(deserialize?.usu_codigo);
            Button button = (Button)sender;
            var h = (MEtiqueta)button.BindingContext;
            string impadicional = "null";
            var accion = await DisplayActionSheet("¿Desea eliminar este articulo de la impresion?", "Cancelar", null, "SI", "NO");
            if (accion == "SI")
            {
                URL = URL.Replace('\n', '/');
                if (h.imp_adicional != "")
                {
                    impadicional = h.imp_adicional;
                }
                string url = $"{URL}Etiquetas/EliminarUbicImprimir/{h.imp_codtex}/{h.imp_codnum}/{impadicional}/{name}";
                HttpResponseMessage request = cliente.GetAsync(url).Result;
                if (request.IsSuccessStatusCode)
                {
                    await DisplayAlert("Mensaje", "Se ha eliminado correctamente", "OK");
                    OnAppearing();
                }
            }
        }

        private void btnContinuar_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void btnFinalizar_Clicked(object sender, EventArgs e)
        {
            var data = Preferences.Get("login", "");
            var deserialize = JsonConvert.DeserializeObject<MLogin>(data);
            var usu_codigo = Convert.ToString(deserialize?.usu_codigo);
            var comentario = await DisplayPromptAsync("Mensaje", "Escriba un comentario!", "OK", "Cancelar", "Comentario");
            if (comentario == "")
            {
                await DisplayAlert("Advertencia", "Escriba un comentario.", "OK");
            }
            else
            {
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Etiquetas/FinalizarUbicImprimir/{comentario}/{name}/{usu_codigo}";
            HttpResponseMessage request = cliente.GetAsync(url).Result;
                if (request.IsSuccessStatusCode)
                {
                    await DisplayAlert("Mensaje", "Se finalizó con éxito", "OK");
                    await Navigation.PopAsync();
                }
            }
        }
    }
}