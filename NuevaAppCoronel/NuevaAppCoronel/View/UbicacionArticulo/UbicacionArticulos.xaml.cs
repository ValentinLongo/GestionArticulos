using Newtonsoft.Json;
using NuevaAppCoronel.Model;
using NuevaAppCoronel.View.UbicacionArticulo;
using NuevaAppCoronel.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace NuevaAppCoronel.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UbicacionArticulos : ContentPage
    {
        public static string ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "terminal.txt");
        public static int name = Convert.ToInt32(File.ReadAllText(ruta));
        VMUbicacionArticulos datos = new VMUbicacionArticulos();
        public UbicacionArticulos()
        {
            InitializeComponent();
            picker.Items.Add("Descripcion");
            picker.Items.Add("Ubicacion");
            picker.Items.Add("Codigo de Barras");
            picker.Items.Add("Codigo Interno");
            picker.Items.Add("Codigo");
            picker.Items.Add("Codigo Fabrica");
            colorLista();
        }

        private void colorLista()
        {
            int color;
            color = datos.cantidadUbi(name);
            if (color == 1)
            {
                btnListaImprimir.BackgroundColor = Color.FromHex("#2196F3");
                btnListaImprimir.TextColor = Color.White;
            }
            else
            {
                btnListaImprimir.BackgroundColor = Color.FromHex("#C9CACA");
                btnListaImprimir.TextColor = Color.Black;
            }
        }

        protected override void OnAppearing()
        {
            colorLista();
        }

        private void btnCamara_Clicked(object sender, EventArgs e)
        {
            Scanner();
        }

        private void picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToString(picker.SelectedItem) == "Descripcion")
            {
                txtUbica1.IsVisible = false;
                txtUbica2.IsVisible = false;
                txtUbica3.IsVisible = false;
                txtUbica4.IsVisible = false;
                txtCodNum.IsVisible = false;
                txtCodTex.IsVisible = false;
                txtCodBarra.IsVisible = false;
                btnCamara.IsVisible = false;
                txtArticulo.IsVisible = true;
                txtCodFabrica.IsVisible = false;
                txtCodigoInt.IsVisible = false;
            }
            else if (Convert.ToString(picker.SelectedItem) == "Ubicacion")
            {
                txtUbica1.IsVisible = true;
                txtUbica2.IsVisible = true;
                txtUbica3.IsVisible = true;
                txtUbica4.IsVisible = true;
                txtArticulo.IsVisible = false;
                txtCodBarra.IsVisible = false;
                txtCodNum.IsVisible = false;
                txtCodTex.IsVisible = false;
                btnCamara.IsVisible = false;
                txtCodigoInt.IsVisible = false;
                txtCodFabrica.IsVisible = false;
            }
            else if (Convert.ToString(picker.SelectedItem) == "Codigo de Barras")
            {
                txtUbica1.IsVisible = false;
                txtUbica2.IsVisible = false;
                txtUbica3.IsVisible = false;
                txtUbica4.IsVisible = false;
                txtArticulo.IsVisible = false;
                txtCodBarra.IsVisible = true;
                btnCamara.IsVisible = true;
                txtCodNum.IsVisible = false;
                txtCodTex.IsVisible = false;
                txtCodigoInt.IsVisible = false;
                txtCodFabrica.IsVisible = false;
            }
            else if (Convert.ToString(picker.SelectedItem) == "Codigo")
            {
                txtUbica1.IsVisible = false;
                txtUbica2.IsVisible = false;
                txtUbica3.IsVisible = false;
                txtUbica4.IsVisible = false;
                txtArticulo.IsVisible = false;
                txtCodBarra.IsVisible = false;
                btnCamara.IsVisible = false;
                txtCodNum.IsVisible = true;
                txtCodTex.IsVisible = true;
                txtCodigoInt.IsVisible = false;
                txtCodFabrica.IsVisible = false;
            }
            else if (Convert.ToString(picker.SelectedItem) == "Codigo Interno")
            {
                txtUbica1.IsVisible = false;
                txtUbica2.IsVisible = false;
                txtUbica3.IsVisible = false;
                txtUbica4.IsVisible = false;
                txtArticulo.IsVisible = false;
                txtCodBarra.IsVisible = false;
                btnCamara.IsVisible = false;
                txtCodNum.IsVisible = false;
                txtCodTex.IsVisible = false;
                txtCodigoInt.IsVisible = true;
                txtCodFabrica.IsVisible = false;
            }
            else if (Convert.ToString(picker.SelectedItem) == "Codigo Fabrica")
            {
                txtUbica1.IsVisible = false;
                txtUbica2.IsVisible = false;
                txtUbica3.IsVisible = false;
                txtUbica4.IsVisible = false;
                txtArticulo.IsVisible = false;
                txtCodBarra.IsVisible = false;
                btnCamara.IsVisible = false;
                txtCodNum.IsVisible = false;
                txtCodTex.IsVisible = false;
                txtCodigoInt.IsVisible = false;
                txtCodFabrica.IsVisible = true;
            }
        }

        private void btnBuscar_Clicked(object sender, EventArgs e)
        {
            buscarArticulo();
        }


        private void ListaArt_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as MUbicacionArticulos;//Guarto todos mis atributos de mi modelo 'MUbicaciones' en la variable 'item'
            var serializer = JsonConvert.SerializeObject(item);
            if (item != null)
            {
                Preferences.Set("datoUbi", serializer);
            }
            Navigation.PushAsync(new DetUbicArticulo());
        }
        public async void buscarArticulo()
        {
            int habArt;
            if (CheckArtHabilitados.IsChecked == true)
            {
                habArt = 1;
            }
            else
            {
                habArt = 2;
            }
            if (Convert.ToString(picker.SelectedItem) == "Descripcion")
            {
                if (txtArticulo.Text != "")
                {
                    ListaArt.ItemsSource = datos.ListaArticulos("Descripcion", txtArticulo.Text, habArt);
                    txtArticulo.Text = "";
                }
                else
                {
                    await DisplayAlert("Mensaje", "Debe completar los campos requeridos", "Ok");
                }
            }
            else if (Convert.ToString(picker.SelectedItem) == "Codigo de Barras")
            {
                if (txtCodBarra.Text != "")
                {
                    ListaArt.ItemsSource = datos.ListaArticulos("Codigo de Barras", txtCodBarra.Text, habArt);
                    txtCodBarra.Text = "";
                }
                else
                {
                    await DisplayAlert("Mensaje", "Debe completar los campos requeridos", "Ok");
                }
            }
            else if (Convert.ToString(picker.SelectedItem) == "Codigo")
            {
                if (txtCodTex.Text != "" && txtCodTex.Text != null)
                {
                    string codigo = txtCodTex.Text + '-' + txtCodNum.Text;
                    ListaArt.ItemsSource = datos.ListaArticulos("Codigo", codigo, habArt);
                    txtCodNum.Text = "";
                    txtCodTex.Text = "";
                }
                else if (txtCodTex.Text == "" || txtCodTex.Text == null)
                {
                    string codigo = txtCodNum.Text;
                    ListaArt.ItemsSource = datos.ListaArticulos("CodigoN", codigo, habArt);
                    txtCodNum.Text = "";
                    txtCodTex.Text = "";
                }
                else
                {
                    await DisplayAlert("Mensaje", "Debe completar los campos requeridos", "Ok");
                }
            }
            else if (Convert.ToString(picker.SelectedItem) == "Codigo Fabrica")
            {
                if (txtCodFabrica.Text != "")
                {
                    ListaArt.ItemsSource = datos.ListaArticulos("Codigo Fabrica", txtCodFabrica.Text, habArt);
                }
                else
                {
                    await DisplayAlert("Mensaje", "Debe completar los campos requeridos", "Ok");
                }
            }
            else if (Convert.ToString(picker.SelectedItem) == "Codigo Interno")
            {
                if (txtCodigoInt.Text != "")
                {
                    ListaArt.ItemsSource = datos.ListaArticulos("Codigo Interno", txtCodigoInt.Text, habArt);
                    txtCodigoInt.Text = "";
                }
                else
                {
                    await DisplayAlert("Mensaje", "Debe completar los campos requeridos", "Ok");
                }
            }
            else if (Convert.ToString(picker.SelectedItem) == "Ubicacion")
            {
                string strUbi = txtUbica1.Text;
                if (string.IsNullOrEmpty(strUbi))
                {
                    string longi = string.Empty.Length.ToString(strUbi);
                    //txtUbica1.Text = string.Format(strUbi, "000");
                    txtUbica1.Text = longi.PadLeft(3, '0');
                }
                if (txtUbica1.Text.Length == 1)
                {
                    txtUbica1.Text = strUbi.PadLeft(3, '0');
                }
                if (txtUbica1.Text.Length == 2)
                {
                    txtUbica1.Text = strUbi.PadLeft(3, '0');
                }
                string strUbi2 = txtUbica2.Text;
                if (string.IsNullOrEmpty(strUbi2))
                {
                    string longi = string.Empty.Length.ToString(strUbi2);
                    //txtUbica1.Text = string.Format(strUbi, "000");
                    txtUbica2.Text = longi.PadLeft(3, '0');
                }
                if (txtUbica2.Text.Length == 1)
                {
                    txtUbica2.Text = strUbi2.PadLeft(3, '0');
                }
                if (txtUbica2.Text.Length == 2)
                {
                    txtUbica2.Text = strUbi2.PadLeft(3, '0');
                }
                string strUbi3 = txtUbica3.Text;
                if (string.IsNullOrEmpty(strUbi3))
                {
                    string longi = string.Empty.Length.ToString(strUbi3);
                    //txtUbica1.Text = string.Format(strUbi, "000");
                    txtUbica3.Text = longi.PadLeft(3, '0');
                }
                if (txtUbica3.Text.Length == 1)
                {
                    txtUbica3.Text = strUbi3.PadLeft(3, '0');
                }
                if (txtUbica3.Text.Length == 2)
                {
                    txtUbica3.Text = strUbi3.PadLeft(3, '0');
                }
                string strUbi4 = txtUbica4.Text;
                if (string.IsNullOrEmpty(strUbi4))
                {
                    string longi = string.Empty.Length.ToString(strUbi4);
                    //txtUbica1.Text = string.Format(strUbi, "000");
                    txtUbica4.Text = longi.PadLeft(3, '0');
                }
                if (txtUbica4.Text.Length == 1)
                {
                    txtUbica4.Text = strUbi4.PadLeft(3, '0');
                }
                if (txtUbica4.Text.Length == 2)
                {
                    txtUbica4.Text = strUbi4.PadLeft(3, '0');
                }
                var ubica = txtUbica1.Text + '-' + txtUbica2.Text + '-' + txtUbica3.Text + '-' + txtUbica4.Text;
                string AuxCampo = "(ubi_ubica1 + '-' + ubi_ubica2 +'-'+ ubi_ubica3 +'-'+ ubi_ubica4) = '" + ubica + "' and art_vigencia = " + habArt + "";
                ListaArt.BeginRefresh();
                if (ubica == "000-000-000-000")
                {
                    await DisplayAlert("Advertencia", "Debe Completar Campos para Realizar la Busqueda", "OK");
                    txtUbica1.Text = "";
                    txtUbica2.Text = "";
                    txtUbica3.Text = "";
                    txtUbica4.Text = "";
                }
                else
                {
                    ListaArt.ItemsSource = datos.UbicacionArticulos(AuxCampo);
                    txtUbica1.Text = "";
                    txtUbica2.Text = "";
                    txtUbica3.Text = "";
                    txtUbica4.Text = "";
                }
                ListaArt.EndRefresh();
            }
        }
        private async void Scanner()
        {
            ZXingScannerPage scannerPage = new ZXingScannerPage();
            scannerPage.OnScanResult += (result) =>
            {
                scannerPage.IsScanning = false;
                Device.BeginInvokeOnMainThread(() =>
                {

                    Navigation.PopAsync();
                    txtCodBarra.Text = result.Text;
                    buscarArticulo();
                });
            };
            await Navigation.PushAsync(scannerPage);
        }

        private void btnListaImprimir_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ImprimirUbi());
        }
    }
}