using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Essentials;

namespace NuevaAppCoronel.ViewModel
{
    public class VMUsuario
    {
        public int ComprobarConexion()
        {
            string ipUrl = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "url.txt");
            string term = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "terminal.txt");
            try
            {
                using (StreamReader reader = new StreamReader(ipUrl))
                {
                    string textoObtenido; //Creamos la variable que contendrá el texto
                    while ((textoObtenido = reader.ReadLine()) != null) //Leemos línea por línea
                    {
                        string obt = textoObtenido;
                        var ser = JsonConvert.SerializeObject(obt);
                        Preferences.Set("url", ser);
                    }
                };

                using (StreamReader reader = new StreamReader(term))
                {
                    string textoObtenido; //Creamos la variable que contendrá el texto
                    while ((textoObtenido = reader.ReadLine()) != null) //Leemos línea por línea
                    {
                        string obt = textoObtenido;
                        var ser = JsonConvert.SerializeObject(obt);
                        Preferences.Set("terminal", ser);
                    }
                };
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
