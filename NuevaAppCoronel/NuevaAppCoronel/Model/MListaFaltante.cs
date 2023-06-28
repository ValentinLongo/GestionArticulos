using System;
using System.Collections.Generic;
using System.Text;

namespace NuevaAppCoronel.Model
{
    public class MListaFaltante
    {
        public int idUser { get; set; }
        public string nomUser { get; set; }
        public int idDeposito { get; set; }
        public string comentario { get; set; }
        public int terminado { get; set; }
        public string Estado { get; set; }
        public DateTime DateTime { get; set; }
        public string horaFin { get; set; }
        public string fechaFin { get; set; }
        public int idFaltantes { get; set; }
        public string colorEstado { get; set; }
    }
}
