using NuevaAppCoronel.Model;
using NuevaAppCoronel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NuevaAppCoronel.View.ReposicionMercaderia
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registrados : ContentPage
    {
        VMRegistrados vm = new VMRegistrados();
        VMEnCurso encurso = new VMEnCurso();
        public static int IdFaltantes;
        public Registrados()
        {
            InitializeComponent();
            inicializarVista();
        }

        private void inicializarVista()
        {
            ListaRegistrados.ItemsSource = vm.listaRegistrados();
        }

        protected override void OnAppearing()
        {
            inicializarVista();
        }

        private async void ListaRegistrados_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as MListaFaltante;
            if (item != null)
            {
                IdFaltantes = item.idFaltantes;
            }
            await Navigation.PushAsync(new DetalleRegistrado());
        }

        //Nueva repo
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            List<MListaFaltante> lista = new List<MListaFaltante>();
            lista = encurso.listaEnCurso();
            if(lista.Count == 0)
            {
                var accion = await DisplayActionSheet("¿Desea comenzar una nueva reposición?", null, null, "SI", "NO");
                if(accion == "SI")
                {
                    await Navigation.PushAsync(new NuevaReposicion());
                }
            }
            else
            {
                await DisplayAlert("Mensaje", "Ya tiene una reposición en curso","OK");
            }
        }
    }
}