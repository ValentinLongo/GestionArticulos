using Newtonsoft.Json;
using NuevaAppCoronel.Model;
using NuevaAppCoronel.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NuevaAppCoronel.ViewModel
{
    public class VMEnCurso : BaseViewModel
    {
        public static string ipUrl = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "url.txt");
        public static string URL = File.ReadAllText(ipUrl);
        public static string term = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "terminal.txt");
        public static int terminal = Convert.ToInt32(File.ReadAllText(term));
        HttpClient cliente = new HttpClient();


        public List<MListaFaltante> listaEnCurso()
        {
            var data = Preferences.Get("login", "");
            var deserialize = JsonConvert.DeserializeObject<MLogin>(data);
            var idUusario = Convert.ToString(deserialize?.usu_codigo);
            List<MListaFaltante> lista = new List<MListaFaltante>();
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Faltante/encurso/{idUusario}";
            HttpResponseMessage req = cliente.GetAsync(url).Result;
            if (req.IsSuccessStatusCode)
            {
                var json = req.Content.ReadAsStringAsync().Result;
                var res = JsonConvert.DeserializeObject<List<MListaFaltante>>(json);

                foreach (var item in res)
                {
                    MListaFaltante oLista = new MListaFaltante()
                    {
                        comentario = item.comentario,
                        nomUser = item.nomUser,
                        DateTime = item.DateTime
                    };
                    lista.Add(oLista);
                }
            }
            return lista;
        }

        public async void ConfirmarListaEnCurso(string usucodigo)
        {
            //var url = new Uri(string.Concat($"{URL}Faltante/ConfirmarListaEnCurso/", usucodigo));
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Faltante/ConfirmarListaEnCurso/{usucodigo}";
            HttpResponseMessage req = cliente.GetAsync(url).Result;
            if (req.IsSuccessStatusCode)
            {
                await Application.Current.MainPage.DisplayAlert("Mensaje", "Lista Confirmada", "ok");
            }
        }
        public void EliminarArticuloLista(string usucodigo, string terminal, string codtex, string codnum, string adicional)
        {
            string url = $"{URL}Faltante/EliminarArticuloDetalle/{usucodigo}/{terminal}/{codtex}/{codnum}/{adicional}";
            HttpResponseMessage req = cliente.GetAsync(url).Result;
            if (req.IsSuccessStatusCode)
            {
            }
        }
        public void EliminarListaEnCurso()
        {
            var data = Preferences.Get("login", "");
            var deserialize = JsonConvert.DeserializeObject<MLogin>(data);
            var usucodigo = Convert.ToString(deserialize?.usu_codigo);
            string url = $"{URL}Faltante/EliminarListaEnCurso/{usucodigo}";
            HttpResponseMessage req = cliente.GetAsync(url).Result;
            if (req.IsSuccessStatusCode)
            { 
            }
        }
    }
}
