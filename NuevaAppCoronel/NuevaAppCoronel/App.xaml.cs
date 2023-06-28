using NuevaAppCoronel.View;
using NuevaAppCoronel.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NuevaAppCoronel
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            ComprobarIP();
        }

        private void ComprobarIP()
        {
            VMUsuario usu = new VMUsuario();
            int resultado = usu.ComprobarConexion();

            if (resultado == 0)
            {
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
            else
            {
                Application.Current.MainPage = new LoginPage();
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
