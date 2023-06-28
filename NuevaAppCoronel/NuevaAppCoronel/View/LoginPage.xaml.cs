using NuevaAppCoronel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NuevaAppCoronel.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        VMLogin vm = new VMLogin();
        public LoginPage()
        {
            InitializeComponent();
        }


        private void btnIngresar_Clicked(object sender, EventArgs e)
        {
            try
            {
                var user = Usuario.Text;
                var pass = Contraseña.Text;
                if (user == null || pass == null)
                {
                    DisplayAlert("Error", "Ingrese Usuario y/o Contraseña", "OK");
                }
                else
                {
                    vm.GetUsuario(Usuario.Text, Contraseña.Text);
                    //Navigation.PushModalAsync(new PaginaInicio());
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Mensaje", "Error en la conexión", "OK");
            }
        }
    }
}