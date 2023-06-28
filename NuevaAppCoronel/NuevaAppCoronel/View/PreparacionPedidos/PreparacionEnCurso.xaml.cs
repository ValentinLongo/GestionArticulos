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

namespace NuevaAppCoronel.View.PreparacionPedidos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PreparacionEnCurso : ContentPage
    {
        VMPedidos vm = new VMPedidos();
        public static int idPedido;

        public PreparacionEnCurso()
        {
            InitializeComponent();
            inicializarVista();
        }

        private void inicializarVista()
        {
            ListaEnCurso.ItemsSource = vm.listaEnCurso();
        }

        protected override void OnAppearing()
        {
            inicializarVista();
        }

        private async void ListaEnCurso_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as MPedidos;
            var serialize = JsonConvert.SerializeObject(item);
            Preferences.Set("datosPedido", serialize);
            Pedidos.observacionPedido = item.vtaObserva;
            await Navigation.PushAsync(new DetallePedido(item.IdPedido, item.nombreVendedor,""));
        }
    }
}