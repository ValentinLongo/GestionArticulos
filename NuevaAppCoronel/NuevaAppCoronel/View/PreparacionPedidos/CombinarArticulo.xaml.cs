using Newtonsoft.Json;
using NuevaAppCoronel.Model;
using NuevaAppCoronel.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace NuevaAppCoronel.View.PreparacionPedidos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CombinarArticulo : ContentPage
    {
        public static string ipUrl = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "url.txt");
        public static string URL = File.ReadAllText(ipUrl);
        public static string ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "terminal.txt");
        public static int name = Convert.ToInt32(File.ReadAllText(ruta));
        HttpClient cliente = new HttpClient();
        VMReposiciones vm = new VMReposiciones();
        VMUbicacionArticulos datos = new VMUbicacionArticulos();
        VMPedidos vmPedidos = new VMPedidos();
        Pedidos mPedidos = new Pedidos();
        public CombinarArticulo()
        {
            InitializeComponent();
            picker.Items.Add("Descripcion");
            picker.Items.Add("Ubicacion");
            picker.Items.Add("Codigo de Barras");
            picker.Items.Add("Codigo Interno");
            picker.Items.Add("Codigo");
            picker.Items.Add("Codigo Fabrica");

            var codigo = Preferences.Get("articulo", "");
            var deserialize = JsonConvert.DeserializeObject<MDetallePedido>(codigo);
            IdPedido.Text = Convert.ToString(deserialize?.IdPedido);
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
            else if (Convert.ToString(picker.SelectedItem) == "Codigo Fábrica")
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

        private async void btnCamara_Clicked(object sender, EventArgs e)
        {
            ZXingScannerPage scannerPage = new ZXingScannerPage();
            scannerPage.OnScanResult += (result) =>
            {
                scannerPage.IsScanning = false;
                Device.BeginInvokeOnMainThread(() =>
                {

                    Navigation.PopAsync();
                    txtCodBarra.Text = result.Text;
                    //buscarArticulo();
                });
            };
            await Navigation.PushAsync(scannerPage);
        }

        private async void btnBuscar_Clicked(object sender, EventArgs e)
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
                    ListaFaltante.ItemsSource = datos.ListaArticulos("Descripcion", txtArticulo.Text, habArt);
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
                    ListaFaltante.ItemsSource = datos.ListaArticulos("Codigo de Barras", txtCodBarra.Text, habArt);
                    txtCodBarra.Text = "";
                }
                else
                {
                    await DisplayAlert("Mensaje", "Debe completar los campos requeridos", "Ok");
                }
            }
            else if (Convert.ToString(picker.SelectedItem) == "Codigo")
            {
                if (txtCodBarra.Text != "" && txtCodBarra.Text != null)
                {
                    string codigo = txtCodTex.Text + '-' + txtCodNum.Text;
                    ListaFaltante.ItemsSource = datos.ListaArticulos("Codigo", codigo, habArt);
                    txtCodNum.Text = "";
                    txtCodTex.Text = "";
                }
                else if (txtCodTex.Text == "" || txtCodTex.Text == null)
                {
                    string codigo = txtCodNum.Text;
                    ListaFaltante.ItemsSource = datos.ListaArticulos("CodigoN", codigo, habArt);
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
                    ListaFaltante.ItemsSource = datos.ListaArticulos("Codigo Fabrica", txtCodFabrica.Text, habArt);
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
                    ListaFaltante.ItemsSource = datos.ListaArticulos("Codigo Interno", txtCodigoInt.Text, habArt);
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
                ListaFaltante.BeginRefresh();
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
                    ListaFaltante.ItemsSource = datos.UbicacionArticulos(AuxCampo);
                    txtUbica1.Text = "";
                    txtUbica2.Text = "";
                    txtUbica3.Text = "";
                    txtUbica4.Text = "";
                }
                ListaFaltante.EndRefresh();
            }
        }

        private async void ListaFaltante_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            string idpedido = IdPedido.Text;
            var item = e.Item as MUbicacionArticulos;
            var dato = await DisplayPromptAsync("Mensaje", "Ingrese la cantidad del articulo", "Aceptar", "Cancelar", null, 10, Keyboard.Numeric, "");
            if (dato != null)
            {
                MArticulo mArticulo = new MArticulo()
                {
                    his_codigo = Convert.ToInt32(idpedido),
                    his_codtex = item.art_codtex,
                    his_codnum = item.art_codnum,
                    his_cpbte = "",
                    his_fecemi = "",
                    his_ctacli = DetallePedido.CtaCliente,
                    his_articulo = item.art_descri,
                    his_aliva = 0,
                    his_cantidad = Convert.ToDouble(dato),
                    his_orden = DetalleArticulo.orden,
                    his_adicional = item.adi_codigo,
                    his_preparada = Convert.ToDouble(dato)
                };
                if (mPedidos.permisoModificarCantidades == false)
                {
                    int resultado = vmPedidos.agregarArticuloCombinado(mArticulo);
                    if (resultado == 1)
                    {
                        await DisplayAlert("Mensaje", "Agregado correctamente", "OK");
                        Preferences.Set("bandera", "1");
                        await Navigation.PopAsync();
                    }
                    if (resultado == 0)
                    {
                        await DisplayAlert("Mensaje", "Se ha excedido la cantidad", "OK");
                    }
                }
                else
                {
                    vmPedidos.agregarArticuloCombinadoCualquierCant(mArticulo);
                    await DisplayAlert("Mensaje", "Agregado correctamente", "OK");
                    Preferences.Set("bandera", "1");
                    await Navigation.PopAsync();
                }
            }
        }

    }
}