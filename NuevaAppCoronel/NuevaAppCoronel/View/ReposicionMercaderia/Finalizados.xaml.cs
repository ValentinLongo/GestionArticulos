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
    public partial class Finalizados : ContentPage
    {
        VMFinalizados vm = new VMFinalizados();
        public Finalizados()
        {
            InitializeComponent();
            inicializarVista();
        }

        protected override void OnAppearing()
        {
            inicializarVista();
        }

        private void inicializarVista()
        {
            ListaFinalizados.ItemsSource = vm.listaFinalizados();
        }
    }
}