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
    public class VMLogin : BaseViewModel
    {
        public static string ipUrl = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "url.txt");
        public static string URL = File.ReadAllText(ipUrl);
        public static string term = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "terminal.txt");
        public static int terminal = Convert.ToInt32(File.ReadAllText(term));
        HttpClient cliente = new HttpClient();

        public static int idUsuario;

        private ObservableCollection<MLogin> listaUsuarios;
        public ObservableCollection<MLogin> ListaUsuarios
        {
            get { return listaUsuarios; }
            set { listaUsuarios = value; OnPropertyChanged(); }
        }

        public ObservableCollection<MLogin> GetUsuario(string login, string pass)
        {
            login = login.ToUpper();
            pass = pass.ToUpper();
            ListaUsuarios = new ObservableCollection<MLogin>();
            try
            {
                URL = URL.Replace('\n', '/');
                string url = "" + URL + "Usuario/" + login + "/" + pass + "";
                HttpResponseMessage req = cliente.GetAsync(url).Result;
                if (req.IsSuccessStatusCode)
                {
                    var json = req.Content.ReadAsStringAsync().Result;
                    var res = JsonConvert.DeserializeObject<ObservableCollection<MLogin>>(json);
                    if (res.Count == 0)
                    {
                        DisplayAlert("Mensaje", "Los datos ingresados son incorrectos, Corroborelos", "OK");
                    }
                    else
                    {
                        foreach (var item in res)
                        {
                            MLogin log = new MLogin
                            {
                                usu_codigo = item.usu_codigo,
                                usu_nombre = item.usu_nombre,
                                usu_login = item.usu_login,
                                usu_contraseña = item.usu_contraseña
                            };
                            idUsuario = item.usu_codigo;
                            ListaUsuarios.Add(log);
                            var serialize = JsonConvert.SerializeObject(ListaUsuarios[0]);
                            if (ListaUsuarios != null && ListaUsuarios.Count > 0)
                            {
                                Preferences.Set("login", serialize);
                                Application.Current.MainPage = new PaginaInicio();
                            }
                        }
                    }
                }
                else
                {
                    DisplayAlert("Mensaje", "Complete Campos", "OK");

                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Mensaje", "" + ex.Message, "OK");
            }
            return ListaUsuarios;
        }
    }
}
