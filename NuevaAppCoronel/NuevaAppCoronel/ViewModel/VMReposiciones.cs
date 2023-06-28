using Newtonsoft.Json;
using NuevaAppCoronel.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NuevaAppCoronel.ViewModel
{
    public class VMReposiciones
    {
        public static string ipUrl = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "url.txt");
        public static string URL = File.ReadAllText(ipUrl);
        public static string term = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "terminal.txt");
        public static int terminal = Convert.ToInt32(File.ReadAllText(term));
        HttpClient cliente = new HttpClient();


        public List<MDeposito> listaDepositos()
        {
            List<MDeposito> lista = new List<MDeposito>();
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Faltante/despositos";
            HttpResponseMessage req = cliente.GetAsync(url).Result;
            if (req.IsSuccessStatusCode)
            {
                var json = req.Content.ReadAsStringAsync().Result;
                var res = JsonConvert.DeserializeObject<List<MDeposito>>(json);

                foreach (var item in res)
                {
                    MDeposito depo = new MDeposito()
                    {
                        dep_codigo = item.dep_codigo,
                        dep_descri = item.dep_descri
                    };
                    lista.Add(depo);
                }
            }
            return lista;
        }

        public int cantidadArticuloRepo(string usucodigo, string terminal, string artcodtex, string artcodnum, string adicodigo)
        {
            int cant = 0;
            URL = URL.Replace('\n', '/');
            if (adicodigo == "" || adicodigo == null)
            {
                adicodigo = "null";
            }
            string url = $"{URL}Faltante/cantidadArticuloRepo/{usucodigo}/{terminal}/{artcodtex}/{artcodnum}/{adicodigo}";
            HttpResponseMessage req = cliente.GetAsync(url).Result;
            if (req.IsSuccessStatusCode)
            {
                var json = req.Content.ReadAsStringAsync().Result;
                var res = JsonConvert.DeserializeObject(json);
                cant = Convert.ToInt32(res);
            }
            return cant;
        }
        public int id(string usucodigo, string terminal)
        {
            int ID = 0;
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Faltante/idArticuloRepo/{usucodigo}/{terminal}";
            HttpResponseMessage req = cliente.GetAsync(url).Result;
            if (req.IsSuccessStatusCode)
            {
                var json = req.Content.ReadAsStringAsync().Result;
                var res = JsonConvert.DeserializeObject(json);
                ID = Convert.ToInt32(res);
            }
            return ID;
        }

        public void agregarArticuloRepo(string Seleccion, string detcodigo, string artcodtex, string artcodnum, string artdescri, string adicodigo, string usucodigo, string cantidad, string terminal, [Optional] string DeDonde)
        {
            //URL = URL.Replace('\n', '/');
            //if (artdescri.Contains("/"))
            //{
            //    artdescri = artdescri.Replace("/", " ");
            //}
            //if (adicodigo.Contains("/"))
            //{
            //    adicodigo = adicodigo.Replace("/", " ");
            //}
            //if (adicodigo == "" || adicodigo == null)
            //{
            //    adicodigo = "null";
            //}
            //string url = $"{URL}Faltante/agregarArticuloRepo/{seleccion}/{detcodigo}/{artcodtex}/{artcodnum}/{artdescri}/{adicodigo}/{usucodigo}/{cantidad}/{terminal}";
            //HttpResponseMessage req = cliente.GetAsync(url).Result;
            //if (req.IsSuccessStatusCode)
            //{
            //    if (DeDonde != "Pedidos")
            //    {
            //        Application.Current.MainPage.DisplayAlert("Mensaje", $"Se agregó correctamente {cantidad} unidad/es", "ok");
            //    }
            URL = URL.Replace('\n', '/');
            MArticulo mArticulo = new MArticulo()
            {
                seleccion = Convert.ToInt32(Seleccion),
                codigoPedido = Convert.ToInt32(detcodigo),
                his_codtex = artcodtex,
                his_codnum = Convert.ToInt32(artcodnum),
                his_articulo = artdescri,
                his_adicional = adicodigo,
                his_cantidad = Convert.ToDouble(cantidad),
                Terminal = Convert.ToInt32(terminal),
                usu_codigo = Convert.ToInt32(usucodigo)
            };
            string url = $"{URL}Faltante/agregarArticuloRepo";
            var serialize = JsonConvert.SerializeObject(mArticulo);
            var contenido = new StringContent(serialize, Encoding.UTF8, "application/json");
            HttpResponseMessage req = cliente.PostAsync(url, contenido).Result;
            if (req.IsSuccessStatusCode)
            {
                if (DeDonde != "Pedidos")
                {
                    Application.Current.MainPage.DisplayAlert("Mensaje", $"Se agregó correctamente {cantidad} unidad/es", "ok");
                }
            }
            //}
        }


        public List<MDetalleLista> listaDetalleNuevo()
        {
            var usuario = Preferences.Get("login", "");
            var deserialize = JsonConvert.DeserializeObject<MLogin>(usuario);
            string usucodigo = deserialize.usu_codigo.ToString();
            List<MDetalleLista> lista = new List<MDetalleLista>();
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Faltante/ListaDetalle/{usucodigo}";
            HttpResponseMessage req = cliente.GetAsync(url).Result;
            if (req.IsSuccessStatusCode)
            {
                var json = req.Content.ReadAsStringAsync().Result;
                var res = JsonConvert.DeserializeObject<List<MDetalleLista>>(json);

                foreach (var item in res)
                {
                    MDetalleLista depo = new MDetalleLista()
                    {
                        aux_descri = item.aux_descri,
                        codigo = item.codigo,
                        adi_descri = item.adi_descri,
                        aux_cantidad = item.aux_cantidad,
                        aux_codnum = item.aux_codnum,
                        aux_codtex = item.aux_codtex,
                        aux_adicional = item.aux_adicional
                    };
                    lista.Add(depo);
                }
            }
            return lista;
        }

        public List<MDetalleLista> listaDetalleRegistrados(int det_codigo)
        {
            List<MDetalleLista> lista = new List<MDetalleLista>();
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Faltante/ListaDetalleRegistrado/{det_codigo}";
            HttpResponseMessage req = cliente.GetAsync(url).Result;
            if (req.IsSuccessStatusCode)
            {
                var json = req.Content.ReadAsStringAsync().Result;
                var res = JsonConvert.DeserializeObject<List<MDetalleLista>>(json);

                foreach (var item in res)
                {
                    MDetalleLista depo = new MDetalleLista()
                    {
                        aux_descri = item.aux_descri,
                        codigo = item.codigo,
                        adi_descri = item.descriAdi,
                        aux_cantidad = item.aux_cantidad,
                        aux_codnum = item.aux_codnum,
                        aux_codtex = item.aux_codtex,
                        aux_adicional = item.aux_adicional,
                        estado = item.estado,
                        color = item.color,
                        det_check = item.det_check,
                        descriAdi = item.descriAdi,
                        Deposito = item.Deposito,
                        imagen = item.imagen
                    };
                    lista.Add(depo);
                }
            }
            return lista;
        }

        public void actualizarEstadoArticulo(string detcheck, string detcodtex, string detcodnum, string adicodigo, string detcodigo)
        {
            URL = URL.Replace('\n', '/');
            MArticulo mArticulo = new MArticulo()
            {
                codigoPedido = Convert.ToInt32(detcodigo),
                det_check = Convert.ToInt32(detcheck),
                his_codtex = detcodtex,
                his_codnum = Convert.ToInt32(detcodnum),
                his_adicional = adicodigo
            };
            string url = $"{URL}Faltante/ActualizarEstadoArticulo";
            var serialize = JsonConvert.SerializeObject(mArticulo);
            var contenido = new StringContent(serialize, Encoding.UTF8, "application/json");
            HttpResponseMessage req = cliente.PostAsync(url, contenido).Result;
            if (req.IsSuccessStatusCode)
            {
            }
        }

        public void finalizarListaRegistrados(int idFaltante)
        {
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Faltante/ConfirmarListaRegistrados/{idFaltante}";
            HttpResponseMessage req = cliente.GetAsync(url).Result;
            if (req.IsSuccessStatusCode)
            {
            }
        }

        //Para pedidos con pedido de repo
        public void agregarArticuloRepoDesdePedidos(string detcodigo, string artcodtex, string artcodnum, string artdescri, string adicodigo, string usucodigo, double cantidad, string terminal, double diferencia, int IdPedido)
        {
            MArticulo mArticulo = new MArticulo()
            {
                codigoPedido = Convert.ToInt32(detcodigo),
                his_codtex = artcodtex,
                his_codnum = Convert.ToInt32(artcodnum),
                his_articulo = artdescri,
                his_adicional = adicodigo,
                usu_codigo = Convert.ToInt32(usucodigo),
                his_cantidad = cantidad,
                Terminal = Convert.ToInt32(terminal),
                CantidadDiferencia = diferencia,
                his_codigo = IdPedido
            };
            string url = $"{URL}Pedidos/agregarArticuloRepo";
            var serialize = JsonConvert.SerializeObject(mArticulo);
            var contenido = new StringContent(serialize, Encoding.UTF8, "application/json");
            HttpResponseMessage req = cliente.PostAsync(url, contenido).Result;
            if (req.IsSuccessStatusCode)
            {
            }
        }
    }
}
