using Newtonsoft.Json;
using NuevaAppCoronel.Model;
using NuevaAppCoronel.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NuevaAppCoronel.View.ReposicionMercaderia
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaDetArtNuevo : ContentPage
    {
        VMReposiciones vm = new VMReposiciones();
        VMEnCurso curso = new VMEnCurso();
        public static string term = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "terminal.txt");
        public static int terminal = Convert.ToInt32(File.ReadAllText(term));
        public ListaDetArtNuevo()
        {
            InitializeComponent();
            inicializarVista();
        }

        private void inicializarVista()
        {
            ListaStock.ItemsSource = vm.listaDetalleNuevo();
        }

        private async void btnEliminarStock_Clicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            var item = (MDetalleLista)btn.BindingContext;
            var usuario = Preferences.Get("login", "");
            var deserialize = JsonConvert.DeserializeObject<MLogin>(usuario);
            string usucodigo = deserialize.usu_codigo.ToString();
            var accion = await DisplayActionSheet("¿Desea eliminar el articulo?", "Cancelar", null, "SI", "NO");
            if (accion == "SI")
            {
                string adicional = "null";
                if(item.aux_adicional != null && item.aux_adicional != "")
                {
                    adicional = item.aux_adicional;
                }
                curso.EliminarArticuloLista(usucodigo,terminal.ToString(),item.aux_codtex,item.aux_codnum.ToString(),adicional);
                await DisplayAlert("Mensaje", "Artículo eliminado", "OK");
                OnAppearing();
            }
        }

        protected override void OnAppearing()
        {
            inicializarVista();
        }

        private void btnAceptar_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void btnFinalizar_Clicked(object sender, EventArgs e)
        {
            var usuario = Preferences.Get("login", "");
            var deserialize = JsonConvert.DeserializeObject<MLogin>(usuario);
            string usucodigo = deserialize.usu_codigo.ToString();
            var accion = await DisplayActionSheet("¿Desea confirmar lista?", "Cancelar", null, "SI", "NO");
            if(accion == "SI")
            {
                curso.ConfirmarListaEnCurso(usucodigo);
                await Navigation.PopAsync();
            }
        }
    }
}