using Newtonsoft.Json;
using NuevaAppCoronel.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using Xamarin.Essentials;

namespace NuevaAppCoronel.ViewModel
{
    public class VMRegistrados
    {
        public static string ipUrl = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "url.txt");
        public static string URL = File.ReadAllText(ipUrl);
        public static string term = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "terminal.txt");
        public static int terminal = Convert.ToInt32(File.ReadAllText(term));
        HttpClient cliente = new HttpClient();


        public List<MListaFaltante> listaRegistrados()
        {
            var data = Preferences.Get("login", "");
            var deserialize = JsonConvert.DeserializeObject<MLogin>(data);
            var idUusario = Convert.ToString(deserialize?.usu_codigo);
            List<MListaFaltante> lista = new List<MListaFaltante>();
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Faltante/registrados";
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
                        DateTime = item.DateTime,
                        idFaltantes = item.idFaltantes
                    };
                    lista.Add(oLista);
                }
            }
            return lista;
        }
    }
}
