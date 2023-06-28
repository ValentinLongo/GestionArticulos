using NuevaAppCoronel.Model;
using NuevaAppCoronel.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NuevaAppCoronel.View.UbicacionArticulo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditarUbicacion : ContentPage
    {
        public static string ipUrl = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "url.txt");
        public static string URL = File.ReadAllText(ipUrl);
        HttpClient cliente = new HttpClient();
        VMUbicacionArticulos vm = new VMUbicacionArticulos();        
        public EditarUbicacion(MUbicacion ubi)
        {
            InitializeComponent();
            inicializarVista(ubi);
        }

        private void inicializarVista(MUbicacion ubi)
        {
            txtDeposito.Text = ubi.ubi_ubica1;
            txtPasillo.Text = ubi.ubi_ubica2;
            txtFila.Text = ubi.ubi_ubica3;
            txtColumna.Text = ubi.ubi_ubica4;
            txtUbi_codigo.Text = Convert.ToString(ubi.ubi_codigo);
            txtCodTex.Text = ubi.ubi_codtex;
            txtCodNum.Text = Convert.ToString(ubi.ubi_codnum);
            chkUbi_predef.IsChecked = Convert.ToBoolean(ubi.ubi_predef);
            txtAdicional.Text = ubi.ubi_adicional;
            txtUbi_predef.Text = Convert.ToString(ubi.ubi_predef);
            Deposito.Text = ubi.deposito;
            txtUbi_deposito.Text = Convert.ToString(ubi.ubi_deposito);

            bool val = Xamarin.Essentials.Preferences.ContainsKey("login");
            if (val)
            {
                var data = Xamarin.Essentials.Preferences.Get("login", "");
                var deserializer = Newtonsoft.Json.JsonConvert.DeserializeObject<MLogin>(data);
                txtUbi_usumod.Text = Convert.ToString(deserializer?.usu_codigo);
            }

            bool keys = Xamarin.Essentials.Preferences.ContainsKey("dato");
            if (keys)
            {
                var dato = Xamarin.Essentials.Preferences.Get("dato", "");
                var deserializer = Newtonsoft.Json.JsonConvert.DeserializeObject<MUbicacion>(dato);
            }
        }

        private void txtDeposito_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(txtDeposito.Text.Length >= 2)
            {
                Deposito.Text = vm.deposito(txtDeposito.Text);
            }
        }

        private async void btnGuardar_Clicked(object sender, EventArgs e)
        {
            if(chkUbi_predef.IsChecked == true)
            {
                if (txtAdicional.Text == "")
                {
                    txtAdicional.Text = "null";
                }
                if (txtUbi_codigo.Text == "")
                {
                    txtUbi_codigo.Text = "null";
                }
                URL = URL.Replace('\n', '/');
                string url = $"{URL}Articulo/UbiPredef/{txtCodTex.Text}/{txtCodNum.Text}/{txtAdicional.Text}/{txtUbi_codigo.Text}";
                HttpResponseMessage request = cliente.GetAsync(url).Result;
                if (!request.IsSuccessStatusCode)
                {
                    await DisplayAlert("Mensaje", "Error", "Ok");
                }
            }

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

            var accion = await DisplayActionSheet("¿Desea realizar la modificación?", "Cancelar", null, "SI", "NO");
            if (accion == "SI")
            {
                string anio = DateTime.Now.ToString("yyyy");
                string mes = DateTime.Now.ToString("MM");
                string dia = DateTime.Now.ToString("dd");
                string fecha = $"{anio}{mes}{dia}";
                URL = URL.Replace('\n', '/');
                string url = $"{URL}Articulo/modificarUbi/{txtDeposito.Text}/{txtPasillo.Text}/{txtFila.Text}/{txtColumna.Text}/{txtUbi_deposito.Text}/{txtUbi_usumod.Text}/{fecha}/{DateTime.Now.ToString("T")}/{txtUbi_codigo.Text}";
                HttpResponseMessage request = cliente.GetAsync(url).Result;
                if (request.IsSuccessStatusCode) 
                {
                    await DisplayAlert("Modificacion", "Registro Modificado", "OK");
                    await Navigation.PopAsync();
                }
            }
        }
    }
}