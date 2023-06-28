using Newtonsoft.Json;
using NuevaAppCoronel.Model;
using NuevaAppCoronel.View.PreparacionPedidos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using Xamarin.Essentials;

namespace NuevaAppCoronel.ViewModel
{
    public class VMCarro
    {
        HttpClient cliente = new HttpClient();
        public static string ipUrl = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "url.txt");
        public static string URL = File.ReadAllText(ipUrl);
        public static string term = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "terminal.txt");
        public static int terminal = Convert.ToInt32(File.ReadAllText(term));

        public void ComenzarNuevoParcial(int vtaCodigo, string vtaObserva)
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
            string url = $"{URL}Carro/finalizacionParcial";
            var serialize = JsonConvert.SerializeObject(mFinalizarPedido);
            var contenido = new StringContent(serialize, Encoding.UTF8, "application/json");
            HttpResponseMessage req = cliente.PostAsync(url, contenido).Result;

            string url2 = $"{URL}Carro/agregarEtiqueta/{DetallePedido.NumeroPreparacionActual}/{0}";
            HttpResponseMessage req2 = cliente.GetAsync(url2).Result;
            if (req.IsSuccessStatusCode)
            {
            }
        }
    }
}
