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

namespace NuevaAppCoronel.View.UbicacionArticulo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class AgregarArticulo : ContentPage
    {
        VMUbicacionArticulos vm = new VMUbicacionArticulos();
        public static string Vigencia;
        public static string art_vigencia;
        public static int permiso;
        public AgregarArticulo()
        {
            InitializeComponent();
            descDepo.ItemsSource = vm.ListaDepositos();
            bool keys = Preferences.ContainsKey("dato");
            if (keys)
            {
                var dato = Preferences.Get("dato", "");
                var deserializer = JsonConvert.DeserializeObject<MUbicacion>(dato);
                txtubi_codigo.Text = Convert.ToString(deserializer?.ubi_codigo);
                txtUbiDepo.Text = Convert.ToString(deserializer?.ubi_deposito);
            }

            bool val = Preferences.ContainsKey("login");
            if (val)
            {
                var data = Preferences.Get("login", "");
                var deserializer = JsonConvert.DeserializeObject<MLogin>(data);
                txtUbi_usualta.Text = Convert.ToString(deserializer?.usu_codigo);

            }

            bool ubi = Preferences.ContainsKey("datoUbi");
            if (ubi)
            {
                var datoUbi = Preferences.Get("datoUbi", "");
                var deserializer = JsonConvert.DeserializeObject<MUbicacionArticulos>(datoUbi);
                txtada_vigencia.Text = Convert.ToString(deserializer?.ada_vigencia);
                txtArt_vigencia.Text = Convert.ToString(deserializer?.art_vigencia);
                txtCodtex.Text = deserializer?.art_codtex;
                txtCodnum.Text = Convert.ToString(deserializer?.art_codnum);
                txtUbiAdi.Text = deserializer?.adi_codigo;
                //txtVigencia.Text = Convert.ToString(deserializer?.Vigencia);
            }
            vm.buscarVigencia(txtCodtex.Text, txtCodnum.Text, txtUbiAdi.Text);
        }

        private async void btnAgregar_Clicked(object sender, EventArgs e)
        {
            string strUbi = txtDeposito.Text;
            if (string.IsNullOrEmpty(strUbi))
            {
                string longi = string.Empty.Length.ToString(strUbi);
                //txtUbica1.Text = string.Format(strUbi, "000");
                txtDeposito.Text = longi.PadLeft(3, '0');
            }
            if (txtDeposito.Text.Length == 1)
            {
                txtDeposito.Text = strUbi.PadLeft(3, '0');
            }
            if (txtDeposito.Text.Length == 2)
            {
                txtDeposito.Text = strUbi.PadLeft(3, '0');
            }
            string strUbi2 = txtPasillo.Text;
            if (string.IsNullOrEmpty(strUbi2))
            {
                string longi = string.Empty.Length.ToString(strUbi2);
                //txtUbica1.Text = string.Format(strUbi, "000");
                txtPasillo.Text = longi.PadLeft(3, '0');
            }
            if (txtPasillo.Text.Length == 1)
            {
                txtPasillo.Text = strUbi2.PadLeft(3, '0');
            }
            if (txtPasillo.Text.Length == 2)
            {
                txtPasillo.Text = strUbi2.PadLeft(3, '0');
            }
            string strUbi3 = txtFila.Text;
            if (string.IsNullOrEmpty(strUbi3))
            {
                string longi = string.Empty.Length.ToString(strUbi3);
                //txtUbica1.Text = string.Format(strUbi, "000");
                txtFila.Text = longi.PadLeft(3, '0');
            }
            if (txtFila.Text.Length == 1)
            {
                txtFila.Text = strUbi3.PadLeft(3, '0');
            }
            if (txtFila.Text.Length == 2)
            {
                txtFila.Text = strUbi3.PadLeft(3, '0');
            }
            string strUbi4 = txtColumna.Text;
            if (string.IsNullOrEmpty(strUbi4))
            {
                string longi = string.Empty.Length.ToString(strUbi4);
                //txtUbica1.Text = string.Format(strUbi, "000");
                txtColumna.Text = longi.PadLeft(3, '0');
            }
            if (txtColumna.Text.Length == 1)
            {
                txtColumna.Text = strUbi4.PadLeft(3, '0');
            }
            if (txtColumna.Text.Length == 2)
            {
                txtColumna.Text = strUbi4.PadLeft(3, '0');
            }
            MUbicacion oUbicacion = new MUbicacion()
            {
                ubi_ubica1 = txtDeposito.Text,
                ubi_ubica2 = txtPasillo.Text,
                ubi_ubica3 = txtFila.Text,
                ubi_ubica4 = txtColumna.Text
            };

            vm.buscarPermisos(txtubi_codigo.Text);

            if(Vigencia != "1")
            {
                if(permiso == 0)
                {
                    await DisplayAlert("Aviso", "El Articulo y/o su Adicional se encuentra Inhabilitado. Debera Habilitarlo para poder vincular una Ubicación", "Aceptar");
                }
                var accion = await DisplayAlert("Aviso", "El Articulo y/o Adicional se encuentra Inhabilitado. ¿Desea Habilitarlo?", "SI", "NO");
                if(accion == true)
                {
                    vm.habilitar(txtCodtex.Text, txtCodnum.Text, txtUbiAdi.Text, txtArt_vigencia.Text, txtUbi_usualta.Text);
                }
            }
            else
            {
                int codigoDeposito = vm.codeposito(oUbicacion.ubi_ubica1);
                vm.agregarUbicacion(txtubi_codigo.Text, txtCodtex.Text, txtCodnum.Text, oUbicacion.ubi_ubica1, oUbicacion.ubi_ubica2, oUbicacion.ubi_ubica3, oUbicacion.ubi_ubica4, txtUbiAdi.Text, txtUbi_usualta.Text, codigoDeposito.ToString());
                await Navigation.PopAsync();
            }

        }

        private void txtDeposito_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtDeposito.Text.Length > 2)
            {

                //descDepo.Text = vm.deposito(txtDeposito.Text);
                descDepo.SelectedIndex = vm.codeposito(txtDeposito.Text);
            }
        }
    }
}