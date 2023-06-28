using System;
using System.Collections.Generic;
using System.Text;

namespace NuevaAppCoronel.Model
{
    public class MPedidos
    {
        public int IdPedido { get; set; } //Codigo del pedido vta_codigo
        public DateTime FechaPedido { get; set; } //vta_fecpro
        public int IdCliente { get; set; } // vta_ctacli
        public string Cliente { get; set; } //Nombre cliente vta_cliente
        public string EstadoPedido { get; set; } //vta_estado
        public string NumeroComprobante { get; set; } // vta_cpbte
        public int TipMov { get; set; } //vta_tipmov
        public int NIndex { get; set; }
        public string nombreVendedor { get; set; }
        public string vtaObserva { get; set; }
        public string UsuModif { get; set; }
        public int CantidadItems { get; set; }
        public int CodigoPrep { get; set; }
    }
}
