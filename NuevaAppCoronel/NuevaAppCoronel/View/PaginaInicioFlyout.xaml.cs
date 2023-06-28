using NuevaAppCoronel.View.Etiquetas;
using NuevaAppCoronel.View.PreparacionPedidos;
using NuevaAppCoronel.View.ReposicionMercaderia;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NuevaAppCoronel.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaInicioFlyout : ContentPage
    {
        public ListView ListView;

        public PaginaInicioFlyout()
        {
            InitializeComponent();
        }

        private async void btnUbicacionArticulos_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new UbicacionArticulos()));
        }

        private async void btnDevoluciones_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new Etiquetas.Etiquetas()));
        }

        private async void btnFalntates_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new ReposicionDeMercaderia()));
        }

        private async void btnPreparación_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new PreparacionInicio()));
        }
    }
}