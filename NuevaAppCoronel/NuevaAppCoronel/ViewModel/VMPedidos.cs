using Newtonsoft.Json;
using NuevaAppCoronel.Model;
using NuevaAppCoronel.View.PreparacionPedidos;
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
    public class VMPedidos
    {
        HttpClient cliente = new HttpClient();
        public static string ipUrl = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "url.txt");
        public static string URL = File.ReadAllText(ipUrl);
        public static string term = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "terminal.txt");
        public static int terminal = Convert.ToInt32(File.ReadAllText(term));

        public static string NombreVendedor;
        public static int CuentaCliente;
        public List<MPedidos> listaPedidos()
        {
            List<MPedidos> lista = new List<MPedidos>();
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Pedidos/Pedidos";
            HttpResponseMessage req = cliente.GetAsync(url).Result;
            if (req.IsSuccessStatusCode)
            {
                var json = req.Content.ReadAsStringAsync().Result;
                var res = JsonConvert.DeserializeObject<List<MPedidos>>(json);
                foreach (var pedido in res)
                {
                    MPedidos mpedido = new MPedidos()
                    {
                        IdPedido = pedido.IdPedido,
                        FechaPedido = pedido.FechaPedido,
                        IdCliente = pedido.IdCliente,
                        Cliente = pedido.Cliente,
                        EstadoPedido = pedido.EstadoPedido,
                        NumeroComprobante = pedido.NumeroComprobante,
                        TipMov = pedido.TipMov,
                        NIndex = pedido.NIndex,
                        nombreVendedor = pedido.nombreVendedor,
                        vtaObserva = pedido.vtaObserva,
                        CantidadItems = pedido.CantidadItems
                    };
                    lista.Add(mpedido);
                    NombreVendedor = mpedido.nombreVendedor;
                    CuentaCliente = mpedido.IdCliente;
                }
            }
            return lista;
        }

        public List<MPedidos> listaEnCurso()
        {
            List<MPedidos> lista = new List<MPedidos>();
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Pedidos/EnCurso";
            HttpResponseMessage req = cliente.GetAsync(url).Result;
            if (req.IsSuccessStatusCode)
            {
                var json = req.Content.ReadAsStringAsync().Result;
                var res = JsonConvert.DeserializeObject<List<MPedidos>>(json);
                foreach (var pedido in res)
                {
                    MPedidos mpedido = new MPedidos()
                    {
                        IdPedido = pedido.IdPedido,
                        FechaPedido = pedido.FechaPedido,
                        IdCliente = pedido.IdCliente,
                        Cliente = pedido.Cliente,
                        EstadoPedido = pedido.EstadoPedido,
                        NumeroComprobante = pedido.NumeroComprobante,
                        TipMov = pedido.TipMov,
                        NIndex = pedido.NIndex,
                        nombreVendedor = pedido.nombreVendedor,
                        vtaObserva = pedido.vtaObserva,
                        UsuModif = pedido.UsuModif
                    };
                    lista.Add(mpedido);
                    NombreVendedor = mpedido.nombreVendedor;
                    CuentaCliente = mpedido.IdCliente;
                }
            }
            return lista;
        }

        //public List<MPedidos> listaPendiente()
        //{
        //    List<MPedidos> lista = new List<MPedidos>();
        //    URL = URL.Replace('\n', '/');
        //    string url = $"{URL}Pedidos/ListaPedidoPreparacion";
        //    HttpResponseMessage req = cliente.GetAsync(url).Result;
        //    if (req.IsSuccessStatusCode)
        //    {
        //        var json = req.Content.ReadAsStringAsync().Result;
        //        var res = JsonConvert.DeserializeObject<List<MPedidos>>(json);
        //        foreach (var pedido in res)
        //        {
        //            MPedidos mpedido = new MPedidos()
        //            {
        //                IdPedido = pedido.IdPedido,
        //                FechaPedido = pedido.FechaPedido,
        //                IdCliente = pedido.IdCliente,
        //                Cliente = pedido.Cliente,
        //                EstadoPedido = pedido.EstadoPedido,
        //                NumeroComprobante = pedido.NumeroComprobante,
        //                TipMov = pedido.TipMov,
        //                NIndex = pedido.NIndex,
        //                nombreVendedor = pedido.nombreVendedor,
        //                vtaObserva = pedido.vtaObserva
        //            };
        //            lista.Add(mpedido);
        //            NombreVendedor = mpedido.nombreVendedor;
        //            CuentaCliente = mpedido.IdCliente;
        //        }
        //    }
        //    return lista;
        //}

        public void iniciarPedido(int idPedido, string tipMov, string numeroComprobante, int idUsuario)
        {
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Pedidos/ComienzoPedido/{idPedido}/{tipMov}/{numeroComprobante}/{idUsuario}/{terminal}";
            HttpResponseMessage req = cliente.GetAsync(url).Result;
            if (req.IsSuccessStatusCode)
            {
            }
        }

        public int validarModificacionEnCurso(int idPedido)
        {
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Pedidos/ValidarModificar/{idPedido}";
            HttpResponseMessage req = cliente.GetAsync(url).Result;
            if (req.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int validarModificacionEnCursoPorUsuario(int IdUsuario)
        {
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Pedidos/ValidarModificarPorUsuario/{IdUsuario}";
            HttpResponseMessage req = cliente.GetAsync(url).Result;
            if (req.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public void finalizarPedido(int vtaCodigo, string vtaObserva)
        {
            var data = Preferences.Get("login", "");
            var deserialize = JsonConvert.DeserializeObject<MLogin>(data);
            int IdUsuario = Convert.ToInt32(deserialize?.usu_codigo);
            MFinalizarPedido mFinalizarPedido = new MFinalizarPedido()
            {
                vta_codigo = vtaCodigo,
                idUsuario = IdUsuario,
                vta_observa = vtaObserva,
                numeroReferencia = DetallePedido.NumeroPreparacionActual

            };
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Pedidos/finalizarPedido";
            var serialize = JsonConvert.SerializeObject(mFinalizarPedido);
            var contenido = new StringContent(serialize, Encoding.UTF8, "application/json");
            HttpResponseMessage req = cliente.PostAsync(url, contenido).Result;

            //Impresion etiqueta
            string url2 = $"{URL}Carro/agregarEtiqueta/{DetallePedido.NumeroPreparacionActual}/{1}";
            HttpResponseMessage req2 = cliente.GetAsync(url2).Result;
            if (req.IsSuccessStatusCode)
            {
            }
        }
        //public void finalizarPedidoConPedRem(int vtaCodigo, string vtaObserva)
        //{
        //    var data = Preferences.Get("login", "");
        //    var deserialize = JsonConvert.DeserializeObject<MLogin>(data);
        //    int IdUsuario = Convert.ToInt32(deserialize?.usu_codigo);
        //    MFinalizarPedido mFinalizarPedido = new MFinalizarPedido()
        //    {
        //        vta_codigo = vtaCodigo,
        //        idUsuario = IdUsuario,
        //        vta_observa = vtaObserva,
        //        numeroReferencia = DetallePedido.NumeroPreparacionActual
        //    };
        //    URL = URL.Replace('\n', '/');
        //    string url = $"{URL}Pedidos/finalizarPedidoConPendiente";
        //    var serialize = JsonConvert.SerializeObject(mFinalizarPedido);
        //    var contenido = new StringContent(serialize, Encoding.UTF8, "application/json");
        //    HttpResponseMessage req = cliente.PostAsync(url, contenido).Result;
        //}

        public List<MDetallePedido> listaDetallePedido(int idPedido, int numeroCarroActual)
        {
            List<MDetallePedido> lista = new List<MDetallePedido>();
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Pedidos/DetallePedido/{idPedido}";
            HttpResponseMessage req = cliente.GetAsync(url).Result;
            if (req.IsSuccessStatusCode)
            {
                var json = req.Content.ReadAsStringAsync().Result;
                var res = JsonConvert.DeserializeObject<List<MDetallePedido>>(json);
                foreach (var pedido in res)
                {
                    MDetallePedido mpedido = new MDetallePedido()
                    {
                        Articulo = pedido.Articulo,
                        Cantidad = pedido.Cantidad,
                        IdArticulos = pedido.IdArticulos,
                        IdPedido = pedido.IdPedido,
                        Adicional = pedido.Adicional,
                        CodTex = pedido.CodTex,
                        DescAdicional = pedido.DescAdicional,
                        Imagen = pedido.Imagen,
                        Ubicacion = pedido.Ubicacion,
                        CantidadPrep = pedido.CantidadPrep,
                        numPrep = pedido.numPrep,
                        Orden = pedido.Orden,
                        CtaCli = pedido.CtaCli,
                        hisCtrlprec = pedido.hisCtrlprec,
                        codTexYCodNum = pedido.CodTex + " - " + pedido.IdArticulos,
                        artVigencia = pedido.artVigencia,
                        numeroCarro = pedido.numeroCarro,
                        RefPrepara = pedido.RefPrepara
                    };
                    if (mpedido.artVigencia != 1)
                    {
                        mpedido.color = "#ebebeb";
                    }
                    lista.Add(mpedido);
                }
            }
            return lista;
        }

        public List<MDetallePedido> listaDetallePedidoConPendiente(int idPedido)
        {
            List<MDetallePedido> lista = new List<MDetallePedido>();
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Pedidos/DetallePedido/{idPedido}";
            HttpResponseMessage req = cliente.GetAsync(url).Result;
            if (req.IsSuccessStatusCode)
            {
                var json = req.Content.ReadAsStringAsync().Result;
                var res = JsonConvert.DeserializeObject<List<MDetallePedido>>(json);
                foreach (var pedido in res)
                {
                    MDetallePedido mpedido = new MDetallePedido()
                    {
                        Articulo = pedido.Articulo,
                        Cantidad = pedido.Cantidad,
                        IdArticulos = pedido.IdArticulos,
                        IdPedido = pedido.IdPedido,
                        Adicional = pedido.Adicional,
                        CodTex = pedido.CodTex,
                        DescAdicional = pedido.DescAdicional,
                        Imagen = pedido.Imagen,
                        Ubicacion = pedido.Ubicacion,
                        CantidadPrep = pedido.CantidadPrep,
                        numPrep = pedido.numPrep,
                        Orden = pedido.Orden,
                        CtaCli = pedido.CtaCli,
                        hisCtrlprec = pedido.hisCtrlprec,
                        codTexYCodNum = pedido.CodTex + " - " + pedido.IdArticulos,
                        artVigencia = pedido.artVigencia
                    };
                    if (mpedido.artVigencia != 1)
                    {
                        mpedido.color = "#ebebeb";
                    }
                    if (mpedido.hisCtrlprec == 1)
                    {
                        lista.Add(mpedido);
                    }
                }
            }
            return lista;
        }

        public void modificarCantidadPreparada(double cantidadPreparada, string CodigoPedido, string CodTex, string CodNum, string CodAdi)
        {
            MArticulo mArticulo = new MArticulo()
            {
                his_preparada = cantidadPreparada,
                codigoPedido = Convert.ToInt32(CodigoPedido),
                his_codtex = CodTex,
                his_codnum = Convert.ToInt32(CodNum),
                his_adicional = CodAdi,
                carro = DetallePedido.NumeroCarroActual,
                refPrepara = DetallePedido.NumeroPreparacionActual
            };
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Pedidos/ModificarCantidad";
            var serialize = JsonConvert.SerializeObject(mArticulo);
            var contenido = new StringContent(serialize, Encoding.UTF8, "application/json");
            HttpResponseMessage req = cliente.PostAsync(url, contenido).Result;
        }

        public void modificarCantidadReemplazo(double cantidadPreparada, string CodigoPedido, string CodTex, string CodNum, string CodAdi, string codtexRe, string codnumRe, string adicionalRe, string descriReemplazo)
        {
            if (adicionalRe == "")
            {
                adicionalRe = "null";
            }
            if (codtexRe.Contains("/"))
            {
                codtexRe = codtexRe.Replace("/", " ");
            }
            if (CodAdi.Contains("/"))
            {
                CodAdi = CodAdi.Replace("/", " ");
            }
            if (CodAdi == "")
            {
                CodAdi = "null";
            }
            if (descriReemplazo.Contains("/"))
            {
                descriReemplazo = descriReemplazo.Replace("/", " ");
            }
            if (descriReemplazo == "")
            {
                descriReemplazo = "null";
            }
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Pedidos/ModificarReemplazo/{cantidadPreparada}/{CodigoPedido}/{CodTex}/{CodNum}/{CodAdi}/{codtexRe}/{codnumRe}/{adicionalRe}/{descriReemplazo}";
            HttpResponseMessage req = cliente.GetAsync(url).Result;
        }

        public int agregarArticuloCombinado(MArticulo mArticulo)
        {
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Pedidos/AgregarCombinado";
            var serialize = JsonConvert.SerializeObject(mArticulo);
            var contenido = new StringContent(serialize, Encoding.UTF8, "application/json");
            HttpResponseMessage req = cliente.PostAsync(url, contenido).Result;

            if (req.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public void agregarArticuloCombinadoCualquierCant(MArticulo mArticulo)
        {
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Pedidos/AgregarCombinadoCantMod";
            var serialize = JsonConvert.SerializeObject(mArticulo);
            var contenido = new StringContent(serialize, Encoding.UTF8, "application/json");
            HttpResponseMessage req = cliente.PostAsync(url, contenido).Result;
        }

        public string aceptaReemplazo(int idPedido)
        {
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Pedidos/AceptaReemplazo/{idPedido}";
            HttpResponseMessage req = cliente.GetAsync(url).Result;
            if (req.IsSuccessStatusCode)
            {
                return "Acepta Reemplazo = SI";
            }
            else
            {
                return "Acepta Reemplazo = NO";
            }
        }

        public bool permisoParaModCant()
        {
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Pedidos/PermisoParaModif";
            HttpResponseMessage req = cliente.GetAsync(url).Result;
            if (req.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //SECCION PARA FUNCIONES EN CASO DE QUE PIDA QUE SUSANA COMPLETE EL PEDIDO
        public void modificarCantidadParaPedido(string cantidadPreparada, string CodigoPedido, string CodTex, string CodNum, string CodAdi)
        {
            MArticulo mArticulo = new MArticulo()
            {
                his_codigo = Convert.ToInt32(CodigoPedido),
                his_preparada = Convert.ToDouble(cantidadPreparada),
                his_codtex = CodTex,
                his_codnum = Convert.ToInt32(CodNum),
                his_adicional = CodAdi,
                carro = DetallePedido.NumeroCarroActual,
                refPrepara = DetallePedido.NumeroPreparacionActual
            };
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Pedidos/ModificarCantidadConRepo";
            var serialize = JsonConvert.SerializeObject(mArticulo);
            var contenido = new StringContent(serialize, Encoding.UTF8, "application/json");
            HttpResponseMessage req = cliente.PostAsync(url, contenido).Result;
        }

        //LISTA MovArticPed
        public List<MDetallePedido> listaMovArticPed(int hisCodigo)
        {
            List<MDetallePedido> lista = new List<MDetallePedido>();
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Pedidos/listaMovArticPed/{hisCodigo}";
            HttpResponseMessage req = cliente.GetAsync(url).Result;
            if (req.IsSuccessStatusCode)
            {
                var json = req.Content.ReadAsStringAsync().Result;
                var res = JsonConvert.DeserializeObject<List<MDetallePedido>>(json);
                foreach (var pedido in res)
                {
                    MDetallePedido mpedido = new MDetallePedido()
                    {
                        Articulo = pedido.Articulo,
                        Cantidad = pedido.Cantidad,
                        IdArticulos = pedido.IdArticulos,//codnum,
                        IdPedido = pedido.IdPedido,
                        Adicional = pedido.Adicional,
                        CodTex = pedido.CodTex,
                        DescAdicional = pedido.DescAdicional,
                        Ubicacion = pedido.Ubicacion,
                        CantidadPrep = pedido.CantidadPrep,
                        numPrep = pedido.numPrep,
                        Orden = pedido.Orden,
                        CtaCli = pedido.CtaCli,
                    };
                    lista.Add(mpedido);
                }
            }
            return lista;
        }

        //Numero de carro y Numero de preparacion actual
        public void CarroYPreparacion(int hisCodigo, [Optional] string parcial)
        {
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Carro/DatoCarroYRef/{hisCodigo}";
            HttpResponseMessage req = cliente.GetAsync(url).Result;
            if (req.IsSuccessStatusCode)
            {
                var json = req.Content.ReadAsStringAsync().Result;
                var res = JsonConvert.DeserializeObject<List<MCarro>>(json);
                foreach (var carro in res)
                {
                    if (parcial == "parcial")
                    {
                        DetallePedido.NumeroPreparacionActual = carro.NumeroRefPrep;
                    }
                    else
                    {
                        DetallePedido.NumeroCarroActual = carro.NumeroCarro;
                        DetallePedido.NumeroPreparacionActual = carro.NumeroRefPrep;
                    }
                }
            }
        }
    }
}
