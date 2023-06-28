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

namespace NuevaAppCoronel.View.PreparacionPedidos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetalleArticulo : ContentPage
    {
        VMUbicacionArticulos vmUbi = new VMUbicacionArticulos();
        VMPedidos vm = new VMPedidos();
        public static int orden;
        public DetalleArticulo()
        {
            InitializeComponent();
            var codigo = Preferences.Get("articulo", "");
            var deserialize = JsonConvert.DeserializeObject<MDetallePedido>(codigo);
            CodigoArticulo.Text = Convert.ToString(deserialize?.IdArticulos);
            CodigoPedido.Text = Convert.ToString(deserialize?.IdPedido);
            DescripcionArticulo.Text = Convert.ToString(deserialize?.Articulo);
            AdicionalArticulo.Text = "Adicional: " + Convert.ToString(deserialize?.DescAdicional);
            CodTex.Text = "Código: " + Convert.ToString(deserialize?.CodTex) + " - " + Convert.ToString(deserialize?.IdArticulos);
            //CodTex.Text = Convert.ToString(deserialize?.IdArticulos);
            Cantidad.Text = Convert.ToString(deserialize?.Cantidad);
            CodAdi.Text = Convert.ToString(deserialize?.Adicional);
            if (Convert.ToInt32(deserialize?.numPrep) > 0)
            {
                Cantidadd.Text = Convert.ToString(deserialize?.numPrep);
            }
            string path = Convert.ToString(deserialize?.Imagen);
            imgProducto.Source = path;
            orden = Convert.ToInt32(deserialize?.Orden);
            listaUbi.ItemsSource = vmUbi.ubicacion(deserialize?.CodTex, deserialize?.IdArticulos.ToString(), deserialize?.Adicional);
        }

        protected override void OnAppearing()
        {
            var bandera = Preferences.Get("bandera", "");
            if (bandera == "1")
            {
                Navigation.PopAsync();
                Preferences.Set("bandera", "");
            }
            var codigo2 = Preferences.Get("datoUbic", "");
            var deserialize2 = JsonConvert.DeserializeObject<MUbicacionArticulos>(codigo2);
            if (codigo2 != "")
            {
                ArticuloReemplazo.Text = Convert.ToString(deserialize2?.art_descri);
                CodTexRe.Text = Convert.ToString(deserialize2?.art_codtex);
                CodNumRe.Text = Convert.ToString(deserialize2?.art_codnum);
                AdicionalRe.Text = Convert.ToString(deserialize2?.adi_codigo);
                AdicionalDescriRem.Text = Convert.ToString(deserialize2?.adi_descri);
            }
            if (ArticuloReemplazo.Text != "" && ArticuloReemplazo.Text != null)
            {
                PedirReposicion.IsEnabled = false;
            }
            else
            {
                PedirReposicion.IsEnabled = true;
            }
        }

        private void BtnUbi_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ReemplazarArticulo());
        }

        private void Combinar_Clicked(object sender, EventArgs e)
        {
            double cantidadPreparada = Convert.ToInt32(Cantidadd.Text);
            int cantidadPedida = Convert.ToInt32(Cantidad.Text);
            var codigo = Preferences.Get("articulo", "");
            var deserialize = JsonConvert.DeserializeObject<MDetallePedido>(codigo);
            if (Cantidadd.Text != null && (cantidadPreparada < cantidadPedida) && cantidadPreparada > 0)
            {
                vm.modificarCantidadPreparada(cantidadPreparada, CodigoPedido.Text, deserialize.CodTex, CodigoArticulo.Text, deserialize.Adicional);
                Navigation.PushAsync(new CombinarArticulo());
            }
            else
            {
                DisplayAlert("Mensaje", "Para combinar, la cantidad preparada debe ser mayor a cero y menor a la cantidad requerida", "Ok");
            }
        }

        Pedidos mPedidos = new Pedidos();
        private void Aceptar_Clicked(object sender, EventArgs e)
        {
            double cantidadPreparada = Convert.ToDouble(Cantidadd.Text);
            double cantidadPedida = Convert.ToDouble(Cantidad.Text);
            var codigo = Preferences.Get("articulo", "");
            var deserialize = JsonConvert.DeserializeObject<MDetallePedido>(codigo);
            if (mPedidos.permisoModificarCantidades == false)
            {
                if (cantidadPreparada <= cantidadPedida)
                {
                    var codigo2 = Preferences.Get("datoUbic", "");
                    var deserialize2 = JsonConvert.DeserializeObject<MUbicacionArticulos>(codigo2);
                    if (codigo2 != "")
                    {
                        vm.modificarCantidadReemplazo(cantidadPreparada, CodigoPedido.Text, deserialize.CodTex, CodigoArticulo.Text, deserialize.Adicional, deserialize2.art_codtex, deserialize2.art_codnum.ToString(), deserialize2.adi_codigo, ArticuloReemplazo.Text);
                    }
                    else
                    {
                        vm.modificarCantidadPreparada(cantidadPreparada, CodigoPedido.Text, deserialize.CodTex, CodigoArticulo.Text, deserialize.Adicional);
                    }

                    Navigation.PopAsync();
                }
                else
                {
                    DisplayAlert("Mensaje", "La cantidad ingresada es mayor a la solicitada", "OK");
                }
            }
            else if (mPedidos.permisoModificarCantidades == true)
            {
                var codigo2 = Preferences.Get("datoUbic", "");
                var deserialize2 = JsonConvert.DeserializeObject<MUbicacionArticulos>(codigo2);
                if (codigo2 != "")
                {
                    vm.modificarCantidadReemplazo(cantidadPreparada, CodigoPedido.Text, deserialize.CodTex, CodigoArticulo.Text, deserialize.Adicional, deserialize2.art_codtex, deserialize2.art_codnum.ToString(), deserialize2.adi_codigo, ArticuloReemplazo.Text);
                }
                else
                {
                    vm.modificarCantidadPreparada(cantidadPreparada, CodigoPedido.Text, deserialize.CodTex, CodigoArticulo.Text, deserialize.Adicional);
                }
                Navigation.PopAsync();
            }
        }

        private async void PedirReposicion_Clicked(object sender, EventArgs e)
        {
            var accion = await DisplayActionSheet("¿Desea realizar pedido de reposición de este articulo?", "Cancelar", null, "SI", "NO");
            if (accion == "SI")
            {
                double cantidadPreparada = Convert.ToInt32(Cantidadd.Text);
                double cantidadPedida = Convert.ToInt32(Cantidad.Text);
                double diferenciaDeCantidades = cantidadPedida - cantidadPreparada;
                var codigo = Preferences.Get("articulo", "");
                var deserialize = JsonConvert.DeserializeObject<MDetallePedido>(codigo);
                vm.modificarCantidadParaPedido(cantidadPreparada.ToString(), CodigoPedido.Text, deserialize.CodTex, CodigoArticulo.Text, deserialize.Adicional);

                await Navigation.PopAsync();
            }
        }
    }
}