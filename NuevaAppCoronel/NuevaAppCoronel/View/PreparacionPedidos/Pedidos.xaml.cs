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
    public partial class Pedidos : ContentPage
    {
        VMPedidos vm = new VMPedidos();
        public static int bandera;
        public bool permisoModificarCantidades;
        public static string observacionPedido;
        public Pedidos()
        {
            InitializeComponent();
            inicializarVista();
        }

        private void inicializarVista()
        {
            permisoModificarCantidades = vm.permisoParaModCant();
            ListaPedidos.ItemsSource = vm.listaPedidos();
        }

        protected override void OnAppearing()
        {
            inicializarVista();
        }

        private async void ListaPedidos_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as MPedidos;
            if (item.NIndex == 0)
            {
                observacionPedido = item.vtaObserva;

                var data = Preferences.Get("login", "");
                var deserialize = JsonConvert.DeserializeObject<MLogin>(data);
                int idUusario = Convert.ToInt32(deserialize?.usu_codigo);

                int validarMovimientoPorUsuario = vm.validarModificacionEnCursoPorUsuario(idUusario);

                if (validarMovimientoPorUsuario == 1)
                {
                    var accion = await DisplayActionSheet("¿Desea comenzar un nuevo pedido?", null, null, "SI", "NO");
                    if (accion == "SI")
                    {
                        int validarMovimiento = vm.validarModificacionEnCurso(item.IdPedido);
                        if (validarMovimiento == 1)
                        {
                            vm.iniciarPedido(item.IdPedido, item.TipMov.ToString(), item.NumeroComprobante, idUusario);
                            var serialize = JsonConvert.SerializeObject(item);
                            Preferences.Set("datosPedido", serialize);
                            await Navigation.PushAsync(new DetallePedido(item.IdPedido, item.nombreVendedor, "", item.IdCliente));
                        }
                        else
                        {
                            await DisplayAlert("Mensaje", "El pedido ya ha sido modificado", "Aceptar");
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Mensaje", "Tiene un pedido en curso", "Aceptar");
                }

            }
            else
            {
                await DisplayAlert("Mensaje", "Solo puede comenzar el primer pedido", "Aceptar");
                inicializarVista();
            }
        }

        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            OnAppearing();
            await Task.Delay(3000);
            refresco.IsRefreshing = false;
        }
    }
}