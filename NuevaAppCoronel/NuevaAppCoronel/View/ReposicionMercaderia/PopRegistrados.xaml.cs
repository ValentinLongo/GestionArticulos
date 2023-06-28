using Newtonsoft.Json;
using NuevaAppCoronel.Model;
using NuevaAppCoronel.ViewModel;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NuevaAppCoronel.View.ReposicionMercaderia
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopRegistrados : ContentPage
    {
        VMUbicacionArticulos vm = new VMUbicacionArticulos();
        VMReposiciones vmRepo = new VMReposiciones();
        MDetalleLista oDetalleLista = new MDetalleLista();
        public PopRegistrados()
        {
            InitializeComponent();
            inicializarVista();
        }

        private void inicializarVista()
        {
            var datos = Preferences.Get("item", "");
            var deserialize = JsonConvert.DeserializeObject<MDetalleLista>(datos);
            oDetalleLista = deserialize;
            imgArtFaltante.Source = deserialize.imagen;
            listaUbicaciones.ItemsSource = vm.ubicacion(deserialize.aux_codtex, deserialize.aux_codnum.ToString(), deserialize.aux_adicional);
        }

        protected override void OnAppearing()
        {
            inicializarVista();
        }

        private async void btnPreparado_Clicked(object sender, EventArgs e)
        {
            vmRepo.actualizarEstadoArticulo("1", oDetalleLista.aux_codtex, oDetalleLista.aux_codnum.ToString(), oDetalleLista.aux_adicional, DetalleRegistrado.detcodigo.ToString());
            await Navigation.PopAsync();
        }

        private void btnSinStock_Clicked(object sender, EventArgs e)
        {
            vmRepo.actualizarEstadoArticulo("2", oDetalleLista.aux_codtex, oDetalleLista.aux_codnum.ToString(), oDetalleLista.aux_adicional, DetalleRegistrado.detcodigo.ToString());
            Navigation.PopAsync();
        }

        private void btnNoPreparado_Clicked(object sender, EventArgs e)
        {
            vmRepo.actualizarEstadoArticulo("0", oDetalleLista.aux_codtex, oDetalleLista.aux_codnum.ToString(), oDetalleLista.aux_adicional, DetalleRegistrado.detcodigo.ToString());
            Navigation.PopAsync();
        }
    }
}