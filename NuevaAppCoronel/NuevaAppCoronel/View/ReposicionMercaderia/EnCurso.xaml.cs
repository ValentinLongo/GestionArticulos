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
    public partial class EnCurso : ContentPage
    {
        VMEnCurso vm = new VMEnCurso();
        public EnCurso()
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

        private void ListaEnCurso_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Navigation.PushAsync(new DetalleEnCurso());
        }
    }
}