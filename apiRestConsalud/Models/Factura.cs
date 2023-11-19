using System;
namespace apiRestConsalud.Models
{
	public class Factura
	{
        public float NumeroDocumento { get; set; }
        public float RUTVendedor { get; set; }
        public string DvVendedor { get; set; }
        public float RUTComprador { get; set; }
        public string DvComprador { get; set; }
        public string DireccionComprador { get; set; }
        public float ComunaComprador { get; set; }
        public float RegionComprador { get; set; }
        public float TotalFactura { get; set; }
        //public Detallefactura[] DetalleFactura { get; set; }
        public Detallefactura[] Detallefactura { get; internal set; }
    }
}

