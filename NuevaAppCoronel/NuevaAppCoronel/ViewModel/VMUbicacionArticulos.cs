using Newtonsoft.Json;
using NuevaAppCoronel.Model;
using NuevaAppCoronel.View.UbicacionArticulo;
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
    public class VMUbicacionArticulos
    {
        public static string ipUrl = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "url.txt");
        public static string URL = File.ReadAllText(ipUrl);
        HttpClient cliente = new HttpClient();
        public List<MUbicacionArticulos> ListaArticulos(string tipoBusqueda, string aux, int habilitado)
        {
            List<MUbicacionArticulos> listaArticulos = new List<MUbicacionArticulos>();
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Articulo/{tipoBusqueda}/{aux}/{habilitado}";
            HttpResponseMessage request = cliente.GetAsync(url).Result;
            if (request.IsSuccessStatusCode)
            {
                var json = request.Content.ReadAsStringAsync().Result;
                var response = JsonConvert.DeserializeObject<List<MUbicacionArticulos>>(json);
                if (response.Count > 0)
                {
                    foreach (var item in response)
                    {
                        var arti = new MUbicacionArticulos()
                        {
                            art_descri = item.art_descri,
                            imagen = item.imagen,
                            Codigo = item.Codigo,
                            Ubicacion = item.Ubicacion,
                            CBarra = item.CBarra,
                            art_codtex = item.art_codtex,
                            art_codnum = item.art_codnum,
                            art_codfab = item.art_codfab,
                            art_codinterno = item.art_codinterno,
                            adi_codigo = item.adi_codigo,
                            adi_descri = item.adi_descri,
                            Vigencia = item.Vigencia,
                            art_vigencia = item.art_vigencia,
                            ada_vigencia = item.ada_vigencia
                        };
                        listaArticulos.Add(arti);
                    }
                }
                else
                {
                    Application.Current.MainPage.DisplayAlert("Error", "No se encontro el articulo buscado!", "OK");
                }
            }
            return listaArticulos;
        }

        public List<MUbicacionArticulos> UbicacionArticulos(string aux)
        {
            List<MUbicacionArticulos> listaArticulos = new List<MUbicacionArticulos>();
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Articulo/BuscarUbicacion/{aux}";
            HttpResponseMessage request = cliente.GetAsync(url).Result;
            if (request.IsSuccessStatusCode)
            {
                var json = request.Content.ReadAsStringAsync().Result;
                var response = JsonConvert.DeserializeObject<List<MUbicacionArticulos>>(json);
                if (response.Count > 0)
                {
                    foreach (var item in response)
                    {
                        var arti = new MUbicacionArticulos()
                        {
                            art_descri = item.art_descri,
                            imagen = item.imagen,
                            Codigo = item.Codigo,
                            Ubicacion = item.Ubicacion,
                            CBarra = item.CBarra,
                            art_codtex = item.art_codtex,
                            art_codnum = item.art_codnum,
                            art_codfab = item.art_codfab,
                            art_codinterno = item.art_codinterno,
                            adi_codigo = item.adi_codigo,
                            adi_descri = item.adi_descri,
                            Vigencia = item.Vigencia,
                            art_vigencia = item.art_vigencia,
                            ada_vigencia = item.ada_vigencia
                        };
                        listaArticulos.Add(arti);
                    }
                }
                else
                {
                    Application.Current.MainPage.DisplayAlert("Error", "No se encontro el articulo buscado!", "OK");
                }
            }
            return listaArticulos;
        }

        public List<MUbicacion> ubicacion(string codtex, string codnum, string adicional)
        {
            if (adicional == "")
            {
                adicional = "null";
            }
            List<MUbicacion> listaArticulos = new List<MUbicacion>();
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Articulo/Ubicacion/{codtex}/{codnum}/{adicional}";
            HttpResponseMessage request = cliente.GetAsync(url).Result;
            if (request.IsSuccessStatusCode)
            {
                var json = request.Content.ReadAsStringAsync().Result;
                var response = JsonConvert.DeserializeObject<List<MUbicacion>>(json);
                if (response.Count > 0)
                {
                    foreach (var item in response)
                    {
                        MUbicacion ubi = new MUbicacion()
                        {
                            ubicacion = item.ubicacion,
                            deposito = item.deposito,
                            ubi_codtex = item.ubi_codtex,
                            ubi_adicional = item.ubi_adicional,
                            ubi_codnum = item.ubi_codnum,
                            ubi_codigo = item.ubi_codigo,
                            ubi_deposito = item.ubi_deposito,
                            ubi_predef = item.ubi_predef,
                            ubi_ubica1 = item.ubi_ubica1,
                            ubi_ubica2 = item.ubi_ubica2,
                            ubi_ubica3 = item.ubi_ubica3,
                            ubi_ubica4 = item.ubi_ubica4
                        };
                        listaArticulos.Add(ubi);
                        var serialize = JsonConvert.SerializeObject(ubi);
                        Preferences.Set("dato", serialize);
                    }
                }
            }
            return listaArticulos;
        }

        public string deposito(string ubi_ubica1)
        {
            List<MUbicacion> listaArticulos = new List<MUbicacion>();
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Articulo/Deposito/{ubi_ubica1}";
            HttpResponseMessage request = cliente.GetAsync(url).Result;
            string depo = "";
            if (request.IsSuccessStatusCode)
            {
                var json = request.Content.ReadAsStringAsync().Result;
                var response = JsonConvert.DeserializeObject<List<MUbicacion>>(json);
                if (response.Count > 0)
                {
                    foreach (var item in response)
                    {
                        depo = item.dep_descri;
                    }
                }

            }
            return depo;
        }
        public ObservableCollection<MUbicacion> depositoParticular(string ubi_ubica1)
        {
            ObservableCollection<MUbicacion> listaDepositos = new ObservableCollection<MUbicacion>();
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Articulo/Deposito/{ubi_ubica1}";
            HttpResponseMessage request = cliente.GetAsync(url).Result;
            if (request.IsSuccessStatusCode)
            {
                var json = request.Content.ReadAsStringAsync().Result;
                var response = JsonConvert.DeserializeObject<List<MUbicacion>>(json);
                if (response.Count > 0)
                {
                    foreach (var item in response)
                    {
                        MUbicacion ubi = new MUbicacion()
                        {
                            ubi_codigo = item.ubi_codigo,
                            dep_descri = item.dep_descri
                        };
                        listaDepositos.Add(ubi);
                    }
                }

            }
            return listaDepositos;
        }

        public ObservableCollection<MUbicacion> ListaDepositos()
        {
            ObservableCollection<MUbicacion> listaDepositos = new ObservableCollection<MUbicacion>();
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Articulo/ListaDepositos";
            HttpResponseMessage request = cliente.GetAsync(url).Result;
            if (request.IsSuccessStatusCode)
            {
                var json = request.Content.ReadAsStringAsync().Result;
                var response = JsonConvert.DeserializeObject<List<MUbicacion>>(json);
                if (response.Count > 0)
                {
                    foreach (var item in response)
                    {
                        MUbicacion ubi = new MUbicacion()
                        {
                            ubi_codigo = item.ubi_codigo,
                            dep_descri = item.dep_descri
                        };
                        listaDepositos.Add(item);
                    }
                }
            }
            return listaDepositos;
        }
        public int codeposito(string ubi_ubica1)
        {
            List<MUbicacion> listaArticulos = new List<MUbicacion>();
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Articulo/Deposito/{ubi_ubica1}";
            HttpResponseMessage request = cliente.GetAsync(url).Result;
            int depo = 0;
            if (request.IsSuccessStatusCode)
            {
                var json = request.Content.ReadAsStringAsync().Result;
                var response = JsonConvert.DeserializeObject<List<MUbicacion>>(json);
                if (response.Count > 0)
                {
                    foreach (var item in response)
                    {
                        depo = item.ubi_deposito;
                    }
                }

            }
            return depo;
        }

        public void buscarVigencia(string codtex, string codnum, string adi)
        {
            if (adi == "")
            {
                adi = "null";
            }
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Articulo/Vigencia/{codtex}/{codnum}/{adi}";
            HttpResponseMessage request = cliente.GetAsync(url).Result;
            if (request.IsSuccessStatusCode)
            {
                var json = request.Content?.ReadAsStringAsync().Result;
                var response = JsonConvert.DeserializeObject<List<MUbicacion>>(json);
                if (response.Count > 0)
                {
                    foreach (var item in response)
                    {
                        AgregarArticulo.Vigencia = item.Vigencia;
                        AgregarArticulo.art_vigencia = item.ubi_codigo.ToString();
                    }
                }
            }
        }

        public void buscarPermisos(string id)
        {
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Articulo/Permiso/{id}";
            HttpResponseMessage request = cliente.GetAsync(url).Result;
            if (request.IsSuccessStatusCode)
            {
                var json = request.Content?.ReadAsStringAsync().Result;
                var response = JsonConvert.DeserializeObject<List<MLogin>>(json);
                if (response.Count > 0)
                {
                    AgregarArticulo.permiso = 1;
                }
                else
                {
                    AgregarArticulo.permiso = 0;
                }
            }
        }
        public void habilitar(string codtex, string codnum, string ubiadi, string artvigencia, string ubiusualta)
        {
            if (ubiadi == "")
            {
                ubiadi = "null";
            }
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Articulo/HabilitarArticulo/{codtex}/{codnum}/{ubiadi}/{artvigencia}/{ubiusualta}";
            HttpResponseMessage request = cliente.GetAsync(url).Result;
            if (request.IsSuccessStatusCode)
            {

            }
        }

        public void agregarUbicacion(string ubicodigo, string codtex, string codnum, string ubica1, string ubica2, string ubica3, string ubica4, string ubiadi, string usualta, string ubidepo)
        {
            if (ubiadi == "")
            {
                ubiadi = "null";
            }
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Articulo/AgregarArticulo/{ubicodigo}/{codtex}/{codnum}/{ubica1}/{ubica2}/{ubica3}/{ubica4}/{ubiadi}/{usualta}/{ubidepo}";
            HttpResponseMessage request = cliente.GetAsync(url).Result;
            if (request.IsSuccessStatusCode)
            {
                Application.Current.MainPage.DisplayAlert("Mensaje", "Agregado con éxito!", "OK");
            }
        }

        public void eliminarUltimaUbicacion(string ubicodigo, string codtex, string codnum, string ubiadi, string usualta, string artvigencia)
        {

            if (AgregarArticulo.permiso == 1)
            {
                if (ubiadi == "")
                {
                    ubiadi = "null";
                }
                URL = URL.Replace('\n', '/');
                string url = $"{URL}Articulo/EliminarArticulo/{ubicodigo}/{codtex}/{codnum}/{ubiadi}/{usualta}/{artvigencia}";
                HttpResponseMessage request = cliente.GetAsync(url).Result;
                if (request.IsSuccessStatusCode)
                {
                    Application.Current.MainPage.DisplayAlert("Mensaje", "Eliminado con éxito", "OK");
                }
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Mensaje", "No tiene permisos para eliminar la ubicacion", "OK");
            }
        }
        public void eliminarUbicacion(string ubicodigo, string codtex, string codnum, string ubiadi, string usualta, string artvigencia)
        {
            if (ubiadi == "")
            {
                ubiadi = "null";
            }
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Articulo/EliminarArticulo/{ubicodigo}/{codtex}/{codnum}/{ubiadi}/{usualta}/{artvigencia}";
            HttpResponseMessage request = cliente.GetAsync(url).Result;
            if (request.IsSuccessStatusCode)
            {
                Application.Current.MainPage.DisplayAlert("Mensaje", "Eliminado con éxito", "OK");
            }
        }

        public List<MEtiqueta> listaUbicacionesImprimir(int impterminal)
        {
            List<MEtiqueta> etiqueta = new List<MEtiqueta>();
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Etiquetas/ListaImprimir/{impterminal}";
            HttpResponseMessage request = cliente.GetAsync(url).Result;
            if (request.IsSuccessStatusCode)
            {
                var json = request.Content.ReadAsStringAsync().Result;
                var response = JsonConvert.DeserializeObject<List<MEtiqueta>>(json);
                if (response.Count > 0)
                {
                    foreach (var item in response)
                    {
                        var arti = new MEtiqueta()
                        {
                            imp_descri = item.imp_descri,
                            Codigo = item.Codigo,
                            imp_adicional = item.imp_adicional,
                            imp_codnum = item.imp_codnum,
                            imp_codtex = item.imp_codtex
                        };
                        etiqueta.Add(arti);
                    }
                }
            }
            return etiqueta;
        }

        public int cantidadUbi(int impterminal)
        {
            int cantidad = 0;
            List<MEtiqueta> etiqueta = new List<MEtiqueta>();
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Etiquetas/ListaImprimir/{impterminal}";
            HttpResponseMessage request = cliente.GetAsync(url).Result;
            if (request.IsSuccessStatusCode)
            {
                var json = request.Content.ReadAsStringAsync().Result;
                var response = JsonConvert.DeserializeObject<List<MEtiqueta>>(json);
                if (response.Count > 0)
                {
                    cantidad = 1;
                }
            }
            return cantidad;
        }

        public List<MEtiqueta> listaEtiquetasImprimir(int impterminal)
        {
            List<MEtiqueta> etiqueta = new List<MEtiqueta>();
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Etiquetas/ListaImprimirEtiqueta/{impterminal}";
            HttpResponseMessage request = cliente.GetAsync(url).Result;
            if (request.IsSuccessStatusCode)
            {
                var json = request.Content.ReadAsStringAsync().Result;
                var response = JsonConvert.DeserializeObject<List<MEtiqueta>>(json);
                if (response.Count > 0)
                {
                    foreach (var item in response)
                    {
                        var arti = new MEtiqueta()
                        {
                            imp_descri = item.imp_descri,
                            Codigo = item.Codigo,
                            imp_adicional = item.imp_adicional,
                            imp_codnum = item.imp_codnum,
                            imp_codtex = item.imp_codtex,
                            imp_cantimp = item.imp_cantimp
                        };
                        etiqueta.Add(arti);
                    }
                }
            }
            return etiqueta;
        }

        public int cantidadEtiquetas(int impterminal)
        {
            int cantidad = 0;
            List<MEtiqueta> etiqueta = new List<MEtiqueta>();
            URL = URL.Replace('\n', '/');
            string url = $"{URL}Etiquetas/ListaImprimirEtiqueta/{impterminal}";
            HttpResponseMessage request = cliente.GetAsync(url).Result;
            if (request.IsSuccessStatusCode)
            {
                var json = request.Content.ReadAsStringAsync().Result;
                var response = JsonConvert.DeserializeObject<List<MEtiqueta>>(json);
                if (response.Count > 0)
                {
                    cantidad = 1;
                }
            }
            return cantidad;
        }
    }
}
