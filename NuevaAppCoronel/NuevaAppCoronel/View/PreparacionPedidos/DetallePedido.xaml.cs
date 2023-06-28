using Newtonsoft.Json;
using NuevaAppCoronel.Model;
using NuevaAppCoronel.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NuevaAppCoronel.View.PreparacionPedidos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetallePedido : ContentPage
    {
        VMPedidos vm = new VMPedidos();
        VMCarro vmCarro = new VMCarro();
        int IdPedido;
        string DetalleBusqueda;

        public static string ipUrl = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "url.txt");
        public static string URL = File.ReadAllText(ipUrl);
        public static string ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "terminal.txt");
        public static int name = Convert.ToInt32(File.ReadAllText(ruta));
        HttpClient cliente = new HttpClient();
        VMReposiciones vm2 = new VMReposiciones();
        VMUbicacionArticulos datos = new VMUbicacionArticulos();
        VMEnCurso vmEnCurso = new VMEnCurso();
        public static List<MDetallePedido> listaOriginal = new List<MDetallePedido>();
        string NombreVendedor;
        public static int CtaCliente;
        public static int NumeroCarroActual;
        public static int NumeroPreparacionActual;
        public DetallePedido(int idPedido, [Optional] string nombreVendedor, [Optional] string detalleBusqueda, [Optional] int ctaCliente)
        {
            InitializeComponent();
            IdPedido = idPedido;
            NombreVendedor = nombreVendedor;
            DetalleBusqueda = detalleBusqueda;
            CtaCliente = ctaCliente;
            vm.CarroYPreparacion(IdPedido);
            inicializarVista(idPedido);
        }

        private void inicializarVista(int IdPedido)
        {
            var pedido = Preferences.Get("datosPedido", "");
            var deserializePedidos = JsonConvert.DeserializeObject<MPedidos>(pedido);
            string aceptaReemplazo = vm.aceptaReemplazo(IdPedido);
            lbModificaciones.Text = aceptaReemplazo;
            string vendedor = NombreVendedor;
            lbVendedor.Text = $"Vendedor = {vendedor}";
            lbCliente.Text = $"Cliente = {deserializePedidos.Cliente}";
            enObservacion.Text = Pedidos.observacionPedido;
            lbCarro.Text = $"Etiqueta: {NumeroCarroActual}";

            if (DetalleBusqueda == null || DetalleBusqueda == "")
            {
                ListaProducto.ItemsSource = vm.listaDetallePedido(IdPedido, NumeroCarroActual);
            }
            else
            {
                ListaProducto.ItemsSource = vm.listaDetallePedidoConPendiente(IdPedido);
            }
        }

        protected override void OnAppearing()
        {
            inicializarVista(IdPedido);
        }

        private void ListaProducto_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as MDetallePedido;
            if (item.RefPrepara == NumeroPreparacionActual || item.RefPrepara == 0)
            {
                var serializer = JsonConvert.SerializeObject(item);
                if (item != null)
                {
                    Preferences.Set("articulo", serializer);
                }
                Preferences.Set("datoUbic", "");
                Preferences.Set("bandera", "");
                Navigation.PushAsync(new DetalleArticulo());
            }
            else
            {
                DisplayAlert("Mensaje", "No se puede modificar este artículo", "Ok");
            }
        }
        private void agregarRepo()
        {
            var usuario = Preferences.Get("login", "");
            var deserialize = JsonConvert.DeserializeObject<MLogin>(usuario);
            string usucodigo = deserialize.usu_codigo.ToString();
            URL = URL.Replace('\n', '/');
            var pedido = Preferences.Get("datosPedido", "");
            var deserializePedidos = JsonConvert.DeserializeObject<MPedidos>(pedido);
            string numeroComprobante = deserializePedidos.NumeroComprobante;
            int idCliente = deserializePedidos.IdCliente;
            string url = $"{URL}Pedidos/agregarRepo/{usucodigo}/{name}/{idCliente}/{numeroComprobante}";
            HttpResponseMessage req = cliente.GetAsync(url).Result;
        }
        private void agregarArticulo(MDetallePedido h)
        {
            var usuario = Preferences.Get("login", "");
            var deserialize = JsonConvert.DeserializeObject<MLogin>(usuario);
            string usucodigo = deserialize.usu_codigo.ToString();
            //int seleccion = vm2.cantidadArticuloRepo(usucodigo, name.ToString(), h.CodTex, h.IdArticulos.ToString(), h.Adicional);
            double cantidadOriginal = 0;

            foreach (MDetallePedido item in listaOriginal)
            {
                if (item.IdArticulos == h.IdArticulos && item.CodTex == h.CodTex && item.Adicional == h.Adicional)
                {
                    cantidadOriginal = item.Cantidad;
                }
            }
            int id = vm2.id(usucodigo, name.ToString());
            double diferencia = cantidadOriginal - h.numPrep;
            vm2.agregarArticuloRepoDesdePedidos(id.ToString(), h.CodTex, h.IdArticulos.ToString(), h.Articulo, h.Adicional, usucodigo, h.numPrep, name.ToString(), diferencia, h.IdPedido);
        }
        private async void btnFinalizar_Clicked(object sender, EventArgs e)
        {
            bool terminada = true;
            foreach (MDetallePedido articulo in ListaProducto.ItemsSource)
            {
                if (articulo.CantidadPrep == false)
                {
                    terminada = false;
                }
            }
            if (terminada == true)
            {
                bool FaltaPreparacion = false;
                foreach (MDetallePedido item in ListaProducto.ItemsSource)
                {
                    if (item.hisCtrlprec == 1)
                    {
                        FaltaPreparacion = true;
                    }
                }
                if (FaltaPreparacion == false)
                {
                    vm.finalizarPedido(IdPedido, enObservacion.Text);
                    await DisplayAlert("Mensaje", "El pedido fue finalizado", "Ok");
                    await Navigation.PopAsync();
                }
                else if (FaltaPreparacion == true)
                {
                    vm.finalizarPedido(IdPedido, enObservacion.Text);
                    agregarRepo();
                    listaOriginal = vm.listaMovArticPed(IdPedido);
                    foreach (MDetallePedido item in ListaProducto.ItemsSource)
                    {
                        if (item.hisCtrlprec == 1)
                        {
                            agregarArticulo(item);
                        }
                    }
                    var usuario = Preferences.Get("login", "");
                    var deserialize = JsonConvert.DeserializeObject<MLogin>(usuario);
                    string usucodigo = deserialize.usu_codigo.ToString();
                    await DisplayAlert("Mensaje", "Se generó un pedido de reposición de los artículos correspondientes", "Ok");
                    vmEnCurso.ConfirmarListaEnCurso(usucodigo);
                    await Navigation.PopAsync();
                }
            }
            else
            {
                await DisplayAlert("Mensaje", "Para finalizar el pedido deben estar todos los items preparados", "Ok");
            }
        }

        private async void agregarCarro_Clicked(object sender, EventArgs e)
        {
            int etiqueta;
            bool resultado = await DisplayAlert("Mensaje", "¿Desea agregar un nuevo carro?", "Sí", "No");
            if (resultado)
            {
                etiqueta = agregarEtiqueta();
            }
        }

        public int agregarEtiqueta()
        {
            int etiqueta = 0;
            bool carroSinArticulo = true;
            foreach (MDetallePedido item in ListaProducto.ItemsSource)
            {
                if (item.numeroCarro == NumeroCarroActual)
                {
                    carroSinArticulo = false;
                }
            }
            if (carroSinArticulo)
            {
                DisplayAlert("Mensaje", $"El carro {NumeroCarroActual} debe tener por lo menos un artículo", "OK");
                etiqueta = 1;
            }
            else
            {
                NumeroCarroActual++;
                inicializarVista(IdPedido);
            }
            return etiqueta;
        }

        private async void btnFinalizarParcial_Clicked(object sender, EventArgs e)
        {
            int etiqueta = 0;
            bool terminada = true;
            foreach (MDetallePedido articulo in ListaProducto.ItemsSource)
            {
                if (articulo.CantidadPrep == false)
                {
                    terminada = false;
                }
            }
            if (terminada == false)
            {
                bool resultado = await DisplayAlert("Mensaje", "¿Desea realizar una finalización parcial?", "Sí", "No");
                if (resultado)
                {
                    etiqueta = agregarEtiqueta();
                    if (etiqueta == 0)
                    {
                        vmCarro.ComenzarNuevoParcial(IdPedido, enObservacion.Text);
                        vm.CarroYPreparacion(IdPedido,"parcial");
                    }

                }
            }
            else
            {
                await DisplayAlert("Mensaje", "Todos los productos estan terminados, debe finalizar de forma total", "Ok");
            }

        }
    }
}