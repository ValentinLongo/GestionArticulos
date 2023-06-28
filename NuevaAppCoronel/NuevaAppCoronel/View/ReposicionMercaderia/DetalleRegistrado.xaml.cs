using Newtonsoft.Json;
using NuevaAppCoronel.Model;
using NuevaAppCoronel.ViewModel;
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
    public partial class DetalleRegistrado : ContentPage
    {
        VMReposiciones vm = new VMReposiciones();
        List<MDetalleLista> listaRegistrados = new List<MDetalleLista>();
        public static int detcodigo;
        public DetalleRegistrado()
        {
            InitializeComponent();
            detcodigo = Registrados.IdFaltantes;
            inicializarVista();
        }

        private void inicializarVista()
        {
            listaRegistrados = vm.listaDetalleRegistrados(detcodigo);
            listaDetFalt.ItemsSource = listaRegistrados;
        }

        private async void listaDetFalt_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as MDetalleLista;
            var serialize = JsonConvert.SerializeObject(item);
            Preferences.Set("item", serialize);
            await Navigation.PushAsync(new PopRegistrados());
        }
        protected override void OnAppearing()
        {
            inicializarVista();
            verificarListaTerminada();
        }

        private async void verificarListaTerminada()
        {
            bool ListaTerminada = true;
            foreach(var item in listaRegistrados)
            {
                if(item.estado == "NO PREPARADO")
                {
                    ListaTerminada = false;
                }
            }
            if(ListaTerminada == true)
            {
                var finalizar = await DisplayActionSheet("¿Desea finalizar la reposición?", "Cancelar", null, "SI", "NO");
                if(finalizar == "SI")
                {
                    vm.finalizarListaRegistrados(detcodigo);
                    await DisplayAlert("Mensaje","Reposición finalizada","OK");
                    await Navigation.PopAsync();
                }
            }
        }
    }
}