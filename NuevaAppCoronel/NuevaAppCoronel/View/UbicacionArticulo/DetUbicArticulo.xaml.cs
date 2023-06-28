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
    public partial class DetUbicArticulo : ContentPage
    {
        VMUbicacionArticulos datos = new VMUbicacionArticulos();
        string txtUbi_usualta;
        public static string ipUrl = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "url.txt");
        public static string URL = File.ReadAllText(ipUrl);
        public static string ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "terminal.txt");
        public static int name = Convert.ToInt32(File.ReadAllText(ruta));
        HttpClient cliente = new HttpClient();
        public DetUbicArticulo()
        {
            InitializeComponent();
            cargarDatos();
        }

        protected override void OnAppearing()
        {
            cargarDatos();
        }

        private void cargarDatos()
        {
            bool val = Preferences.ContainsKey("login");
            if (val)
            {
                var data = Preferences.Get("login", "");
                var deserializer = JsonConvert.DeserializeObject<MLogin>(data);
                txtUbi_usualta = Convert.ToString(deserializer?.usu_codigo);
            }

            var datosArticulo = Preferences.Get("datoUbi", "");
            var deserialize = JsonConvert.DeserializeObject<MUbicacionArticulos>(datosArticulo);
            txtCodigo.Text = deserialize.Codigo;
            txtDescri.Text = deserialize.art_descri;
            imgProducto.Source = deserialize.imagen;
            txtAdi_descri.Text = deserialize.adi_descri;
            txtCodNum.Text = Convert.ToString(deserialize.art_codnum);
            txtCodText.Text = deserialize.art_codtex;
            txtAdi_codigo.Text = deserialize.adi_codigo;
            listaUbi.ItemsSource = datos.ubicacion(txtCodText.Text, txtCodNum.Text, txtAdi_codigo.Text);

            bool keys = Preferences.ContainsKey("dato");
            if (keys)
            {
                var dato = Preferences.Get("dato", "");
                var deserializer = JsonConvert.DeserializeObject<MUbicacion>(dato);
                //txtCodtex.Text = deserializer?.ubi_codtex;
                //txtCodnum.Text = Convert.ToString(deserializer?.ubi_codnum);
                //txtUbiAdi.Text = deserializer?.ubi_adicional;
                txtubi_codigo.Text = Convert.ToString(deserializer?.ubi_codigo);
            }
        }

        private void agUbicacion_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AgregarArticulo());
        }

        private async void btnEditar_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            var h = (MUbicacion)button.BindingContext;
            await Navigation.PushAsync(new EditarUbicacion(h));
        }

        private async void btnEliminar_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            var h = (MUbicacion)button.BindingContext;

            List<MUbicacion> list = new List<MUbicacion>();
            list = datos.ubicacion(txtCodText.Text, txtCodNum.Text, txtAdi_codigo.Text);

            var accion = await DisplayActionSheet("¿Desea eliminar ubicación?", "Cancelar", null, "SI", "NO");
            if (accion == "SI")
            {
                if (list.Count > 1)
                {
                    datos.buscarVigencia(txtCodText.Text, txtCodNum.Text, txtAdi_codigo.Text);
                    datos.eliminarUbicacion(Convert.ToString(h.ubi_codigo), txtCodText.Text, txtCodNum.Text, txtAdi_codigo.Text, txtUbi_usualta, AgregarArticulo.Vigencia);
                }
                else
                {
                    datos.buscarPermisos(txtUbi_usualta);
                    datos.buscarVigencia(txtCodText.Text, txtCodNum.Text, txtAdi_codigo.Text);
                    var accion2 = await DisplayActionSheet("¿Desea deshabilitar el artículo?", "Cancelar", null, "SI", "NO");
                    if(accion2 == "SI")
                    {
                        datos.eliminarUltimaUbicacion(Convert.ToString(h.ubi_codigo), txtCodText.Text, txtCodNum.Text, txtAdi_codigo.Text, txtUbi_usualta, AgregarArticulo.Vigencia);
                    }  
                }
                OnAppearing();
            }
        }

        private async void btnImprimir_Clicked(object sender, EventArgs e)
        {
            var datados = Preferences.Get("datoUbi", "");
            var deserializedos = JsonConvert.DeserializeObject<MUbicacionArticulos>(datados);
            var descripcionArt = deserializedos?.art_descri;
            var data = Preferences.Get("login", "");
            var deserialize = JsonConvert.DeserializeObject<MLogin>(data);
            var usu_codigo = Convert.ToString(deserialize?.usu_codigo);
            string ubiadicional = "null";
            Button button = (Button)sender;
            var h = (MUbicacion)button.BindingContext;

            var accion = await DisplayActionSheet("¿Desea incluir este articulo en la impresion?", "Cancelar", null, "SI", "NO");
            if (accion == "SI")
            {
                URL = URL.Replace('\n', '/');
                if (h.ubi_adicional != "")
                {
                    ubiadicional = h.ubi_adicional;
                }
                string url = $"{URL}Etiquetas/AgregarUbicImprimir/{h.ubi_codtex}/{h.ubi_codnum}/{ubiadicional}/{name}/{usu_codigo}/{descripcionArt}";
                HttpResponseMessage request = cliente.GetAsync(url).Result;
                if (request.IsSuccessStatusCode)
                {
                    await DisplayAlert("Mensaje", "Se ha guardado correctamente", "OK");
                }
            }
        }
    }
}